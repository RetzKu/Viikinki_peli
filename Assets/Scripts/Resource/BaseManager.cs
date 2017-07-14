using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseManager : MonoBehaviour
{
    public static BaseManager Instance = null;

    private GameObject _baseCheckerGo;
    private BaseChecker _baseChecker;

    void Awake()
    {
        Instance = this;
        _baseCheckerGo = GameObject.FindWithTag("BaseChecker");
        _baseChecker = _baseCheckerGo.GetComponent<BaseChecker>();

        if (_baseCheckerGo == null || _baseChecker == null)
        {
            Debug.LogWarning("WARNING: BaseManager.cs basechecker null");
        }
    }

    // FastTravel menut jne tänne
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
}
