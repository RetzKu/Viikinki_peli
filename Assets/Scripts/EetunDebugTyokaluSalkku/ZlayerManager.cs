using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZlayerManager : MonoBehaviour
{
    // public static void GetZLayerValue(Vector2 position)
    // {
    // }

    // public static void GetZLayerValue(Vector3 position)
    // { 
    // }

    // http://answers.unity3d.com/questions/620318/sprite-layer-order-determined-by-y-value.html
    public static void SetSortingOrder(Transform transform, SpriteRenderer renderer)
    {
        int OrderOffset = 0;
        int pos = Mathf.RoundToInt(transform.position.y);
        pos /= 3;
        renderer.sortingOrder = (pos * -1) + OrderOffset;
    }

    public static float GetZFromY(Vector3 position)
    {
        float z = position.y / 10000f;
        return z;
    }
}
