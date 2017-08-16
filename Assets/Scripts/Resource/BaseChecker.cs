using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseChecker : MonoBehaviour
{
    public float CraftRange = 4f;
    public Action OnEnter;
    public Action OnExit;

    private bool _lastFrameOnBase = false;
    private int _baseMask;
    private GameObject _player;

    public List<InfoStone> ClosebyInfoStones = new List<InfoStone>(2);

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        // _baseMask = LayerMask.GetMask(CheckMask);
    }

    public void SetMask(params string[] checkMask)
    {
        _baseMask = LayerMask.GetMask(checkMask);
    }

    public void CircleCast(Vector3 position)
    {
        var hit = Physics2D.CircleCast(position, CraftRange, Vector2.zero, 0f, _baseMask);

        if (hit) // bases nearby
        {
            if (!_lastFrameOnBase && OnEnter != null)
            {
                OnEnter();
                _lastFrameOnBase = true;
            }
        }
        else
        {
            if (_lastFrameOnBase && OnExit != null)
            {
                OnExit();
                _lastFrameOnBase = false;
            }
        }

        hit = Physics2D.CircleCast(position, CraftRange, Vector2.zero, 0f, LayerMask.GetMask("RuneStone"));
        // Tee array
        var RunestoneCast = Physics2D.CircleCastAll(position, 5, Vector2.zero, 0f, LayerMask.GetMask("RuneStone"));

        ClosebyInfoStones.Clear();
        if (RunestoneCast.Length != 0)
        {
            foreach (RaycastHit2D t in RunestoneCast)
            {
                var infostone = t.transform.GetComponent<InfoStone>();
                infostone.AlphaEffect();
                ClosebyInfoStones.Add(infostone);
                // reki
            }
        }

        //if (hit) // runeStoneNearby!
        //{
        //    // !TODO: tänään aloitetaan tosta!
        //    //hit.transform.gameObject.GetComponent<InfoStone>().Vibrate();

        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(0, 0);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(1, 0);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(2, 0);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(1, 1);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(0, 2);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(1, 2);
        //    //GameObject.FindGameObjectWithTag("ResourceUiController").GetComponent<CraftingUiController>().Vibrate(2, 2);
        //}
        //else
        //{
        //}
    }

    public void SendIndices(Vec2[] positions, int[] touchCounts)
    {
        var RunestoneCast = Physics2D.CircleCastAll(_player.transform.position, 5, Vector2.zero, 0f, LayerMask.GetMask("RuneStone"));
        if (RunestoneCast.Length != 0)
        {
            foreach (RaycastHit2D t in RunestoneCast)
            {
                var infostone = t.transform.GetComponent<InfoStone>();
                infostone.AlphaEffect();
                ClosebyInfoStones.Add(infostone);
                infostone.TryToTeachRune(positions, touchCounts);
            }
        }
    }
}
