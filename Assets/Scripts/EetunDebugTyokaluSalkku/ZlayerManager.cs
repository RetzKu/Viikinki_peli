using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZlayerManager : MonoBehaviour
{
    public static float GetZFromY(Vector3 position)
    {
        return position.y / 10000f;
    }
}
