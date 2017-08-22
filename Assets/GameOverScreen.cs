using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
                GameObject.Find("Player").transform.Find("Main Camera").GetComponent<CameraSpinner>().StartSpinning();
                StartTime = Time.time;
                StartCoroutine(FadeIn()); 
            }
        }
	}

    IEnumerator FadeIn()
    {
        while(GameObject.Find("Player").GetComponent<combat>().hp <= 0)
        {
            float t = (Time.time - StartTime) / FadeDuration;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 0, 0, Mathf.SmoothStep(0, 1, t));
            transform.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.SmoothStep(0, 1, t));

            if(t == 1)
            {
                GameObject.Find("Player").transform.Find("Main Camera").GetComponent<CameraSpinner>().StopSpinning();
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                 // Application.LoadLevel(Application.loadedLevel);
                 SceneManager.LoadScene("HikkyBoxV2");
            }

            yield return new WaitForFixedUpdate();
        }

    }
}
