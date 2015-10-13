using UnityEngine;
using System.Collections;

public struct Creation
{
    public float time;
    public Sprite sprite;
    public Command action;

    public Creation( float _time, Sprite _sprite, Command _action )
    {
        time = _time;
        sprite = _sprite;
        action = _action;
    }
}
