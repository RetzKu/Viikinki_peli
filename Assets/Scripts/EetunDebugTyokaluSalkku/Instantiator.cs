using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public static GameObject instantiateGo(GameObject go)
    {
        return Instantiate(go);
    }
}
