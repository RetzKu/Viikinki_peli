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
    private float[] minAngles;
    private float[] minPositionX;
    private float[] minPositionY;
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
    private bool cardChanged = false;

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
        updatedCardCount = cardCount;

        if (cardCount != 0)
        {
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            minAngles = rotationMin(cardCount);
            minPositionX = positionMinX(cardCount);
            minPositionY = positionMinY(cardCount);
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

        // Jos inventoryn koko kasvaa
        if(cardCount < updatedCardCount)
        {
            // Tallennetaan "vanhojen" korttien propertiesit
            saveCardProperties();

            // Tuhoaa kaikki olemassa olevat kortit
            for (int x = 0; x < cardCount; x++)
            {
                Destroy(GameObject.Find("Card" + x).gameObject);
            }

            cards = new GameObject[updatedCardCount];

            // Tekee oikean määrän kortteja (lisätty/vähennetty määrä)
            for (int x = 0; x < updatedCardCount; x++)
            {
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = gameObject.transform;
                cards[x].transform.localScale = new Vector3(0.7f, 1f, 1f);
                cards[x].transform.SetSiblingIndex(x);
                cards[x].AddComponent<Image>().sprite = GameObject.Find("CardPlaceholder").GetComponent<Image>().sprite;
                GameObject tempObj = new GameObject("CardChild");
                tempObj.transform.position = cards[x].transform.position;
                Sprite cardImage = GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<SpriteRenderer>().sprite;
                Rect cardImageRect = cardImage.rect;

                var image = tempObj.AddComponent<Image>();
                image.sprite = cardImage;
                image.rectTransform.sizeDelta = cardImageRect.size;
                tempObj.transform.SetParent(cards[x].transform);
                tempObj.transform.localScale = new Vector3(2.85714285714286f, 2f, 1f);
            }

            // Ladataan uudet positiot ja anglet uudelle kortti määrälle
            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            minAngles = rotationMin(cardCount);
            minPositionX = positionMinX(cardCount);
            minPositionY = positionMinY(cardCount);
            t = 0;

            cardChanged = true;
        }

        /*
        // Jos inventoryn koko kasvaa
        if (cardCount > updatedCardCount)
        {
            // Tallennetaan "vanhojen" korttien propertiesit
            saveCardProperties();

            // Tuhoaa kaikki olemassa olevat kortit
            for (int x = 0; x < cardCount; x++)
            {
                Destroy(GameObject.Find("Card" + x).gameObject);
            }

            cards = new GameObject[updatedCardCount];

            // Tekee oikean määrän kortteja (lisätty/vähennetty määrä)
            for (int x = 0; x < updatedCardCount; x++)
            {
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = gameObject.transform;
                cards[x].transform.localScale = new Vector3(0.7f, 1f, 1f);
                cards[x].transform.SetSiblingIndex(x);
                cards[x].AddComponent<Image>().sprite = GameObject.Find("CardPlaceholder").GetComponent<Image>().sprite;
                GameObject tempObj = new GameObject("CardChild");
                tempObj.transform.position = cards[x].transform.position;
                Sprite cardImage = GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<SpriteRenderer>().sprite;
                Rect cardImageRect = cardImage.rect;

                var image = tempObj.AddComponent<Image>();
                image.sprite = cardImage;
                image.rectTransform.sizeDelta = cardImageRect.size;
                tempObj.transform.SetParent(cards[x].transform);
                tempObj.transform.localScale = new Vector3(2.85714285714286f, 2f, 1f);
            }

            // Ladataan uudet positiot ja anglet uudelle kortti määrälle
            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            minAngles = rotationMin(cardCount);
            minPositionX = positionMinX(cardCount);
            minPositionY = positionMinY(cardCount);
            t = 0;

            cardChanged = true;
        }

    */

        // Kasvata lerpin t
        t += Time.deltaTime * Speed;

        // Aukaisee inventoryn
        if (open == true && cardChanged == false)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;
                //transform.FindChild("Base").localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            // Juoksee joka kerta
            for (int x = 0; x < cardCount; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(minPositionX[x], minPositionY[x], 0f), new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(minAngles[x], maxAngles[x], t), 0.5f);
            }
        }

        // Aukaisee inventoryn ja kortti on vaihtunut
        if (open == true && cardChanged == true)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;

            }
            // Juoksee joka kerta
            for (int x = 0; x < cardCount - 1; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[x], t), 0.5f);
            }
            transform.FindChild("Card" + (cardCount - 1)).localPosition = Vector3.Lerp(new Vector3(minPositionX[(cardCount - 1)], minPositionY[(cardCount - 1)], 0f), new Vector3(maxPositionX[(cardCount - 1)], maxPositionY[(cardCount - 1)], 0f), t);
            transform.FindChild("Card" + (cardCount - 1)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(minAngles[(cardCount - 1)], maxAngles[(cardCount - 1)], t), 0.5f);
            if (t >= 1)
            {
                cardChanged = false;
            }
        }

        // Sulkee inventoryn
        if (open == false && cardChanged == false)
        {
            // Juoksee kerran kun open false
            if (openChanger2 == true)
            {
                t = 0;
                openChanger2 = false;
                openChanger = true;
                //transform.FindChild("Base").localRotation = new Quaternion(0f, 0f, -180f, 0f);
            }
            // Juoksee joka kerta
            // 
            for (int x = 0; x < cardCount; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(maxPositionX[x], maxPositionY[x], 0f), new Vector3(minPositionX[x], minPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(maxAngles[x], minAngles[x], t), 0.5f);
            }
        }

        // Kun invi kiinni ja kortti vaihtunut
        if (open == false && cardChanged == true)
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
            for (int x = 0; x < cardCount - 1; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(minPositionX[x], minPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, minAngles[x], t), 0.5f);
            }
            transform.FindChild("Card" + (cardCount - 1)).localPosition = Vector3.Lerp(new Vector3(maxPositionX[(cardCount - 1)], maxPositionY[(cardCount - 1)], 0f), new Vector3(minPositionX[(cardCount - 1)], minPositionY[(cardCount - 1)], 0f), t);
            transform.FindChild("Card" + (cardCount - 1)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(maxAngles[(cardCount - 1)], minAngles[(cardCount - 1)], t), 0.5f);
            if (t >= 1)
            {
                cardChanged = false;
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
                    max[0] = 0.09f;
                    max[1] = -0.09f;
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
                    max[1] = 0.06f;
                    max[2] = -0.06f;
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
        Debug.LogError("Error on rotationMax method");
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
                    max[0] = -45f;
                    max[1] = 45f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = -65f;
                    max[1] = 0f;
                    max[2] = 65f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = -97.5f;
                    max[1] = -32.5f;
                    max[2] = 32.5f;
                    max[3] = 97.5f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = -100f;
                    max[1] = -50f;
                    max[2] = 0f;
                    max[3] = 50f;
                    max[4] = 100f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }
        Debug.LogError("Error on positionMaxX method");
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
                    max[0] = 50f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = 40f;
                    max[1] = 40f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = 35f;
                    max[1] = 50f;
                    max[2] = 35f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = 15f;
                    max[1] = 50f;
                    max[2] = 50f;
                    max[3] = 15f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = 0f;
                    max[1] = 40f;
                    max[2] = 50f;
                    max[3] = 40f;
                    max[4] = 0f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }
        Debug.LogError("Error on positionMaxY method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Metodi jolla voidaan tarkistella inventoryn kokoa
    public int inventorySize()
    {
        return GameObject.Find("Player").GetComponent<PlayerScript>().Inventory.InventoryData.Count;
    }

    // Default arvoja rotationille
    private float[] rotationMin(int cards)
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
                    max[0] = 0.07f;
                    max[1] = -0.07f;
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
                    max[0] = 0.06f;
                    max[1] = 0.06f;
                    max[2] = -0.06f;
                    max[3] = -0.06f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = 0.08f;
                    max[1] = 0.06f;
                    max[2] = 0f;
                    max[3] = -0.06f;
                    max[4] = -0.08f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }
        Debug.LogError("Error on rotationMin method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien x positiolle
    private float[] positionMinX(int cards)
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
                    max[0] = -40f;
                    max[1] = 0f;
                    max[2] = 40f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = -50f;
                    max[1] = -20f;
                    max[2] = 20f;
                    max[3] = 50f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = -54f;
                    max[1] = -30f;
                    max[2] = 0f;
                    max[3] = 30f;
                    max[4] = 54f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }
        Debug.LogError("Error on positionMinX method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien Y positiolle
    private float[] positionMinY(int cards)
    {
        float[] max = new float[cards];

        switch (cards)
        {
            case 1:
                {
                    max[0] = -50f;
                    return new float[] { max[0] };
                }
            case 2:
                {
                    max[0] = -55f;
                    max[1] = -55f;
                    return new float[] { max[0], max[1] };
                }
            case 3:
                {
                    max[0] = -60f;
                    max[1] = -50f;
                    max[2] = -60f;
                    return new float[] { max[0], max[1], max[2] };
                }
            case 4:
                {
                    max[0] = -80f;
                    max[1] = -50f;
                    max[2] = -50f;
                    max[3] = -80f;
                    return new float[] { max[0], max[1], max[2], max[3] };
                }
            case 5:
                {
                    max[0] = -80f;
                    max[1] = -62f;
                    max[2] = -50f;
                    max[3] = -62f;
                    max[4] = -80f;
                    return new float[] { max[0], max[1], max[2], max[3], max[4] };
                }
            default:
                {
                    break;
                }
        }
        Debug.LogError("Error on positionMinY method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    public void saveCardProperties()
    {
        // Päivitetään arrayt oikean kokoisiksi
        positions = new Vector3[cardCount];
        rotations = new Quaternion[cardCount];

        // Tuodaan arrayhin vanhat positionit
        for (int x = 0; x < cardCount; x++)
        {
            positions[x] = new Vector3(transform.FindChild("Card" + x).localPosition.x, transform.FindChild("Card" + x).localPosition.y, 0f);
            rotations[x] = new Quaternion(transform.FindChild("Card" + x).localRotation.x, transform.FindChild("Card" + x).localRotation.y, transform.FindChild("Card" + x).localRotation.z, transform.FindChild("Card" + x).localRotation.w);
        }
    }
}
