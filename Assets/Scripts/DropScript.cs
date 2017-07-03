using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{

    public List<Drops> DropsList;
    private DropCheck DropChecker;

    void Start()
    {
        DropChecker = GameObject.Find("Passive").GetComponent<DropCheck>();
    }
    
    private void OnDestroy()
    {
        Drop();
    }
        
    public void Drop()
    {
        bool night = DropChecker.NightDrops(); // is it night drops time

        for (int i = 0; i < DropsList.Count; i++) //Loop x times in objects drop list
        {

            if (DropsList[i].NightItem == night || DropsList[i].NightItem == false) // misses if item is nightitem and it is daytime
            {
                if (Random.Range(0, 100) <= DropsList[i].DropChance) //did rng jesus bless ye
                {
                    GameObject Copy = Instantiate(DropsList[i].Item) as GameObject; //Copy item from droplist with correct index
                    Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; // changes layer so it shows to player and not under the map
                    Copy.transform.position = transform.position; //drop it at destroyed objects position
                }
            }
        }
    }

    [System.Serializable]
    public class Drops
    {
        public GameObject Item;
        public int DropChance;
        public bool NightItem;

        public Drops(GameObject _Item, int _DropChance, bool _NightItem) { Item = _Item; DropChance = _DropChance; NightItem = _NightItem; }
    }
}
