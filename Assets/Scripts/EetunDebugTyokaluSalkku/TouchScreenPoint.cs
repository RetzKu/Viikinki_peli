using UnityEngine;

public class TouchScreenPoint : MonoBehaviour
{
    private TouchController _touchController;
    public int x;
    public int y;

    void Start()
    {
        _touchController = FindObjectOfType<TouchController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TouchPoint"))
        {
            if (_touchController != null)
            {
                _touchController.OnTouchDetected(x, y, transform.position);
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
}
