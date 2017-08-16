using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{
    public List<Drops> DropsList;
    private DropCheck DropChecker;

    void Start()
    {
        // DropChecker = GameObject.Find("Passive").GetComponent<DropCheck>();
    }
    
    // resurrsit osaavat dropata itse itsensä
    //private void OnDestroy()
    //{
    //    Drop();
    //}
        
    public void Drop()
    {
        // TODO: korjaan kun on päivä yö cycle
        // bool night = DropChecker.NightDrops(); // is it night drops time
        bool night = false;

        for (int i = 0; i < DropsList.Count; i++) //Loop x times in objects drop list
        {
            if (DropsList[i].NightItem == night || DropsList[i].NightItem == false) // misses if item is nightitem and it is daytime
            {
                if (Random.Range(0, 100) <= DropsList[i].DropChance) //did rng jesus bless ye
                {
                    GameObject Copy = Instantiate(DropsList[i].Item) as GameObject; //Copy item from droplist with correct _nextIndex
                    Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; // changes layer so it shows to player and not under the map
                    Copy.transform.position = transform.position; //drop it at destroyed objects position

                    // TODO: HUOMIO FREEFALL ON T}}LL}}
                    Copy.AddComponent<ObjectFaller>().StartFreeFall(1.5f);
                }
            }
        }
    }

    public static void Drop(List<Drops> dropList, Transform transform)
    {
        bool night = false;
        for (int i = 0; i < dropList.Count; i++) //Loop x times in objects drop list
        {
            if (dropList[i].NightItem == night || dropList[i].NightItem == false) // misses if item is nightitem and it is daytime
            {
                if (Random.Range(0, 100) <= dropList[i].DropChance) //did rng jesus bless ye
                {
                    GameObject Copy = Instantiate(dropList[i].Item) as GameObject; //Copy item from droplist with correct _nextIndex
                    Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; // changes layer so it shows to player and not under the map
                    Copy.transform.position = transform.position; //drop it at destroyed objects position

                    // TODO: HUOMIO FREEFALL ON T}}LL}}
                    Copy.AddComponent<ObjectFaller>().StartFreeFall(Random.Range(1.4f, 2.0f), new Vector2(-0.3f, 0.3f));
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
