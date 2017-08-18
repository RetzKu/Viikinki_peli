using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public void StartFading(float totalTime, float fadeStartTime, SpriteRenderer sr)
    {

        StartCoroutine(FadeAway(totalTime, fadeStartTime, sr));
    }

    private IEnumerator FadeAway(float time, float fadeStartTime, SpriteRenderer trunk)
    {
        float fadeTime = time - fadeStartTime;
        yield return new WaitForSeconds(fadeStartTime);

        float timeIncrement = (fadeTime) / 40;
        float increment = (float)1 / 40;

        
        Color transparent = new Color(1f, 1f, 1f, 0f);
        float t = 0f;
        print("start ");

        for (int i = 0; i < 40; i++)
        {
            if (trunk != null)
            {
                trunk.color = Color.Lerp(Color.white, transparent, t);
            }
            t += increment;
            yield return new WaitForSeconds(timeIncrement);
        }

        print("end ");
    }
}
