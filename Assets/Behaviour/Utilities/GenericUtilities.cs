using UnityEngine;

public static class GenericUtilities
{
    public static void ChangeLayer(GameObject gameObject, int layer, bool changeChildren = true)
    {
        gameObject.layer = layer;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.layer = layer;
        }
    }
}
