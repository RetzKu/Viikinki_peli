using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropCheck : MonoBehaviour
{
    private GameObject[] Items;
    private List<ItemStates> ItemStatesList;

    private float Clock;

    /*ITEM DROPWHITELIST*/
    private void Start()
    {
        System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo("Assets/Resources/Items/");
        Items = new GameObject[myDir.GetFiles().Length];
        Items = Resources.LoadAll<GameObject>("Items"); // Lataa kaikki items kansiossa olevat prefabit

        ItemStatesList = new List<ItemStates>();
        ResetStates();
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            Clock = transform.GetComponent<DayNightCycle>().Result;
            yield return new WaitForSeconds(5f);
        }
    }

    public bool NightDrops()
    {
        if (Clock > 75)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

    public void ResetStates()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            ItemStatesList.Add(new ItemStates(Items[i], false)); 
        }
    }
    class ItemStates
    {
        private bool State;
        private GameObject Item;

        public ItemStates(GameObject _Item, bool _State) { Item = _Item; State = _State; }
    }
}
