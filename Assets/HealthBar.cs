using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float Speed;
    static float t = 0.0f;
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
	void FixedUpdate()
    {
        t += Time.deltaTime * Speed;

        for(int x = 0; x < 10; x++)
        {
            if (transform.Find("Blood" + x) != null)
            {
                GameObject tempObj = transform.Find("Blood" + x).gameObject;
                tempObj.transform.position = new Vector3(tempObj.transform.position.x, Mathf.Lerp(tempObj.transform.position.y, tempObj.transform.position.y - 50f, t));
            }
        }
    }

    public void RefreshHP(int CurrentHP)
    {
        for (int x = 0; x < 10; x++)
        {
            HealthArray[x].SetActive(true);
        }

        if (CurrentHP >= 0)
        {
            for (int x = 9; x >= CurrentHP; x--)
            {
                HealthArray[x].SetActive(false);
                if (transform.Find("Blood" + x) == null)
                {
                    GameObject BloodDrop;
                    BloodDrop = new GameObject("Blood" + x);
                    Sprite[] tempSprites = Resources.LoadAll<Sprite>("hpbar");
                    BloodDrop.AddComponent<Image>().sprite = tempSprites[14];
                    BloodDrop.transform.SetParent(HealthArray[x].transform.parent.transform);
                    BloodDrop.transform.position = new Vector3(HealthArray[x].transform.position.x, HealthArray[x].transform.position.y -10f, 1f);
                    BloodDrop.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    //BloodDrop.GetComponent<RectTransform>().local
                    Destroy(BloodDrop, 2f);
                }
            }
        }
    }
}
