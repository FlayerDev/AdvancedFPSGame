using UnityEngine;

public class Item : MonoBehaviour, IUsable
{
    public WeaponType weaponType;
    [SerializeField] MonoBehaviour[] DisableOnDrop;
    [SerializeField] bool RigidBodyOnDrop = true;
    [SerializeField] Collider objCollider;
    public void drop()
    {
        gameObject.transform.parent = null;
        if (RigidBodyOnDrop) gameObject.AddComponent<Rigidbody>();
        objCollider.enabled = true;
        if (DisableOnDrop.Length > 0) foreach (MonoBehaviour obj in DisableOnDrop) obj.enabled = false;
        GenericUtilities.ChangeLayer(gameObject, 0, true);
    }

    public void use(GameObject user)
    {
        
    }

    private void Awake()
    {

    }
}
public enum WeaponType
{
    Main,
    Secondary,
    Utility
}
