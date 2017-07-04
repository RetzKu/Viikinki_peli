using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Touch,
    Mouse
}

public class CustomJoystick : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endposition;
    private bool touching = false;
    private bool touchingLastFrame = false;

    public Sprite JoystickSprite;
    public float maxLength;
   

    public ControllerType controlScheme = ControllerType.Mouse;

    private SpriteRenderer renderer;
    public Rect HitBox;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(startPosition, (Vector3)startPosition + (Vector3)GetTouchVector().normalized );
    }

    void Start()
    {
        startPosition = new Vector2(0f, 0f);
        endposition = new Vector2(0f, 0f);
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = JoystickSprite;

        HitBox.Set(transform.position.x + HitBox.x, transform.position.y + HitBox.y, HitBox.width, HitBox.height);
    }

    void Update()
    {
        touching = Input.GetMouseButton(0);

        Vector3 currentPotition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // sormi

        //if (HitBox.Contains(currentPotition))
        {
            if (touching && !touchingLastFrame)
            {
                startPosition = currentPotition;
                transform.position = startPosition;
            }
            if (touching)
            {
                endposition = currentPotition;
            }

            touchingLastFrame = touching;

            Vector2 touchVector = GetTouchVector();
            if (touchVector.magnitude > maxLength)
            {
                endposition = startPosition + (touchVector.normalized * maxLength);
            }

            transform.position = endposition;
           // HitBox.Set(transform.position.x + HitBox.x, transform.position.y + HitBox.y, HitBox.width, HitBox.height);
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
        renderer.color = touching ? Color.grey : Color.white;
        // normilizoi
    }

    Vector2 GetTouchVector()
    {
        return (endposition - startPosition);   
    }

    Vector2 GetInputVector()
    {
        Vector2 pos = endposition - startPosition;
        pos.x *= (Map(pos.magnitude, 0f, maxLength, 0f, 1f)) / 2f;
        pos.y *= (Map(pos.magnitude, 0f, maxLength, 0f, 1f)) / 2f;
        return pos;
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
