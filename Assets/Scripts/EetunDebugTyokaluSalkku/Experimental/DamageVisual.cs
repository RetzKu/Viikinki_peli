using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

//using UnityEngine.PostProcessing;

public class DamageVisual : MonoBehaviour
{
    public static DamageVisual inctance;

    private PostProcessingBehaviour postProcessingBehaviour;
    public float DefaultVigietteIntensity = 0.45f;
    private Coroutine fader;
    // Use this for initialization
    void Start()
    {
        postProcessingBehaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        postProcessingBehaviour.profile.vignette.enabled = false;
        inctance = this;
    }

    public static void TakeDamage()
    {
        inctance.TakeDamage1();
    }

    private  void TakeDamage1()
    {
        // Debug.Log("Post-processor effect launcher");

        postProcessingBehaviour.profile.vignette.enabled = true;
        VignetteModel.Settings s = postProcessingBehaviour.profile.vignette.settings; //  = DefaultVigietteIntensity;
        s.intensity = DefaultVigietteIntensity;

        postProcessingBehaviour.profile.vignette.settings = s;

        if (fader != null)
            StopCoroutine(fader);
        fader = StartCoroutine(FadeOut(s));
    }

    IEnumerator FadeOut(VignetteModel.Settings s)
    {
        float a = DefaultVigietteIntensity / 90;
        float amount = 1f / 90;
        for (int i = 0; i < 90; i++)
        {
            s.intensity -= a;
            postProcessingBehaviour.profile.vignette.settings = s;
            yield return new WaitForSeconds(amount);
        }
        postProcessingBehaviour.profile.vignette.enabled = false;
    }
}
