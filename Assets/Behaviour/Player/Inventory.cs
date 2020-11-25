using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] weaponHolders = new GameObject[3];
    public int enabledIndex = 0;
    [Range(1f, 10f)] public float usableDistance = 5f;
    public bool allowBombPickup = false;
    void Start()
    {
        setIndex(0);
    }


    void Update()
    {
        if (Input.GetKeyDown(LocalInfo.KeyBinds.Use) && !LocalInfo.IsPaused) use();
        if (Input.GetKeyDown(LocalInfo.KeyBinds.InventoryDrop) && !LocalInfo.IsPaused) drop();
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0) incrementIndex(scrollValue < 0);
    }
    void use()
    {
        RaycastHit hit = (RaycastHit)LocalInfo.useRaycastHit;
        if (hit.collider.gameObject.TryGetComponent(out IUsable usable)) usable.use(gameObject);
    }

    void drop() => weaponHolders[enabledIndex].transform.GetChild(0).GetComponent<Item>().drop();
    void drop(int index)
    {
        weaponHolders[index].transform.GetChild(0).GetComponent<Item>().drop();
        setIndex(index);
    }

    #region ChangeWeapon
    void incrementIndex(bool dir)
    {
        int step = (dir ? 1 : -1);
        for (int i = 0,x = enabledIndex; i < weaponHolders.Length; i++)
        {
            x += step;
            if (x >= weaponHolders.Length) x = 0;
            if (x < 0) x = weaponHolders.Length - 1;
            if (weaponHolders[x].transform.childCount != 0)
            {
                enableIndex(x);
                enabledIndex = x;
                return;
            }
        }
    }
    void setIndex(int index)
    {
        for (int i = 0, x = index; i < weaponHolders.Length; i++)
        {
            x += 1;
            if (x >= weaponHolders.Length) x = 0;
            if (x < 0) x = weaponHolders.Length - 1;
            if (weaponHolders[x].transform.childCount != 0)
            {
                enableIndex(x);
                enabledIndex = x;
                return;
            }
        }
    }
    void enableIndex(int index)
    {
        for (int i = 0; i < weaponHolders.Length; i++)
        {
            if (i == index) weaponHolders[i].SetActive(true);
            else weaponHolders[i].SetActive(false);
        }
    }
    #endregion
    void refreshInventory()
    {

    }
}
