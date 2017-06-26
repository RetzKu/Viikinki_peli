using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour {

    public List<Sprites> PlayerSprites;

    private GameObject Player;

    private GameObject Torso;
    private GameObject Head;
    private GameObject LeftUpperArm;
    private GameObject LeftLowerArm;
    private GameObject LeftHand;
    private GameObject LeftUpperLeg;
    private GameObject LeftLowerLeg;
    private GameObject LeftBoot;
    private GameObject RightUpperArm;
    private GameObject RightLowerArm;
    private GameObject RightHand;
    private GameObject RightUpperLeg;
    private GameObject RightLowerLeg;
    private GameObject RightBoot;

    // Use this for initialization AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO  AYY LAMO EPOGKJFDSPOGKFDPOGKFDPOGKDFOGPKFDPO 
    void Start()
    {
        Player = transform.gameObject;
        Torso =         Player.transform.GetChild(0).gameObject;
        Head =          Player.transform.GetChild(0).GetChild(0).gameObject;
        LeftUpperArm =  Player.transform.GetChild(0).GetChild(1).gameObject;
        LeftLowerArm =  Player.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        LeftHand =      Player.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
        LeftUpperLeg =  Player.transform.GetChild(0).GetChild(2).gameObject;
        LeftLowerLeg =  Player.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        LeftBoot =      Player.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).gameObject;
        RightUpperArm = Player.transform.GetChild(0).GetChild(3).gameObject;
        RightLowerArm = Player.transform.GetChild(0).GetChild(3).GetChild(0).gameObject;
        RightHand =     Player.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject;
        RightUpperLeg = Player.transform.GetChild(0).GetChild(4).gameObject;
        RightLowerLeg = Player.transform.GetChild(0).GetChild(4).GetChild(0).gameObject;
        RightBoot =     Player.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
    
    void CheckInput()
    {
        if (Input.GetKeyDown("a")) { LeftSprites(); }
        if (Input.GetKeyDown("d")) { RightSprites(); }
        if (Input.GetKeyDown("w")) { UpSprites();  }
        if (Input.GetKeyDown("s")) { DownSprites(); }
    }
    
    void LeftSprites()
    {
        Torso.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].Torso; 
        Head.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].Head;
        LeftUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftUpperArm;
        LeftLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftLowerArm;
        LeftHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftHand;
        LeftUpperLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftUpperLeg;
        LeftLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftLowerLeg;
        LeftBoot.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftBoot;
        RightUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightUpperArm;
        RightLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightLowerArm;
        RightHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightHand;
        RightUpperLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightUpperLeg;
        RightLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightLowerLeg;
        RightBoot.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightBoot;

        RightLowerArm.transform.localPosition = new Vector2(0, 0);
        LeftLowerArm.transform.localPosition = new Vector2(0, 0);
        RightUpperArm.transform.localPosition = new Vector2(0, 0.0703f);
        LeftUpperArm.transform.localPosition = new Vector2(0, 0.0703f);
        LeftUpperLeg.transform.localPosition = new Vector2(0, 0);
        RightUpperLeg.transform.localPosition = new Vector2(0, 0);
        // i think it is semen
        Torso.GetComponent<SpriteRenderer>().flipX = true;
        Head.GetComponent<SpriteRenderer>().flipX = true;
        LeftUpperArm.GetComponent<SpriteRenderer>().flipX = true;
        LeftLowerArm.GetComponent<SpriteRenderer>().flipX = true;
        LeftHand.GetComponent<SpriteRenderer>().flipX = true;
        LeftUpperLeg.GetComponent<SpriteRenderer>().flipX = true;
        LeftLowerLeg.GetComponent<SpriteRenderer>().flipX = true;
        LeftBoot.GetComponent<SpriteRenderer>().flipX = true;
        RightUpperArm.GetComponent<SpriteRenderer>().flipX = true;
        RightLowerArm.GetComponent<SpriteRenderer>().flipX = true;
        RightHand.GetComponent<SpriteRenderer>().flipX = true;
        RightUpperLeg.GetComponent<SpriteRenderer>().flipX = true;
        RightLowerLeg.GetComponent<SpriteRenderer>().flipX = true;
        RightBoot.GetComponent<SpriteRenderer>().flipX = true;
    }

    void DownSprites()
    {
        Torso.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].Torso;
        Head.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].Head;
        LeftUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].LeftUpperArm;
        LeftLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].LeftLowerArm;
        LeftHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].LeftHand;
        LeftUpperLeg.GetComponent<SpriteRenderer>().sprite = null;
        LeftLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].LeftLowerLeg;
        LeftBoot.GetComponent<SpriteRenderer>().sprite = null;
        RightUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].RightUpperArm;
        RightLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].RightLowerArm;
        RightHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].RightHand;
        RightUpperLeg.GetComponent<SpriteRenderer>().sprite = null;
        RightLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[2].RightLowerLeg;
        RightBoot.GetComponent<SpriteRenderer>().sprite = null;

        RightLowerArm.transform.localPosition = new Vector2(0f, -0.036f);
        LeftLowerArm.transform.localPosition = new Vector2(0f, -0.036f);
        RightUpperArm.transform.localPosition = new Vector2(0.05f, 0.085f);
        LeftUpperArm.transform.localPosition = new Vector2(-0.05f, 0.085f);
        LeftUpperLeg.transform.localPosition = new Vector2(-0.017f, 0.043f);
        RightUpperLeg.transform.localPosition = new Vector2(0.017f, 0.043f);

        Torso.GetComponent<SpriteRenderer>().flipX = false;
        Head.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftHand.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftBoot.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        RightHand.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightBoot.GetComponent<SpriteRenderer>().flipX = false;
    }

    void RightSprites()
    {
        Torso.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].Torso;
        Head.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].Head;
        LeftUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftUpperArm;
        LeftLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftLowerArm;
        LeftHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftHand;
        LeftUpperLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftUpperLeg;
        LeftLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftLowerLeg;
        LeftBoot.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].LeftBoot;
        RightUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightUpperArm;
        RightLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightLowerArm;
        RightHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightHand;
        RightUpperLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightUpperLeg;
        RightLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightLowerLeg;
        RightBoot.GetComponent<SpriteRenderer>().sprite = PlayerSprites[0].RightBoot;

        RightLowerArm.transform.localPosition = new Vector2(0, 0);
        LeftLowerArm.transform.localPosition = new Vector2(0, 0);
        RightUpperArm.transform.localPosition = new Vector2(0, 0.0703f);
        LeftUpperArm.transform.localPosition = new Vector2(0, 0.0703f);
        LeftUpperLeg.transform.localPosition = new Vector2(0, 0);
        RightUpperLeg.transform.localPosition = new Vector2(0, 0);

        Torso.GetComponent<SpriteRenderer>().flipX = false;
        Head.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftHand.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftBoot.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        RightHand.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightBoot.GetComponent<SpriteRenderer>().flipX = false;
    }

    void UpSprites()
    {
        Torso.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].Torso;
        Head.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].Head;
        LeftUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].LeftUpperArm;
        LeftLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].LeftLowerArm;
        LeftHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].LeftHand;
        LeftUpperLeg.GetComponent<SpriteRenderer>().sprite = null;
        LeftLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].LeftLowerLeg;
        LeftBoot.GetComponent<SpriteRenderer>().sprite = null;
        RightUpperArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].RightUpperArm;
        RightLowerArm.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].RightLowerArm;
        RightHand.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].RightHand;
        RightUpperLeg.GetComponent<SpriteRenderer>().sprite = null;
        RightLowerLeg.GetComponent<SpriteRenderer>().sprite = PlayerSprites[1].RightLowerLeg;
        RightBoot.GetComponent<SpriteRenderer>().sprite = null;

        RightLowerArm.transform.localPosition = new Vector2(0f, -0.036f);
        LeftLowerArm.transform.localPosition = new Vector2(0f, -0.036f);
        RightUpperArm.transform.localPosition = new Vector2(0.05f, 0.085f);
        LeftUpperArm.transform.localPosition = new Vector2(-0.05f, 0.085f);
        LeftUpperLeg.transform.localPosition = new Vector2(-0.0138f, 0.0065f);
        RightUpperLeg.transform.localPosition = new Vector2(0.0162f, 0.047f);

        Torso.GetComponent<SpriteRenderer>().flipX = false;
        Head.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        LeftHand.GetComponent<SpriteRenderer>().flipX = false;
        LeftUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        LeftBoot.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperArm.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerArm.GetComponent<SpriteRenderer>().flipX = false;
        RightHand.GetComponent<SpriteRenderer>().flipX = false;
        RightUpperLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightLowerLeg.GetComponent<SpriteRenderer>().flipX = false;
        RightBoot.GetComponent<SpriteRenderer>().flipX = false;
    }

    [System.Serializable]
    public class Sprites
    {
        public Sprite Torso;
        public Sprite Head;
        public Sprite LeftUpperArm;
        public Sprite LeftLowerArm;
        public Sprite LeftHand;
        public Sprite LeftUpperLeg;
        public Sprite LeftLowerLeg;
        public Sprite LeftBoot;
        public Sprite RightUpperArm;
        public Sprite RightLowerArm;
        public Sprite RightHand;
        public Sprite RightUpperLeg;
        public Sprite RightLowerLeg;
        public Sprite RightBoot;


        //Sprites(Sprite _Torso,
        //        Sprite _Head,
        //        Sprite _LeftUpperArm,
        //        Sprite _LeftLowerArm,
        //        Sprite _LeftHand,
        //        Sprite _LeftUpperLeg,
        //        Sprite _LeftLowerLeg,
        //        Sprite _LeftBoot,
        //        Sprite _RightUpperArm,
        //        Sprite _RightLowerArm,
        //        Sprite _RightHand,
        //        Sprite _RightUpperLeg,
        //        Sprite _RightLowerLeg,
        //        Sprite _RightBoot)
    }
}

