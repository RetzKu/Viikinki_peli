using System;
using UnityEngine;



/// <summary>
/// Luokka, joka ei toimi
/// </summary>

public class SceneSaver : MonoBehaviour
{

    // GameObject.FindObjectsOfType(typeof(MonoBehaviour)); //returns Object[]
    // GameObject.FindGameObjectsWithTag("Untagged");  //returns GameObject[]

    private GameObject[] _gameObjects;
    private GameObject Parent;
    private String[] _tags;
    private int _realLength;

    void Start()
    {
        // GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        Parent = new GameObject("Saved");
        Parent.tag = "saved";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            GameObject[] gos = UnityEngine.Object.FindObjectsOfType<GameObject>();
            _gameObjects = new GameObject[gos.Length];
            _tags = new string[gos.Length];

            int y = 0;
            for (int i = 0; i < gos.Length; i++)
            {
                if (!gos[i].transform.parent)
                {
                    _gameObjects[y] = Instantiate(gos[i]);
                    _gameObjects[y].SetActive(false);
                    _gameObjects[y].transform.parent = Parent.transform;

                    _tags[y] = gos[i].gameObject.tag;
                    _gameObjects[y].tag = "saved";
                    y++;
                }
            }
            _realLength = y;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // GameObject[] sceneObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (i < _realLength - 1)
                {
                    if (gameObjects[i] != null && gameObjects[i].tag != "saved")
                    {
                        Destroy(gameObjects[i]);
                        _gameObjects[i].SetActive(true);
                        gameObjects[i] = Instantiate(_gameObjects[i]);
                        gameObjects[i].SetActive(true);
                        gameObjects[i].tag = _tags[i];
                    }
                    else
                    {
                        Destroy(_gameObjects[i]);
                        Destroy(gameObjects[i]);
                    }
                }
                else
                {
                    Destroy(gameObjects[i]);
                }
            }
        }

    }
}
