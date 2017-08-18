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
    private float[] maxAngles2;
    private float[] maxPositionX2;
    private float[] maxPositionY2;
    private float[] minAngles;
    private float[] minPositionX;
    private float[] minPositionY;
    private float[] minAngles2;
    private float[] minPositionX2;
    private float[] minPositionY2;
    // Arrayt joihin voidaan tallentaa positiot ja rotatiot
    private Vector3[] positions;
    private Quaternion[] rotations;
    private bool[] _weaponEquips;
    private bool[] _armorEquips;
    private bool[] armorInv;
    private bool[] activeArmor;
    // Muuttuva uusi korttiluku
    private int updatedCardCount;

    // Bool onko invi auki
    [HideInInspector]
    public bool open = false;
    // Flageja invin hallintaan
    private bool openChanger = false;
    private bool openChanger2 = true;
    private bool addCard = false;
    private bool removeCard = false;
    [HideInInspector]
    public bool weaponChanged = false;

    private bool dragFlag = false;

    private int brokenWeaponInt;
    private Sprite[] cardArray;

    // Array jossa kulkee kortti peliobjektit
    private GameObject[] cards;

    // Testi metodi napille
    public void openButton()
    {
        if (open == false) open = true;
        else open = false;
    }

    // Määritetään juttuja jotta päästään eroon nullreference erroreista
    void Start()
    {
        cardArray = Resources.LoadAll<Sprite>("iteminventory");
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
            //cards = new GameObject[1];
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
    void Update()
    {

        // Katsotaan inventoryn koko
        updatedCardCount = inventorySize();


        // Tää nousee kahella kerralla ja se pugaa
        // Jos inventoryn koko kasvaa
        if (cardCount < updatedCardCount)
        {

            // Tallennetaan "vanhojen" korttien propertiesit
            saveCardProperties();

            // Tuhoaa kaikki olemassa olevat kortit
            for (int x = 0; x < cardCount; x++)
            {
                Destroy(GameObject.Find("Card" + x).gameObject);
            }

            cards = new GameObject[updatedCardCount];

            bool equipsBoolean = false;

            for (int z = 0; z < _weaponEquips.Length; z++)
            {
                if (_weaponEquips[z] == true)
                {
                    equipsBoolean = true;
                }
            }


            armorInv = new bool[updatedCardCount];
            activeArmor = new bool[updatedCardCount];

            if(_armorEquips != null)
            {
                for(int c = 0; c < _armorEquips.Length; c++)
                {
                    activeArmor[c] = _armorEquips[c];
                }
            }

            // Tekee oikean määrän kortteja (lisätty määrä)
            for (int x = 0; x < updatedCardCount; x++)
            {
                // Tehdään parent objekti kortille
                cards[x] = new GameObject("Card" + x);
                cards[x].transform.parent = gameObject.transform;
                cards[x].transform.localScale = new Vector3(0.7f, 1f, 1f);
                cards[x].transform.SetSiblingIndex(x);
                cards[x].AddComponent<Image>().sprite = cardArray[3];
                cards[x].AddComponent<CardMoverEraser>();


                // Covercolor esittää esineen durationia/kulumista
                GameObject tempObj2 = new GameObject("coverColor");
                tempObj2.transform.position = cards[x].transform.position;
                tempObj2.AddComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
                tempObj2.AddComponent<Image>().sprite = Resources.Load<Sprite>("whitecard");
                tempObj2.transform.SetParent(cards[x].transform);
                tempObj2.transform.localScale = new Vector3(1f, 1f, 1f);
                tempObj2.GetComponent<Image>().color = new Color(0.13334f, 0.0902f, 0.02745f, 0.6902f);
                tempObj2.GetComponent<RectTransform>().localPosition = new Vector3(50f, 0f, 0f);

                // Equippedcolor indikoi mi(t)kä esineet pelaajalla on päällä
                GameObject tempObj3 = new GameObject("equippedColor");
                tempObj3.transform.position = cards[x].transform.position;
                tempObj3.AddComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
                tempObj3.AddComponent<Image>().sprite = Resources.Load<Sprite>("whitecard");
                tempObj3.transform.SetParent(cards[x].transform);
                tempObj3.transform.localScale = new Vector3(1f, 1f, 1f);
                tempObj3.GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0f);
                tempObj3.GetComponent<RectTransform>().localPosition = new Vector3(50f, 0f, 0f);
                // Tarkistetaan taulukosta oliko kyseinen esine päällä ennen kortin tuhoamista, jos oli laitetaan se uudestaan päälle
                if (_weaponEquips.Length > x)
                {
                    if (_weaponEquips[x] == true)
                    {
                        tempObj3.GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0.2667f);
                    }
                }

                // Tarkistetaan taulukosta oliko kyseinen armori päällä ennen kortin tuhoamista, jos oli laitetaan se uudestaan päälle
                if (_armorEquips.Length > x)
                {
                    if (_armorEquips[x] == true)
                    {
                        tempObj3.GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0.2667f);
                    }
                }

                // Kortissa olevan aseen kuva
                GameObject tempObj = new GameObject("CardChild");
                tempObj.transform.position = cards[x].transform.position;
                Sprite cardImage = PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<SpriteRenderer>().sprite;

                if (PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<armorScript>() != null)
                {
                    // Kyseinen esine on armori
                    armorInv[x] = true;



                    // Laitetaan x armori päälle ja kaikki taaemmat laitetaan inactiveksi
                    for (int z = 0; z < x; z++)
                    {
                        activeArmor[z] = false;
                        if (armorInv[z] == true)
                        {
                            //PlayerScript.Player.GetComponent<PlayerScript>().Inventory.EquipItem(z);
                            transform.GetChild(z).GetChild(1).GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0f);
                        }
                    }
                    activeArmor[x] = true;
                    PlayerScript.Player.GetComponent<PlayerScript>().UnEquipArmor();
                    PlayerScript.Player.GetComponent<PlayerScript>().Inventory.EquipItem(x);
                    tempObj3.GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0.2667f);

                }

                else
                {
                    // Kyseinen esine on ase
                    armorInv[x] = false;
                }

                Rect cardImageRect = cardImage.rect;
                var image = tempObj.AddComponent<Image>();
                image.sprite = cardImage;
                image.rectTransform.sizeDelta = cardImageRect.size;
                tempObj.transform.SetParent(cards[x].transform);
                tempObj.transform.localScale = new Vector3(2.85714285714286f, 2f, 1f);

                if (weaponChanged == true && updatedCardCount - 1 == x && equipsBoolean == false)
                {
                    if (PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<armorScript>() == null)
                    {
                        transform.GetChild(x).GetComponent<CardMoverEraser>().AutoEquipFirstItem();
                    }
                    else
                    {
                        transform.GetChild(x).GetComponent<CardMoverEraser>().AutoEquipFirstItem2();
                    }
                    _weaponEquips[x] = true;
                    weaponChanged = false;
                }
            }

            equipsBoolean = false;

            // Ladataan uudet positiot ja anglet uudelle kortti määrälle
            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            minAngles = rotationMin(cardCount);
            minPositionX = positionMinX(cardCount);
            minPositionY = positionMinY(cardCount);
            t = 0;

            addCard = true;
        }

        // Jos inventoryn koko laskee
        if (cardCount > updatedCardCount)
        {
            // Tallennetaan "vanhojen" korttien propertiesit
            saveCardProperties();

            // Testiä --> GetComponentInChildren<CardMoverEraser>().ApplyOpenPosition();
            // Ladataan uudet positiot ja anglet uudelle kortti määrälle
            cardCount = updatedCardCount;
            maxAngles = rotationMax(cardCount);
            maxPositionX = positionMaxX(cardCount);
            maxPositionY = positionMaxY(cardCount);
            minAngles = rotationMin(cardCount);
            minPositionX = positionMinX(cardCount);
            minPositionY = positionMinY(cardCount);
            maxAngles2 = rotationMax(cardCount + 1);
            maxPositionX2 = positionMaxX(cardCount + 1);
            maxPositionY2 = positionMaxY(cardCount + 1);
            minAngles2 = rotationMin(cardCount + 1);
            minPositionX2 = positionMinX(cardCount + 1);
            minPositionY2 = positionMinY(cardCount + 1);
            t = 0;
            removeCard = true;
        }

        // Kasvata lerpin t
        t += Time.deltaTime * Speed;

        // Aukaisee inventoryn
        if (open == true && addCard == false && removeCard == false && dragFlag == false)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;
                // GetComponentInChildren<CardMoverEraser>().ApplyOpenPosition();
                //transform.FindChild("Base").localRotation = new Quaternion(0f, 0f, 0f, 0f);
            }
            // Juoksee joka kerta
            for (int x = 0; x < cardCount; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(minPositionX[x], minPositionY[x], 0f), new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(minAngles[x], maxAngles[x], t), 0.5f);
            }
        }


        // Tälle keino käsitellä monen kortin lisäys yhtä aikaa
        // Aukaisee inventoryn ja kortti lisääntynyt
        if (open == true && addCard == true && removeCard == false)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;
                //GetComponentInChildren<CardMoverEraser>().ApplyOpenPosition();
            }
            // Juoksee joka kerta
            if (positions.Length == cardCount - 1 || positions.Length == cardCount)
            {
                for (int x = 0; x < cardCount - 1; x++)
                {
                    transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                    transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[x], t), 0.5f);
                }
            }
            else
            {
                for (int x = 0; x < cardCount - 1; x++)
                {
                    transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(maxPositionX[x], maxPositionY[x], 0f), t);
                    transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(0f, maxAngles[x], t), 0.5f);
                }
            }
            transform.FindChild("Card" + (cardCount - 1)).localPosition = Vector3.Lerp(new Vector3(minPositionX[(cardCount - 1)], minPositionY[(cardCount - 1)], 0f), new Vector3(maxPositionX[(cardCount - 1)], maxPositionY[(cardCount - 1)], 0f), t);
            transform.FindChild("Card" + (cardCount - 1)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(minAngles[(cardCount - 1)], maxAngles[(cardCount - 1)], t), 0.5f);
            if (t >= 1)
            {
                addCard = false;
            }
        }

        // Invi auki ja kortti lähtee
        if (open == true && addCard == false && removeCard == true)
        {
            // Juoksee kerran kun open true
            if (openChanger == true)
            {
                t = 0;
                openChanger = false;
                openChanger2 = true;

            }
            // Juoksee joka kerta

            // Counter pitää X arvon oikeana huomioiden poistuvan aseen arvot
            int counter = 0;

            for (int x = 0; x < updatedCardCount + 1; x++)
            {
                if (x != brokenWeaponInt)
                {
                    if (positions[x] != null)
                    {
                        transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(maxPositionX[counter], maxPositionY[counter], 0f), t);
                        transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, maxAngles[counter], t), 0.5f);
                    }
                }
                else
                {
                    counter--;
                }
                counter++;
            }

            transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().sprite = cardArray[1];
            transform.FindChild("Card" + (brokenWeaponInt)).localPosition = Vector3.Lerp(positions[brokenWeaponInt], new Vector3(maxPositionX2[(brokenWeaponInt)], maxPositionY2[(brokenWeaponInt)] + 50f, 0f), t);
            transform.FindChild("Card" + (brokenWeaponInt)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[brokenWeaponInt].z, maxAngles2[(brokenWeaponInt)], t), 0.5f);
            Color cardColor = transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().color = new Color(cardColor.r, cardColor.g, cardColor.b, Mathf.Lerp(1f, 0f, t));
            Color cardColor2 = transform.FindChild("Card" + (brokenWeaponInt)).GetChild(0).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetChild(0).GetComponent<Image>().color = new Color(cardColor2.r, cardColor2.g, cardColor2.b, Mathf.Lerp(1f, 0f, t));
            Color cardColor3 = transform.FindChild("Card" + (brokenWeaponInt)).GetChild(1).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetChild(1).GetComponent<Image>().color = new Color(cardColor3.r, cardColor3.g, cardColor3.b, Mathf.Lerp(1f, 0f, t));

            if (t >= 1)
            {
                removeCard = false;
                transform.FindChild("Card" + (brokenWeaponInt)).GetChild(1).GetComponent<Image>().color = new Color(0.1961f, 0.7176f, 0.5411f, 0f);
                _weaponEquips[brokenWeaponInt] = false;
                DestroyImmediate(transform.FindChild("Card" + (brokenWeaponInt)).gameObject);
                //DestroyImmediate(cards[brokenWeaponInt]);

                int tempInt = transform.childCount - 1;
                for (int x = 0; x < tempInt; x++)
                {
                    transform.GetChild(x).name = "Card" + x;

                    float duration;
                    try
                    {
                        duration = PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<weaponStats>().duration;
                    }
                    catch
                    {
                        duration = PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<armorScript>().duration;
                    }

                    transform.GetChild(x).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1f - 0.1f * duration, 1f, 1f);

                }
            }
        }

        // Sulkee inventoryn
        if (open == false && addCard == false && removeCard == false)
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

        // Kun invi kiinni ja kortti lisääntynyt
        if (open == false && addCard == true && removeCard == false)
        {
            // Juoksee kerran kun open false
            if (openChanger2 == true)
            {
                t = 0;
                openChanger2 = false;
                openChanger = true;
            }
            // Juoksee joka kerta

            if (positions.Length == cardCount - 1 || positions.Length == cardCount)
            {
                for (int x = 0; x < cardCount - 1; x++)
                {
                    transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(minPositionX[x], minPositionY[x], 0f), t);
                    transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, minAngles[x], t), 0.5f);
                }
            }
            else
            {
                for (int x = 0; x < cardCount - 1; x++)
                {
                    transform.FindChild("Card" + x).localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(minPositionX[x], minPositionY[x], 0f), t);
                    transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(0f, minAngles[x], t), 0.5f);
                }
            }
            transform.FindChild("Card" + (cardCount - 1)).localPosition = Vector3.Lerp(new Vector3(maxPositionX[(cardCount - 1)], maxPositionY[(cardCount - 1)], 0f), new Vector3(minPositionX[(cardCount - 1)], minPositionY[(cardCount - 1)], 0f), t);
            transform.FindChild("Card" + (cardCount - 1)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(maxAngles[(cardCount - 1)], minAngles[(cardCount - 1)], t), 0.5f);

            // 

            /*
            for (int x = 0; x < cardCount - 1; x++)
            {
                transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(minPositionX[x], minPositionY[x], 0f), t);
                transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, minAngles[x], t), 0.5f);
            }
            transform.FindChild("Card" + (cardCount - 1)).localPosition = Vector3.Lerp(new Vector3(maxPositionX[(cardCount - 1)], maxPositionY[(cardCount - 1)], 0f), new Vector3(minPositionX[(cardCount - 1)], minPositionY[(cardCount - 1)], 0f), t);
            transform.FindChild("Card" + (cardCount - 1)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(maxAngles[(cardCount - 1)], minAngles[(cardCount - 1)], t), 0.5f);
            */
            if (t >= 1)
            {
                addCard = false;
            }
        }

        // Kun invi kiinni ja kortti vähentynyt
        if (open == false && addCard == false && removeCard == true)
        {
            // Juoksee kerran kun open false
            if (openChanger2 == true)
            {
                t = 0;
                openChanger2 = false;
                openChanger = true;
            }
            // Juoksee joka kerta

            // Counter pitää X arvon oikeana huomioiden poistuvan aseen arvot
            int counter = 0;

            for (int x = 0; x < updatedCardCount + 1; x++)
            {
                if (x != brokenWeaponInt)
                {
                    if (positions[x] != null)
                    {
                        transform.FindChild("Card" + x).localPosition = Vector3.Lerp(positions[x], new Vector3(minPositionX[counter], minPositionY[counter], 0f), t);
                        transform.FindChild("Card" + x).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[x].z, minAngles[counter], t), 0.5f);
                    }
                }
                else
                {
                    counter--;
                }
                counter++;
            }

            transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().sprite = cardArray[1];
            transform.FindChild("Card" + (brokenWeaponInt)).localPosition = Vector3.Lerp(positions[brokenWeaponInt], new Vector3(maxPositionX2[(brokenWeaponInt)], maxPositionY2[(brokenWeaponInt)], 0f), t);
            transform.FindChild("Card" + (brokenWeaponInt)).localRotation = new Quaternion(0f, 0f, Mathf.Lerp(rotations[brokenWeaponInt].z, maxAngles2[(brokenWeaponInt)], t), 0.5f);
            Color cardColor = transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetComponent<Image>().color = new Color(cardColor.r, cardColor.g, cardColor.b, Mathf.Lerp(1f, 0f, t));
            Color cardColor2 = transform.FindChild("Card" + (brokenWeaponInt)).GetChild(0).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetChild(0).GetComponent<Image>().color = new Color(cardColor2.r, cardColor2.g, cardColor2.b, Mathf.Lerp(1f, 0f, t));
            Color cardColor3 = transform.FindChild("Card" + (brokenWeaponInt)).GetChild(1).GetComponent<Image>().color;
            transform.FindChild("Card" + (brokenWeaponInt)).GetChild(1).GetComponent<Image>().color = new Color(cardColor3.r, cardColor3.g, cardColor3.b, Mathf.Lerp(1f, 0f, t));

            if (t >= 1)
            {
                removeCard = false;
                DestroyImmediate(transform.FindChild("Card" + (brokenWeaponInt)).gameObject);

                int tempInt = transform.childCount - 1;
                for (int x = 0; x < tempInt; x++)
                {
                    transform.GetChild(x).name = "Card" + x;
                }
            }
        }

        for (int x = 0; x < cardCount; x++)
        {
            if (transform.GetChild(x) != null)
            {
                float duration;
                try
                {
                    duration = PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<weaponStats>().duration;
                }
                catch
                {
                    duration = PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData[x].GetComponent<armorScript>().duration;
                }
                transform.GetChild(x).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1f - 0.1f * duration, 1f, 1f);
            }
        }
    }

    // Default arvoja rotationille
    private float[] rotationMax(int cards)
    {
        if (cards == 0) cards = 1;
        float[] max = new float[cards];

        switch (cards)
        {
            case 0:
                {
                    max[0] = 0f;
                    return new float[] { max[0] };
                }
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
                { break; }
        }
        Debug.LogError("Error on rotationMax method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien x positiolle
    private float[] positionMaxX(int cards)
    {
        if (cards == 0) cards = 1;
        float[] max = new float[cards];

        switch (cards)
        {
            case 0:
                {
                    max[0] = 0f;
                    return new float[] { max[0] };
                }
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
                { break; }
        }
        Debug.LogError("Error on positionMaxX method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien Y positiolle
    private float[] positionMaxY(int cards)
    {
        if (cards == 0) cards = 1;
        float[] max = new float[cards];

        switch (cards)
        {
            case 0:
                {
                    max[0] = 50f;
                    return new float[] { max[0] };
                }
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
                { break; }
        }
        Debug.LogError("Error on positionMaxY method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Metodi jolla voidaan tarkistella inventoryn kokoa
    public int inventorySize()
    {
        return PlayerScript.Player.GetComponent<PlayerScript>().Inventory.InventoryData.Count;
    }

    // Default arvoja rotationille
    private float[] rotationMin(int cards)
    {
        if (cards == 0) cards = 1;
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
                { break; }
        }
        Debug.LogError("Error on rotationMin method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien x positiolle
    private float[] positionMinX(int cards)
    {
        if (cards == 0) cards = 1;
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
                { break; }
        }
        Debug.LogError("Error on positionMinX method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    // Default arvoja korttien Y positiolle
    private float[] positionMinY(int cards)
    {
        if (cards == 0) cards = 1;
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
                { break; }
        }
        Debug.LogError("Error on positionMinY method");
        max[0] = 0.0f;
        return new float[] { max[0] };
    }

    private void saveCardProperties()
    {
        // Päivitetään arrayt oikean kokoisiksi
        positions = new Vector3[cardCount];
        rotations = new Quaternion[cardCount];

        // Koko pitää tarkistaa isomman mukaan koska välillä kortteja lisätään ja välillä poistetaan
        if (cardCount > updatedCardCount)
        {
            _weaponEquips = new bool[cardCount];
            _armorEquips = new bool[cardCount];
        }
        else
        {
            _weaponEquips = new bool[updatedCardCount];
            _armorEquips = new bool[updatedCardCount];
        }

        // Tuodaan arrayhin vanhat positionit sekä equip boolit
        for (int x = 0; x < cardCount; x++)
        {
            positions[x] = new Vector3(transform.FindChild("Card" + x).localPosition.x, transform.FindChild("Card" + x).localPosition.y, 0f);
            rotations[x] = new Quaternion(transform.FindChild("Card" + x).localRotation.x, transform.FindChild("Card" + x).localRotation.y, transform.FindChild("Card" + x).localRotation.z, transform.FindChild("Card" + x).localRotation.w);
            _weaponEquips[x] = transform.FindChild("Card" + x).GetComponent<CardMoverEraser>().checkEquip();
            _armorEquips[x] = transform.FindChild("Card" + x).GetComponent<CardMoverEraser>().checkEquip2();
        }
    }

    // Viimeksi tuhoutunut ase, käytetään cardmovereraser scriptissä
    public void lastBrokenWeapon(int weaponInt)
    {
        brokenWeaponInt = weaponInt;
    }

    public void dragTrue()
    {
        dragFlag = true;
    }

    public void dragFalse()
    {
        dragFlag = false;
    }

    public Vector3 OpenInvPositions(int Count)
    {
        Vector3 tempVector3 = new Vector3(maxPositionX[Count], maxPositionY[Count], 0f);
        return tempVector3;
    }

    // Metodi jolla voidaan cardmoveresarerista tarkistaa boolean arrayn arvoja
    public bool EquipArrayCheck(int ID)
    {
        if (_weaponEquips.Length > ID)
            return _weaponEquips[ID];
        else return false;
    }

    public void SetArmorActive(int ID)
    {
        if (activeArmor.Length > ID)
        {
            activeArmor[ID] = true;
        }
    }

    public void SetArmorUnactive(int ID)
    {
        if (activeArmor.Length > ID)
        {
            activeArmor[ID] = false;
        }
    }

    public bool ArmorWeaponCheck(int ID)
    {
        if (armorInv.Length > ID)
        {
            if (armorInv[ID] == true)
            {
                return true;
            }
        }
        return false;
    }

    // Metodi jolla voidaan tarkistaa onko inventoryssä armoria päällä
    public bool ActiveArmorCheck(int ID)
    {
        if (activeArmor.Length > ID)
            return activeArmor[ID];
        else return false;
    }

    // Fläg onko armor päällä vai ei
    private bool ArmorActiveCheck()
    {
        bool flag = false;
        for (int x = 0; x < activeArmor.Length; x++)
        {
            if (activeArmor[x] == true)
                flag = true;
        }
        return flag;
    }
}
