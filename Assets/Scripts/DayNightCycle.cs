using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour {

    public Clock GameClock;
    public float Result;
    public float Multiplier;

    public bool ResetCD;
    private GameObject Ui;
    private Text ClockText;

	void Start ()
    {
        Multiplier = 1;
        ResetCD = true;
        Ui = GameObject.Find("Ui");
        ClockText = Ui.transform.Find("Clock").GetComponent<Text>();
	}
    
    void Update()
    {
        GameClock.Sec += Time.deltaTime * Multiplier;
        CheckSec();
        CheckMin();
        CheckHour();
        ConvertTimeToPrecent();
        ResetDrops();
    }

    void CheckSec()
    {
        if (GameClock.Sec > 59)
        {
            GameClock.Sec -= 60;
            GameClock.Min++;
        }
    }

    void CheckMin()
    {
        if(GameClock.Min > 59)
        {
            GameClock.Min -= 60;
            GameClock.Hour++;
        }
    }
    void CheckHour()
    {
        if (GameClock.Hour == 24)
        {
            GameClock.Hour = 0;
        }
    }

    void ConvertTimeToPrecent()
    {
        if(GameClock.Hour >= 0 && GameClock.Hour < 9)
        {
            float ClockSeconds = GameClock.Hour * 60 * 60 + GameClock.Min * 60 + GameClock.Sec;
            float MaxSeconds = 9 * 60 * 60;
            Result = 100 - (ClockSeconds / MaxSeconds)*100;
            TextMeshUpdate((int)Result);  
        }
        else
        {
            if (GameClock.Hour > 15 && GameClock.Hour < 24)
            {
                float MaxSeconds = 8 * 60 * 60;
                float ClockSecods = (GameClock.Hour-16) * 60 * 60 + GameClock.Min * 60 + GameClock.Sec;
                Result = (ClockSecods / MaxSeconds * 100);
                TextMeshUpdate((int)Result);
            }
            else
            {
                Result = 0;
                TextMeshUpdate((int)Result);
            }
        }
        
    }
    void ResetDrops()
    {
        if (ResetCD == false)
        {
            if (Result > 33)
            {
                transform.GetComponent<DropCheck>().ResetStates();
                Debug.Log("Reseted");
                ResetCD = true;
            } 
        }
        if (ResetCD == true)
        {
            if (Result < 1)
            {
                ResetCD = false;
                print("Cd off");
            } 
        }
    }
    void TextMeshUpdate(int Precent)
    {
        int _Precent = Precent;
        ClockText.text = string.Format("{0}%",Precent);
    }

    [System.Serializable]
    public class Clock
    {
        public int Hour;
        public int Min;
        public float Sec;

        public Clock(int _Hour,int _Min, float _Sec) { Hour = _Hour; Min = _Min; Sec = _Sec; }
    }
}
