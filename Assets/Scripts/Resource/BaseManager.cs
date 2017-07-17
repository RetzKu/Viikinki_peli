using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseManager : MonoBehaviour
{
    public static BaseManager Instance = null;

    private GameObject _baseCheckerGo;
    private BaseChecker _baseChecker;

    private List<GameObject> bases = new List<GameObject>(3);

    void Awake()
    {
        Instance = this;
        _baseCheckerGo = GameObject.FindWithTag("Player");
        _baseChecker = _baseCheckerGo.GetComponent<BaseChecker>();

        if (_baseCheckerGo == null || _baseChecker == null)
        {
            Debug.LogWarning("WARNING: BaseManager.cs basechecker null");
        }
    }

    // FastTravel menut jne tänne: vaatii chunkkien uudestaan inilisoinnin tilemapissa
    // lisää unlockatut campit tänne

    public void RegisterOnBaseEnter(Action func)
    {
        _baseChecker.OnCampFireEnter += func;
    }

    public void RegisterOnBaseExit(Action func)
    {
        _baseChecker.OnCampFireExit += func;
    }

    public void UnRegisterOnBaseEnter(Action func)
    {
        _baseChecker.OnCampFireExit -= func;
    }

    public void UnRegisterOnBaseExit(Action func)
    {
        _baseChecker.OnCampFireExit -= func;
    }

    public GameObject GetClosestBase()
    {
        // vain yksi vielä
        if (bases.Count == 0)
        {
            Debug.LogError("BaseManager.cs: no bases when requesting closest base");
            return null;
        }
        return bases[0];
    }

    public void AddBase(GameObject go)
    {
        bases.Add(go);
    }
}
