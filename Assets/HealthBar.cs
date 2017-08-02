using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float Speed;
    private GameObject[] HealthArray = new GameObject[10];

	// Use this for initialization
	void Start ()
    {
        // Otetaan arrayhin hp gameobjectit
        for(int x = 0; x < 10; x++)
        {
            HealthArray[x] = transform.GetChild(x).gameObject;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void RefreshHP(int CurrentHP)
    {
        for (int x = 0; x < 10; x++)
        {
            HealthArray[x].SetActive(true);
        }

        for (int x = 9; x >= CurrentHP; x--)
        {
            HealthArray[x].SetActive(false);
            GameObject BloodDrop = new GameObject("Blood" + x);
            Sprite[] tempSprites =  Resources.LoadAll<Sprite>("hpbar");
            BloodDrop.AddComponent<Image>().sprite = tempSprites[14];
            BloodDrop.transform.SetParent(HealthArray[x].transform.parent.transform);
            BloodDrop.transform.position = new Vector3(HealthArray[x].transform.position.x, HealthArray[x].transform.position.y - 50f, 1f);
            BloodDrop.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            //BloodDrop.GetComponent<RectTransform>().local
            Destroy(BloodDrop, 2f);
        }
    }
}
