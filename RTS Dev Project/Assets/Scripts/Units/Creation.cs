using UnityEngine;
using System.Collections;

public struct Creation
{
    public float time;
    public Sprite sprite;
    public Action action;

    public Creation( float _time, Sprite _sprite, Action _action )
    {
        time = _time;
        sprite = _sprite;
        action = _action;
    }
}
