using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : Singleton<ResetManager>
{
    Vector2 _startPos;
    public GameObject _gameObject;

    private void Awake()
    {
        _startPos = transform.position;
        _gameObject.transform.position = _startPos;
    }
    public void ResetPos()
    {
        _gameObject.transform.position = _startPos;
    }
}
