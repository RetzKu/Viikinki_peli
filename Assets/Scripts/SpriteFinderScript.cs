using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteFinderScript : MonoBehaviour {

    public static SpriteFinderScript Instance;

    public List<Sprite> Sprites;

    private List<UpperHandSetMaker> UpperHands;
    private List<LowerHandSetMaker> LowerHands;

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
        UpperHands = new List<UpperHandSetMaker>(4);
        LowerHands = new List<LowerHandSetMaker>(4);

        HeadSets();
        TorsoSets();
        UpperArms();
        LowerArms();
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
    public List<Sprite> RandomUpperArms()
    {
        return UpperHands[Random.Range(0, UpperHands.Count)].Sprites();
    }
    public List<Sprite> RandomLowerArms()
    {
        return LowerHands[Random.Range(0, LowerHands.Count)].Sprites();
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
    private void UpperArms()
    {
        for (int i = 1; i < 4; i++)
        {
            UpperHandSetMaker tmp_hands = new UpperHandSetMaker();
            Sprite tmp_s_left = Sprites.Find(a => a.name == "s_l_upper_arm_enemy_" + i);
            Sprite tmp_s_right = Sprites.Find(a => a.name == "s_r_upper_arm_enemy_" + i);

            Sprite tmp_d_left = Sprites.Find(a => a.name == "d_l_upper_arm_enemy_" + i);
            Sprite tmp_d_right = Sprites.Find(a => a.name == "d_r_upper_arm_enemy_" + i);

            Sprite tmp_u_left = Sprites.Find(a => a.name == "u_l_upper_arm_enemy_" + i);
            Sprite tmp_u_right = Sprites.Find(a => a.name == "u_r_upper_arm_enemy_" + i);
            tmp_hands.AddLeftArms(tmp_s_left, tmp_u_left, tmp_d_left);
            tmp_hands.AddRightArms(tmp_s_right, tmp_u_right, tmp_d_right);
            UpperHands.Add(tmp_hands);
        }
    }

    private void LowerArms()
    {
        for (int i = 1; i < 4; i++)
        {
            LowerHandSetMaker tmp_hands = new LowerHandSetMaker();
            Sprite tmp_s_left = Sprites.Find(a => a.name == "s_l_lower_arm_enemy_" + i);
            Sprite tmp_s_right = Sprites.Find(a => a.name == "s_r_lower_arm_enemy_" + i);

            Sprite tmp_d_left = Sprites.Find(a => a.name == "d_l_lower_arm_enemy_" + i);
            Sprite tmp_d_right = Sprites.Find(a => a.name == "d_r_lower_arm_enemy_" + i);

            Sprite tmp_u_left = Sprites.Find(a => a.name == "u_l_lower_arm_enemy_" + i);
            Sprite tmp_u_right = Sprites.Find(a => a.name == "u_r_lower_arm_enemy_" + i);
            tmp_hands.AddLeftArms(tmp_s_left, tmp_u_left, tmp_d_left);
            tmp_hands.AddRightArms(tmp_s_right, tmp_u_right, tmp_d_right);
            LowerHands.Add(tmp_hands);
        }
    }

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

    public class UpperHandSetMaker
    {
        private Sprite _s_right_arm;
        private Sprite _s_left_arm;

        private Sprite _u_right_arm;
        private Sprite _u_left_arm;

        private Sprite _d_right_arm;
        private Sprite _d_left_arm;

        public List<Sprite> Sprites()
        {
            List<Sprite> Tmp_sprite;
            Tmp_sprite = new List<Sprite>(6);

            Tmp_sprite.Add(_s_left_arm);
            Tmp_sprite.Add(_s_right_arm);
            Tmp_sprite.Add(_u_left_arm);
            Tmp_sprite.Add(_u_right_arm);
            Tmp_sprite.Add(_d_left_arm);
            Tmp_sprite.Add(_d_right_arm);

            return Tmp_sprite;
        }

        public void AddLeftArms(Sprite s_left_arm, Sprite u_left_arm, Sprite d_left_arm)
        {
            _s_left_arm = s_left_arm;
            _u_left_arm = u_left_arm;
            _d_left_arm = d_left_arm;
        }

        public void AddRightArms(Sprite s_right_arm, Sprite u_right_arm, Sprite d_right_arm)
        {
            _s_right_arm = s_right_arm;
            _u_right_arm = u_right_arm;
            _d_right_arm = d_right_arm;
        }
    }

    public class LowerHandSetMaker
    {
        private Sprite _s_right_arm;
        private Sprite _s_left_arm;

        private Sprite _u_right_arm;
        private Sprite _u_left_arm;

        private Sprite _d_right_arm;
        private Sprite _d_left_arm;

        public List<Sprite> Sprites()
        {
            List<Sprite> Tmp_sprite;
            Tmp_sprite = new List<Sprite>(6);

            Tmp_sprite.Add(_s_left_arm);
            Tmp_sprite.Add(_s_right_arm);
            Tmp_sprite.Add(_u_left_arm);
            Tmp_sprite.Add(_u_right_arm);
            Tmp_sprite.Add(_d_left_arm);
            Tmp_sprite.Add(_d_right_arm);

            return Tmp_sprite;
        }

        public void AddLeftArms(Sprite s_left_arm, Sprite u_left_arm, Sprite d_left_arm)
        {
            _s_left_arm = s_left_arm;
            _u_left_arm = u_left_arm;
            _d_left_arm = d_left_arm;
        }

        public void AddRightArms(Sprite s_right_arm, Sprite u_right_arm, Sprite d_right_arm)
        {
            _s_right_arm = s_right_arm;
            _u_right_arm = u_right_arm;
            _d_right_arm = d_right_arm;
        }
    }
}
