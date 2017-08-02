using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RuneBarUiController : MonoBehaviour
{
    private Image[] _runeHudImages;
    private RuneHolder _playerRuneHolder;
    private UiImageFader[] _faders;

	void Start ()
	{
        _runeHudImages = new Image[3];

	    var player = GameObject.FindWithTag("Player");
	    if (player == null)
	    {
	        Debug.LogError("RuneBarUiController cannot find player with Player Tag");
	    }

	    _playerRuneHolder = player.GetComponent<RuneHolder>();

	    _faders = GetComponentsInChildren<UiImageFader>();

	    for (int i = 0; i < _faders.Length; i++)
	    {
	        _runeHudImages[i] = _faders[i].Image;
	    }

        SetAllHudImages();
    }

    void SetAllHudImages()
    {
        Sprite[] runeImages = _playerRuneHolder.GetHudImages();
        for(int i = 0; i < runeImages.Length; i++)
        {
            if (runeImages[i] != null)
                _runeHudImages[i].sprite = runeImages[i];
        }
    }

    public void OnCd(int index, float duration)
    {
        _faders[index].FadeOut(duration);
        // StartCoroutine(LerpImageColor(index, duration));
    }

    IEnumerator LerpImageColor(int index, float duration)
    {
        Image image = _runeHudImages[index];
        image.color = Color.black;
        Color original = image.color;

        float increment = 0.02f / duration;
        float t = 0f;

        while (t < 1.0f)
        {
            image.color = Color.Lerp(original, Color.white, t);
            t += increment;
            yield return new WaitForSeconds(increment);
        }
        image.color = Color.white;
    }
    
    void Tint()
    {
        for (int i = 0; i < _runeHudImages.Length; i++)
        {
            _runeHudImages[i].color = Color.black;
        }
    }
}
