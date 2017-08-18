using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiImageFader : MonoBehaviour
{
    public Image Overlay;
    public Image Image;
    private Vector2 _startPosition;


    public void Start()
    {
        var rect = transform.parent.GetComponent<RectTransform>().rect;
        Overlay.transform.localScale = new Vector3(1f, 0f, 1f);
    }

    public void FadeOut(float duration, Sprite onCdImage)
    {
        StartCoroutine(LerpOverlay(duration, onCdImage));
    }

    IEnumerator LerpOverlay(float duration, Sprite onCdImage)
    {
        float t = 0f;

        float increment = 0.02f / duration;

        Sprite notOnCdSprite = Image.sprite;
        Image.sprite = onCdImage;
        while (t <= 1f)
        {
            // Overlay.transform.position = Vector2.Lerp(_startPosition, new Vector2(_startPosition.x ,_startPosition.y - 100f), t);
            float scale = Mathf.Lerp(1f, 0f, t);
            Overlay.transform.localScale = new Vector3(1f, scale, 1f);
            t += increment;
            yield return new WaitForSeconds(increment);
        }
        Image.sprite = notOnCdSprite;
    }
}
