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


    public Vector2 offsets = new Vector2(0, 0);

    public Sprite JoystickSprite;
    public float maxLength;
    public ControllerType controlScheme = ControllerType.Mouse;
    public Rect HitBox;
    public Transform Player;
    private Vector3 lastPos;


    private bool FirstTouchSuccess = false;

    SpriteRenderer renderer;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(startPosition, (Vector3)startPosition + (Vector3)GetTouchVector().normalized);

        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        HitBox.x = bottomLeft.x + offsets.x;
        HitBox.y = bottomLeft.y + offsets.y;

        Vector3 position = Camera.main.transform.position + (Vector3)HitBox.center;
        Gizmos.DrawWireCube(position, new Vector3(HitBox.width, HitBox.height));
    }

    void Start()
    {
        startPosition = new Vector2(0f, 0f);
        endposition = new Vector2(0f, 0f);
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = JoystickSprite;

        //HitBox.Set(transform.position.x + HitBox.x, transform.position.y + HitBox.y, HitBox.width, HitBox.height);
    }

    void Update()
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        HitBox.x = bottomLeft.x + offsets.x;
        HitBox.y = bottomLeft.y + offsets.y;


        lastPos = transform.position;

        touching = Input.GetMouseButton(0);

        //  Vector3 worldPosition = Input.mousePosition;
        Vector3 currentPotition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // sormi

        Vector3 cameraPos = Camera.main.transform.position;
        Rect ScreenView = new Rect(cameraPos.x + HitBox.x, cameraPos.y + HitBox.y, HitBox.width, HitBox.height);

        if (Input.GetMouseButtonDown(0))    // eka 
        {
            if (touching && !touchingLastFrame && ScreenView.Contains(currentPotition))
            {
                startPosition = currentPotition;
                transform.position = startPosition;
                touching = true;
                FirstTouchSuccess = true;
            }
            else
            {
                FirstTouchSuccess = false;
            }
        }
        else
        {
            if (touching && FirstTouchSuccess)    // toka
            {
                if (touching && touchingLastFrame)
                {
                    endposition = currentPotition;
                    transform.position = endposition;
                }

                Vector2 touchVector = GetTouchVector();
                if (touchVector.magnitude > maxLength && touchingLastFrame)
                {
                    endposition = startPosition + (touchVector.normalized * maxLength);
                    transform.position = endposition;
                }
            }
            else
            {
                touching = false;
            }
        }

        touchingLastFrame = touching;
        UpdateVisuals();
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
