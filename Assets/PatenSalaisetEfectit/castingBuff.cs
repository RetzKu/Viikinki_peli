using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castingBuff : MonoBehaviour {

    public void init(GameObject FatherMichael)
    {        
        transform.parent = FatherMichael.transform;
        transform.position = FatherMichael.transform.position;
        Destroy(this.gameObject, 0.5f);
    }
}
