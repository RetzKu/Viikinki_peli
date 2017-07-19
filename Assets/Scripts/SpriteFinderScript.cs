using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteFinderScript : MonoBehaviour {

    public List<Sprite> Sprites;

    [SerializeField]
    public List<HeadSetMaker> Heads;

    public List<Sprite> Torsos;

    public List<SpriteSet> SpriteSets;

	// Use this for initialization
	void Start ()
    {
        foreach (Sprite t in Resources.LoadAll<Sprite>("VIKINGS")){ Sprites.Add(t); }
        Heads = new List<HeadSetMaker>(20);
        HeadSets();
        Debug.Log(Heads);
	}

    private void HeadSets()
    {
        for(int i = 1; i < 20; i++)
        {
            Sprite tmp_side = Sprites.Find(a => a.name == "s_c_head_enemy_"+i);
            Sprite tmp_up = Sprites.Find(a => a.name == "u_c_head_enemy_" + i);
            Sprite tmp_down = Sprites.Find(a => a.name == "d_c_head_enemy_" + i);


            Heads.Add(new HeadSetMaker(tmp_side, tmp_up, tmp_down));
        }
    }

    public HeadSetMaker RandomHead()
    {
        return Heads[Random.Range(1, 20)];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public class SpriteSet
    {
        private Sprite _Head;
        private Sprite _Torso;

        public SpriteSet(Sprite Head, Sprite Torso) { _Head = Head; _Torso = Torso; }
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
