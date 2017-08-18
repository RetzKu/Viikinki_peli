using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour {

    float StartTime;
    public float FadeDuration;

	void Update ()
    {

        if(Input.GetKeyDown(KeyCode.T) == true)
        {
            print("2");
        }


        if (GameObject.Find("Player").GetComponent<combat>().hp <= 0 )
        {
            if (StartTime == 0)
            {

                StartTime = Time.time;
                StartCoroutine(FadeIn()); 
            }
        }
	}
    IEnumerator FadeIn()
    {
        while(true)
        {
            float t = (Time.time - StartTime) / FadeDuration;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, Mathf.SmoothStep(0, 1, t));
            transform.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.SmoothStep(0, 1, t));
            yield return new WaitForFixedUpdate();
        }

    }
}
