using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpSwing : MonoBehaviour {

    public Sprite SwingSprite;

    private GameObject Fx;
        private GameObject CopyFx;
        public float LifeTime;
        public float MaxDistance;
        public Vector3 EffectOffSet;

    private Vector3 MousePoint;
    private Vector3 Base;
    private Vector3 MouseDir;

	void Start ()
    {
        Fx = new GameObject("Fx");
        Fx.AddComponent<SpriteRenderer>().sprite = SwingSprite;
	}

	// Update is called once per frame
	void Update ()
    {
        if (!GetComponent<combat>().attackBoolean())
        {
            CopyFx = Instantiate(Fx);
            CopyFx.AddComponent<DestroyOnTime>().lifetime = LifeTime;
            //CopyFx.AddComponent<FxFade>().Duration = LifeTime;
            ObjectPosition(CopyFx);
            CopyFx.transform.SetParent(transform);
        }
	}
    void ObjectPosition(GameObject Copy)
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        pz.Normalize(); // suunta
        //Copy.transform.position = transform.position + EffectOffSet + pz * MaxDistance;
        MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Base = transform.position + EffectOffSet;
        MouseDir = (MousePoint - transform.position - EffectOffSet).normalized * MaxDistance;

        GetAngleDegress(Copy);
        
    }
    void OnDrawGizmos()
    {
         Gizmos.DrawLine(Base, Base + MouseDir); // piirretään viiva visualisoimaan toimivuutta
    }
    void GetAngleDegress(GameObject Copy)
    {
        Vector3 Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var n = 180 - (Mathf.Atan2(transform.position.y - Copy.transform.position.y, transform.position.x - Copy.transform.position.x)) * 180 / Mathf.PI;
        //print(n); // Saatana mikä pritti :<
        Copy.transform.Rotate(0, 0, n * -1);
    }
   
}
