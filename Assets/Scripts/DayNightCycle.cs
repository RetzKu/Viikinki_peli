using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

    public Clock GameClock;
    public float Result;
    public float Multiplier;

	void Start ()
    {
        Multiplier = 1;
	}
    
    void Update()
    {
        GameClock.Sec += Time.deltaTime * Multiplier;
        CheckSec();
        CheckMin();
        CheckHour();
        ConvertTimeToPrecent();
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

    void TextMeshUpdate(int Precent)
    {
        gameObject.GetComponent<TextMesh>().text = string.Format("%{0}",Precent);
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
