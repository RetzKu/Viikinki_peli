using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public static BaseManager Instance = null;
    public float CraftRange = 5f;

    private GameObject _player;
    private BaseChecker _baseChecker;
    private List<GameObject> bases = new List<GameObject>(3);
    private BaseChecker _InfoStoneChecker;

    void Awake()
    {
        Instance = this;
        _player = GameObject.FindWithTag("Player");

        // TODO: liian vaikea luoda 
        var go = new GameObject("Base Checker");
        go.transform.parent = _player.transform.parent;
        var checker = go.AddComponent<BaseChecker>();
        checker.CraftRange = CraftRange;
        _baseChecker = go.GetComponent<BaseChecker>();
        _baseChecker.SetMask("Base"); // voisi olla parameter




        if (_player == null || _baseChecker == null)
        {
            Debug.LogWarning("WARNING: BaseManager.cs basechecker null");
        }

        var infoStoneGameObject = new GameObject("Info Stone Checker");
        infoStoneGameObject.transform.parent = _player.transform.parent;

        // var infoStoneChecker = go.AddComponent<BaseChecker>();
        checker.CraftRange = 5f;
        _InfoStoneChecker = checker;
    }

    void Update()
    {
        _baseChecker.CircleCast(_player.transform.position);
        _InfoStoneChecker.CircleCast(_player.transform.position);
    }

    // FastTravel menut jne tänne: vaatii chunkkien uudestaan inilisoinnin tilemapissa
    // lisää unlockatut campit tänne

    public void RegisterOnBaseEnter(Action func)
    {
        _baseChecker.OnEnter += func;
    }
    public void RegisterOnBaseExit(Action func)
    {
        _baseChecker.OnExit += func;
    }
    public void UnRegisterOnBaseEnter(Action func)
    {
        _baseChecker.OnExit -= func;
    }
    public void UnRegisterOnBaseExit(Action func)
    {
        _baseChecker.OnExit -= func;
    }


    public GameObject GetClosestBase()
    {
        // vain yksi vielä
        if (bases.Count == 0)
        {
            Debug.LogError("BaseManager.cs: no bases when requesting closest base");
            return null;
        }
        return bases[0]; // en toimi
    }

    public void AddBase(GameObject go)
    {
        bases.Add(go);
    }
}
