using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckScript : MonoBehaviour
{
    public float Speed;
    public int ActiveCard = 1;
    static float t = 0.0f;

    private int cardCount;
    private float[] maxAngles;
    private float[] maxPositionX;
    private float[] maxPositionY;
    private Vector3[] positions;
    private Quaternion[] rotations;
    private int updatedCardCount;

    private GameObject[] cards;
    
    //Inventory.InventoryData[].GetComponent<SpriteRenderer>().sprite

    // Use this for initialization
    void Start () {
        cardCount = inventorySize();
        print(cardCount);
        updatedCardCount = cardCount;

        if (cardCount != 0)
        {
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            positions = new Vector3[cardCount];
            rotations = new Quaternion[cardCount];
            cards = new GameObject[cardCount];
        }

        else
        {
            positions = new Vector3[1];
            rotations = new Quaternion[1];
            cards = new GameObject[1];
        }

        if (cardCount != 0)
        {
            for (int x = 0; x < cardCount; x++)
            {
                positions[x] = new Vector3(0f, 0f, 0f);
                rotations[x] = transform.localRotation;
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = GameObject.Find("Deck1").transform;
            }
        }
        else
        {
            positions[0] = new Vector3(0f, 0f, 0f);
            rotations[0] = transform.localRotation;
            //cards[0] = new GameObject("Card0");
            //cards[0].transform.parent = GameObject.Find("Deck1").transform;
        }
       
        print(inventorySize());

    }
	
	// Update is called once per frame
	void Update () {


        print("Updated card count:" + updatedCardCount + ". InventorySize: " + inventorySize());
        updatedCardCount = inventorySize();
        

        if(cardCount != updatedCardCount)
        {
            positions = new Vector3[updatedCardCount];
            rotations = new Quaternion[updatedCardCount];
            cards = new GameObject[updatedCardCount];
            for (int x = 0; x < updatedCardCount - 1; x++)
                {
                    positions[x] = new Vector3(transform.FindChild("Card" + x).localPosition.x, transform.FindChild("Card" + x).localPosition.y, 0f);
                    rotations[x] = new Quaternion(transform.FindChild("Card" + x).localRotation.x, transform.FindChild("Card" + x).localRotation.y, transform.FindChild("Card" + x).localRotation.z, transform.FindChild("Card" + x).localRotation.w);
                    Destroy(transform.FindChild("Card" + x).gameObject);
                }

            for (int x = 0; x < updatedCardCount; x++)
            {
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = gameObject.transform;
                cards[x].transform.SetSiblingIndex(x);
                cards[x].AddComponent<Image>().sprite = GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<SpriteRenderer>().sprite;
            }

            //for (int x = 0; x < cardCount; x++)
            //{
            //    positions[x] = new Vector3(transform.GetChild(x).localPosition.x, transform.GetChild(x).localPosition.y, 0f);
            //    rotations[x] = new Quaternion(transform.GetChild(x).localRotation.x, transform.GetChild(x).localRotation.y, transform.GetChild(x).localRotation.z, transform.GetChild(x).localRotation.w);
            //}

            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            t = 0;
        }

            t += Time.deltaTime * Speed;

        for(int x = 0; x < cardCount; x++)
        {
            if(cards[x].transform.localPosition == null) { print("Null cards[x]"); }
            if(positions[x] == null) { print("Null positions[x]"); }
            print(maxPositionX);
            print(maxPositionY);
            transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
            transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[x], t), 0.5f);
        }

        //for (int x = 0; x < cardCount; x++)
        //{
        //    transform.GetChild(x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
        //    transform.GetChild(x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[x], t), 0.5f);
        //}
    }

    private float[] rotationMax (int cards)
    {
        float[] max = new float [cards];

        switch (cards)
        {
            case 1:
                {
                    max[0] = 0f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = 0.12f;
                    max[1] = -0.12f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = 0.12f;
                    max[1] = 0f;
                    max[2] = -0.12f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = 0.2f;
                    max[1] = 0.035f;
                    max[2] = -0.035f;
                    max[3] = -0.2f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = 0.24f;
                    max[1] = 0.14f;
                    max[2] = 0f;
                    max[3] = -0.14f;
                    max[4] = -0.24f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }

        Debug.LogWarning("0 korttia");
        Debug.LogError("Ongelma DeckScript.cs rotationMetodissa");
        max[0] = 0.0f;
        return new float[] { max[0] };

    }

    private float[] positionMaxX(int cards)
    {
        float[] max = new float[cards];

        switch (cards)
        {
            case 1:
                {
                    max[0] = 0f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = -35f;
                    max[1] = 35f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = -50f;
                    max[1] = 0f;
                    max[2] = 50f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = -75f;
                    max[1] = -25f;
                    max[2] = 25f;
                    max[3] = 75f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = -100f;
                    max[1] = -60f;
                    max[2] = 0f;
                    max[3] = 60f;
                    max[4] = 100f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }

        Debug.LogWarning("0 korttia");
        Debug.LogError("Ongelma DeckScript.cs positionX metodissa");
        max[0] = 0.0f;
        return new float[] { max[0] };

    }

    private float[] positionMaxY(int cards)
    {
        float[] max = new float[cards];

        switch (cards)
        {
            case 1:
                {
                    max[0] = 45f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = 30f;
                    max[1] = 30f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = 30f;
                    max[1] = 45f;
                    max[2] = 30f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = 20f;
                    max[1] = 40f;
                    max[2] = 40f;
                    max[3] = 20f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = 0f;
                    max[1] = 30f;
                    max[2] = 45f;
                    max[3] = 30f;
                    max[4] = 0f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }

        Debug.LogWarning("0 korttia");
        Debug.LogError("Ongelma DeckScript.cs positionY metodissa");
        max[0] = 0.0f;
        return new float[] { max[0] };

    }

    public int inventorySize()
    {
        return GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData.Count;
    }
}
