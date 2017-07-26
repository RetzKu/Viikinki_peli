using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private Transform _player;
    private Vector3 _offset;

	void Start ()
	{
	    _player = GameObject.FindWithTag("Player").transform;
	    _offset = transform.position - _player.position;
	}
	
	void Update ()
	{
	    transform.position = _player.position + _offset;
	}
}
