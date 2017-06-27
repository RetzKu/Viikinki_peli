using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{

    public List<Drops> DropsList;
    private GameObject Object;
    private Transform Passive;


    void Start()
    {
        Object = transform.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Drop()
    {
        for (int i = 0; i < DropsList.Count; i++)
        {
            if (Random.Range(0, 100) <= DropsList[i].DropChance)
            {
                GameObject Copy = Instantiate(DropsList[i].Item) as GameObject;
                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                Copy.transform.position = transform.position;
            }
        }
    }

    [System.Serializable]
    public class Drops
    {
        public GameObject Item;
        public int DropChance;

        public Drops(GameObject _Item, int _DropChance) { Item = _Item; DropChance = _DropChance; }
    }
}
