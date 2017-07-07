using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxScript : MonoBehaviour {

    public Sprite BareHandSprite;
    private GameObject Fx;
    private GameObject CopyFx;
    [Header("Effect settings")]
    public float LifeTime;
    public float MaxDistance;
    [Header("Default x: 0, y: 0.3, z: 0")]
    public Vector3 EffectOffSet;

    private Vector3 MousePoint;
    private Vector3 Base;
    private Vector3 MouseDir;

	void Start ()
    {
        Fx = new GameObject("Fx");
        Fx.AddComponent<SpriteRenderer>().sprite = BareHandSprite;
        Fx.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);

	}

    public void FxUpdate(Sprite NewFx)
    {
        if(NewFx != null && NewFx.name != Fx.GetComponent<SpriteRenderer>().name)
        {
            Fx.GetComponent<SpriteRenderer>().sprite = NewFx;
        } 
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) == true) // Myöhemmin tämä tarkistetaan eri paikasta
        {
            CopyFx = Instantiate(Fx);
            CopyFx.AddComponent<DestroyOnTime>().lifetime = LifeTime;
            CopyFx.AddComponent<FxFade>().Duration = LifeTime;
            ObjectPosition(CopyFx);
            CopyFx.transform.SetParent(transform);
        }
	}
    void ObjectPosition(GameObject Copy)
    {
        MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // hiiren sijainti
        Base = transform.position + EffectOffSet; //Missä on pelaajan base jonka ympärillä efekti rotatee
        MouseDir = (MousePoint - transform.position - EffectOffSet).normalized * MaxDistance; //mikä on hiiren suunta
        Copy.transform.position = Base + MouseDir; 
        GetAngleDegress(Copy);
    }
    //void OnDrawGizmos()
    //{
    //     Gizmos.DrawLine(Base, Base + MouseDir); // piirretään viiva visualisoimaan toimivuutta
    //}
    void GetAngleDegress(GameObject Copy)
    {
        Vector3 Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var n = 0 - (Mathf.Atan2(Base.y - Base.y + MouseDir.y,Base.x - Base.x + MouseDir.x)) * 180 / Mathf.PI; //origin - target
        print(n);
        Copy.transform.Rotate(0, 0, n * -1);
    }
   
}
