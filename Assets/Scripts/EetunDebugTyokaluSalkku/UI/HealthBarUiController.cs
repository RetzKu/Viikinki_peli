using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUiController : MonoBehaviour
{
    public Image HpBar;
    public Image HpOverlay;

    public float max = 100f;
    public float current = 50f;

	void Start ()
    {
	}

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            float endScale = EnemyMovement.map(current, 0f, 100f, 0f, 1f);
            StartCoroutine(LerpToScale(0.1f, endScale, HpOverlay));
        }
    }

    IEnumerator StartNextBar(float time, float endScale)
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(LerpToScale(0.4f, endScale, HpBar));
    }

    IEnumerator LerpToScale(float duration, float endScale, Image image)
    {
        float t = 0f;
        float increment = 0.002f / duration;

        while (t <= 1f)
        {
            // Overlay.transform.position = Vector2.Lerp(_startPosition, new Vector2(_startPosition.x ,_startPosition.y - 100f), t);
            float scale = Mathf.Lerp(1f, endScale, t);
            image.transform.localScale = new Vector3(scale, 1f, 1f);
            t += increment;
            yield return new WaitForSeconds(increment);
        }
    }
}
