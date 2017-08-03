using UnityEngine;

public class DeckController : MonoBehaviour
{
    private DeckScript _deckScript;

    public Rect hitboxRect;

    void Start()
    {
        _deckScript = GetComponent<DeckScript>();
    }

            //  Touch myTouch = Input.touches[0];
             // Ray myRay = Camera.main.ScreenPointToRay( myTouch.position );
             //RaycastHit hit;
             //Debug.DrawRay( myTouch.position, Vector3.right, Color.red, 10.0f, false );
             //if( Physics.Raycast( myRay, out hit, 10.0f, myMask )){
             //    if( hit.transform.tag == "Door" ){
             //        Debug.Log ( "Touched a door!" );
             //    }
             //}

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            _deckScript.open = !_deckScript.open;

            LayerMask mask = LayerMask.GetMask("UI");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Camera.main.ScreenPointToRay( Input.mousePosition);

            RaycastHit hit;
            Debug.DrawRay(ray.origin, Vector3.back * 10f, Color.red, 10f, false);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.tag == "Door")
                {
                    Debug.Log("hei");
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(startPosition, (Vector3)startPosition + (Vector3)GetTouchVector().normalized);

        //// Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        //Vector3 position = Camera.main.transform.position + (Vector3)HitBox.center;
        //Gizmos.DrawWireCube(position, new Vector3(HitBox.width, HitBox.height));
    }
}
