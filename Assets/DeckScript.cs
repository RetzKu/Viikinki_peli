using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Inventoryn UIn hallintaa, korttien kääntelyä yms
public class DeckScript : MonoBehaviour
{
    // Nopeus jolla kortit tulevat esiin / menevät piiloon
    public float Speed;
    // Lerp t, älä muuta
    static float t = 0.0f;

    // Korttien luku
    private int cardCount;
    // Arrayt default paikoille
    private float[] maxAngles;
    private float[] maxPositionX;
    private float[] maxPositionY;
    // Arrayt joihin voidaan tallentaa positiot ja rotatiot
    private Vector3[] positions;
    private Quaternion[] rotations;
    // Muuttuva uusi korttiluku
    private int updatedCardCount;

    // Bool onko invi auki
    public bool open = false;
    // Flageja invin hallintaan
    private bool openChanger = false;
    private bool openChanger2 = true;

    // Array jossa kulkee kortti peliobjektit
    private GameObject[] cards;

    // Testi metodi napille
    public void openButton()
    {
        if (open == false) open = true;
        else open = false;
    }

    // Määritetään juttuja jotta päästään eroon nullreference erroreista
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
        }
    }
	
	// Lerpataan paljon asioita
	void Update () {

        // Katsotaan inventoryn koko
        updatedCardCount = inventorySize();        

        // Jos inventory koko muuttuu, muutetaan kortteja
        if(cardCount != updatedCardCount)
        {
            // Päivitetään arrayt oikean kokoisiksi
            positions = new Vector3[updatedCardCount];
            rotations = new Quaternion[updatedCardCount];
            cards = new GameObject[updatedCardCount];

            
            // Lataa nolla positiot... paskaa, korjaa
            for (int x = 0; x < updatedCardCount - 1; x++)
                {
                //if (open == true)
                //{
                //    positions[x] = new Vector3(0f, 0f, 0f);
                //    rotations[x] = new Quaternion(0f, 0f, 0f, 1f);
                //}
                //else
                //{
                //    positions[x] = new Vector3(transform.FindChild("Card" + x).localPosition.x, transform.FindChild("Card" + x).localPosition.y, 0f);
                //    rotations[x] = new Quaternion(transform.FindChild("Card" + x).localRotation.x, transform.FindChild("Card" + x).localRotation.y, transform.FindChild("Card" + x).localRotation.z, transform.FindChild("Card" + x).localRotation.w);
                //}
                //if (open == true)
                //{
                //    positions[x] = new Vector3(cards[x].transform.localPosition.x, cards[x].transform.localPosition.y, 0f);
                //   rotations[x] = new Quaternion(cards[x].transform.localRotation.x, cards[x].transform.localRotation.y, cards[x].transform.localRotation.z, cards[x].transform.localRotation.w);
                //}
                //else
                //{
                    positions[x] = new Vector3(0f, 0f, 0f);
                    rotations[x] = new Quaternion(0f, 0f, 0f, 1f);
                //}
                
                }

            // Tuhoaa kaikki olemassa olevat kortit
            for (int x = 0; x < cardCount; x++)
            {
                Destroy(GameObject.Find("Card" + x).gameObject);
            }

            // Tekee oikean määrän kortteja (lisätty/vähennetty määrä)
            for (int x = 0; x < updatedCardCount; x++)
            {
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = gameObject.transform;
                cards[x].transform.localScale = new Vector3(0.7f, 1f, 1f);
                //cards[x].transform.position = gameObject.transform.position - new Vector3(0f, 100f, 0f);
                cards[x].transform.SetSiblingIndex(x);
                cards[x].AddComponent<Image>().sprite = GameObject.Find("CardPlaceholder").GetComponent<Image>().sprite;
                GameObject tempObj = new GameObject("CardChild");
                tempObj.transform.position = cards[x].transform.position;
                tempObj.AddComponent<Image>().sprite = GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<SpriteRenderer>().sprite;
                tempObj.transform.SetParent(cards[x].transform);
                tempObj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

            }

            // Ladataan uudet positiot ja anglet uudelle kortti määrälle
            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            t = 0;
        }

        // Kasvata lerpin t
        t += Time.deltaTime * Speed;

        // Aukaisee inventoryn
        if (open == true)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;
                
            }
            // Juoksee joka kerta
            for (int x = 0; x < cardCount; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[x], t), 0.5f);
            }

        }

        // Sulkee inventoryn
        if (open == false)
        {
            // Juoksee kerran kun open false
            if (openChanger2 == true)
            {
                t = 0;
                openChanger2 = false;
                openChanger = true;
            }
            // Juoksee joka kerta
            // 
            for (int x = 0; x < cardCount; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(maxPositionX[x], maxPositionY[x], 0f), new Vector3(0f, -50f, 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(maxAngles[x], 0f, t), 0.5f);
            }
        }
    }

    // Default arvoja rotationille
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

    // Default arvoja korttien x positiolle
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

    // Default arvoja korttien Y positiolle
    private float[] positionMaxY(int cards)
    {
        float[] max = new float[cards];

        switch (cards)
        {
            case 1:
                {
                    max[0] = 15f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = 0f;
                    max[1] = 0f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = 0f;
                    max[1] = 15f;
                    max[2] = 0f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = -10f;
                    max[1] = 10f;
                    max[2] = 10f;
                    max[3] = -10f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = -30f;
                    max[1] = 0f;
                    max[2] = 15f;
                    max[3] = 0f;
                    max[4] = -30f;
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

    // Metodi jolla voidaan tarkistella inventoryn kokoa
    public int inventorySize()
    {
        return GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData.Count;
    }
}
