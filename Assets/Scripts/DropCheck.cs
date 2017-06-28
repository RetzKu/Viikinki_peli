using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropCheck : MonoBehaviour
{
    private GameObject[] Items;

    public List<ItemStates> ItemStatesList;
    private List<GameObject> asd;
    
    private float Clock;

    /*ITEM DROPWHITELIST*/
    private void Start()
    {
        ItemStatesList = new List<ItemStates>();

        foreach(GameObject t in Resources.LoadAll<GameObject>("Items/NightItems"))
        {
            ItemStatesList.Add(new ItemStates(t,false));
        }

        for(int i = 0; i < ItemStatesList.Count; i++)
        {
            Debug.Log(ItemStatesList[i].Item.name);
        }
        ResetStates();
        StartCoroutine(UpdateClock()); //starts courutine
    }

    IEnumerator UpdateClock() //COURUTINE THAT RUN EVERY 5SEC. SAFE FOR LATER USE
    {
        while (true)
        {
            Clock = transform.GetComponent<DayNightCycle>().Result;
            yield return new WaitForSeconds(5f);
        }
    }

    public bool ItemCD(GameObject Item)
    {

        return true;
    }

    public bool NightDrops()
    {
        if (Clock > 75){return true;}
        else{return false;}
    }

    public void ResetStates()
    {
       
    }

    [System.Serializable]
    public class ItemStates
    {
        public bool State;
        public GameObject Item;

        public ItemStates(GameObject _Item, bool _State) { Item = _Item; State = _State; }
    }
}
