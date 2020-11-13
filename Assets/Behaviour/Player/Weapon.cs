using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Item))]
public class Weapon : MonoBehaviour
{
    #region Properties
    #region Decals
    [Header("Decals")]
    public GameObject woodDecal;
    public GameObject metalDecal;
    public GameObject cobbleDecal;
    public GameObject syntheticDecal;
    public GameObject concreteDecal;
    public GameObject defaultDecal;
    #endregion
    [Space]
    [Header("Basic Settings")]
    public bool isWeaponAutomatic = true; //While enabled the weapon will automatically fire when the Shoot button is held
    GameObject muzzle;
    public float baseDamage = 32; // Initial damage of the weapon
    [Range(10f, 2000f)] public float effectiveRange = 500; // Max distance the bullet/Raycast will travel
    public float DistanceDropoff = .1f;// ![TO BE IMPLEMENTED]! <-----------------------------------------------------------------------------------------
    public float PenetrationPower = 1f;// Νeutralizes the wallbang's DamageDropoffPerMaterial
    [SerializeField] bool isArmed = true; // If enabled weapon will fire upon Fire button click
    public bool allowADS = false;
    [SerializeField] int RPM = 200; // Rounds Per Minute: MS between shots = 1000 / (RPM / 60)
    [Header("Recoil")]
    public bool doRecoil = true;
    [Range(.01f, 1f)] public float recoilReturnSpeed = 1f;
    public float VerticalRecoil = .4f;
    public float MaximumVerticalRecoil = 1f;
    public float HorizontalRecoil = .1f;
    public float HorizontalMultiplierOnMaxVertical = 3f;
    public float MaxNegativeHorizontalRecoil = 3f;

    float currentVerticalRecoil = 0;
    float currentHorizontalRecoil = 0;

    #endregion
    #region others
    private Action update;
    #endregion
    private void Awake()
    {
        muzzle = LocalInfo.muzzle;
        if (isWeaponAutomatic) { update += () => { if (Input.GetKey(LocalInfo.KeyBinds.Shoot)) fire(); }; }
        else {update += () =>  { if (Input.GetKeyDown(LocalInfo.KeyBinds.Shoot)) fire(); }; }

        //if (allowADS) update += () => { if (Input.GetKeyDown(LocalInfo.KeyBinds.ADS)) ; };
    }
    public void Update() => update();
    private void FixedUpdate()
    {
        currentHorizontalRecoil /= 1f + recoilReturnSpeed * Time.fixedDeltaTime;
        currentVerticalRecoil /= 1f + recoilReturnSpeed * Time.fixedDeltaTime;
    }
    async void rearm()
    {
        isArmed = false;
        var ms = RPM / 60;
        ms = 1000 / ms;
        await System.Threading.Tasks.Task.Delay(ms);
        isArmed = true;
    }

    public void fire()
    {
        if (!isArmed) return;
        rearm();
        float dmg = baseDamage;
        calculateRecoil();
        RaycastHit[] hitarr = Physics.RaycastAll(muzzle.transform.position,
            muzzle.transform.forward + new Vector3(0f, currentVerticalRecoil, currentHorizontalRecoil), effectiveRange);

        Array.Sort(hitarr, (x, y) => x.distance.CompareTo(y.distance)); // Sorts hit objects by distance
        foreach (RaycastHit item in hitarr)
        {
            if (item.collider.gameObject.TryGetComponent(out IDamageable dmgable)) applyDamage(dmgable, dmg);
            dmg = calculateDamage(dmg, item);
        }
    }
    #region Fire Functions
    void calculateRecoil()
    {
        if (currentVerticalRecoil < MaximumVerticalRecoil)
        {
            currentVerticalRecoil += VerticalRecoil / 10;
            currentHorizontalRecoil += HorizontalRecoil / 10;
        }
        else
        {
            currentHorizontalRecoil -= HorizontalRecoil * HorizontalMultiplierOnMaxVertical;
        }
    }
    float calculateDamage(float dmg, RaycastHit hit)
    {
        if (dmg == 0) return 0;
        Vector3 returnPoint = muzzle.transform.position + (hit.point - muzzle.transform.position).normalized * effectiveRange;
        hit.collider.Raycast(new Ray(returnPoint, (muzzle.transform.position - returnPoint).normalized)
            , out RaycastHit outhit, effectiveRange * 2);
        Vector3 inpoint = hit.point; Vector3 outpoint = outhit.point; // Gets coordinates of hit positions
        printBulletDecal(hit, outhit, inpoint, outpoint);
        if (!DamageDropoffPerMaterial.MaterialValue.TryGetValue(hit.collider.gameObject.tag, out float dropvalue)) return 0f;
        dmg -= Vector3.Distance(inpoint, outpoint) * (dropvalue / PenetrationPower);
        dmg = dmg < 999f && dmg > 0f ? dmg : (dmg > 999f ? 999f : 0f);
        return dmg;
    }
    //static RaycastHit findOppositeSide(Ray ray, GameObject gO)
    //{
    //    var res = Physics.RaycastAll(ray, 9999f);
    //    foreach (RaycastHit item in res) if (item.collider.gameObject == gO) return item;
    //    return new RaycastHit();
    //}
    void printBulletDecal(RaycastHit hit, RaycastHit outhit, Vector3 inpoint, Vector3 outpoint)
    {
        if (hit.collider.gameObject.CompareTag("Player")) return;
        var decal = decalDictionary(hit.collider.gameObject.tag) ?? defaultDecal;
        Instantiate(decal, inpoint, Quaternion.LookRotation(hit.normal));
        Instantiate(decal, outpoint, Quaternion.LookRotation(outhit.normal));
    }
    void applyDamage(IDamageable dmgable, float amount)
    {
        dmgable.damage(amount);
    }
    #endregion
    #region Dictionaries And Enums
    GameObject decalDictionary(string decal)
    {
        Dictionary<string, GameObject> decalDictionary = new Dictionary<string, GameObject>
        {
        {"SYNTHETIC", syntheticDecal},
        {"WOOD" , woodDecal},
        {"METAL", metalDecal},
        {"COBBLE", cobbleDecal},
        {"CONCRETE", concreteDecal}
        };
        if (decalDictionary.TryGetValue(decal, out GameObject returnObj)) return returnObj;
        else return null;
    }
}
static class DamageDropoffPerMaterial
{
    public static Dictionary<string, float> MaterialValue = new Dictionary<string, float>
    {
        {"SYNTHETIC", 8f},
        {"WOOD" , 16f},
        {"METAL", 32f},
        {"COBBLE", 40f},
        {"CONCRETE", 60f},
        {"Player", 20f}
    };
}

#endregion