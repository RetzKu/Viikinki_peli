using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteFinderScript : MonoBehaviour {

    public static SpriteFinderScript Instance;

    public List<Sprite> Sprites;

    [SerializeField]
    public List<HeadSetMaker> Heads;

    public List<TorsoSetMaker> Torsos;

    private List<GameObject> MeleeWeapons;
    private List<GameObject> Weapons;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Weapons = new List<GameObject>();
        MeleeWeapons = new List<GameObject>();
        foreach (Sprite t in Resources.LoadAll<Sprite>("VIKINGS")) { Sprites.Add(t); }
        foreach (GameObject t in Resources.LoadAll<GameObject>("Items/")) { Weapons.Add(t); }
        foreach (GameObject t in Weapons) { if (t.GetComponent<Melee>() != null) { MeleeWeapons.Add(t); } }
        Heads = new List<HeadSetMaker>(20);
        Torsos = new List<TorsoSetMaker>(6);
        HeadSets();
        TorsoSets();
    }



    public List<Sprite> RandomHead()
    {
        return Heads[Random.Range(0, 20)].HeadSprites();
    }

    public List<Sprite> RandomTorso()
    {
        return Torsos[Random.Range(0, 6)].TorsoSprites();
    }

    public GameObject RandomMeleeWeapon()
    {
        return MeleeWeapons[Random.Range(0, MeleeWeapons.Count)];
    }


    private void HeadSets()
    {
        for(int i = 1; i < 21; i++)
        {
            Sprite tmp_side = Sprites.Find(a => a.name == "s_c_head_enemy_"+i);
            Sprite tmp_up = Sprites.Find(a => a.name == "u_c_head_enemy_" + i);
            Sprite tmp_down = Sprites.Find(a => a.name == "d_c_head_enemy_" + i);


            Heads.Add(new HeadSetMaker(tmp_side, tmp_up, tmp_down));
        }
    }
    private void TorsoSets()
    {
        for (int i = 1; i < 7; i++)
        {
            Sprite tmp_side = Sprites.Find(a => a.name == "s_c_torso_enemy_" + i);
            Sprite tmp_up = Sprites.Find(a => a.name == "u_c_torso_enemy_" + i);
            Sprite tmp_down = Sprites.Find(a => a.name == "d_c_torso_enemy_" + i);

            Torsos.Add(new TorsoSetMaker(tmp_side, tmp_up, tmp_down));
        }
    }

    [System.Serializable]
    public class TorsoSetMaker
    {
        [SerializeField]
        private Sprite _s_c_torso;
        [SerializeField]
        private Sprite _u_c_torso;
        [SerializeField]
        private Sprite _d_c_torso;

        public List<Sprite> TorsoSprites()
        {
            List<Sprite> Tmp_List;
            Tmp_List = new List<Sprite>(3);

            Tmp_List.Add(_s_c_torso);
            Tmp_List.Add(_u_c_torso);
            Tmp_List.Add(_d_c_torso);

            return Tmp_List;
        }


        public TorsoSetMaker(Sprite s_c_torso, Sprite u_c_torso, Sprite d_c_torso) { _s_c_torso = s_c_torso; _u_c_torso = u_c_torso; _d_c_torso = d_c_torso; }
    }
    [System.Serializable]
    public class HeadSetMaker
    {
        [SerializeField]
        private Sprite _s_c_head;
        [SerializeField]
        private Sprite _u_c_head;
        [SerializeField]
        private Sprite _d_c_head;

        public List<Sprite> HeadSprites()
        {
            List<Sprite> Tmp_List;
            Tmp_List = new List<Sprite>(3);

            Tmp_List.Add(_s_c_head);
            Tmp_List.Add(_u_c_head);
            Tmp_List.Add(_d_c_head);
             
            return Tmp_List;
        }


        public HeadSetMaker(Sprite s_c_head, Sprite u_c_head, Sprite d_c_head) { _s_c_head = s_c_head; _u_c_head = u_c_head; _d_c_head = d_c_head; }
    }
}
