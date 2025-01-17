using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : LazyLoadManager<Player>
{
    private long _sTime = 0;
    private int _delay = 1;
    private long _now = 0;
    private bool _attack1 = false;
    public bool Attack1{
        get { return _attack1; }
    }
    private bool _attack2 = false;
    public bool Attack2{
        get { return _attack2; }
    }
    private bool _defend = false;
    public bool Defend {
        get { return _defend; }
    }
    private float _speed;
    public float Speed {
        get { return _speed; }
        set { _speed = value; }
    }
    private float _horizontal;
    private float _vertical;
    public static event Action<bool> onAttack;
    public static event Action<bool> onDefend;
    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            _now = (long)(Time.time-Time.time%1);
            if (!_attack1){
                _attack1 = true;
                onAttack?.Invoke(true);
            }
            else{
                _attack2 = true;
                _attack1 = false;
                onAttack?.Invoke(false);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            onDefend?.Invoke(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            onDefend?.Invoke(false);
        }
        if (_delay <= Time.time-Time.time%1 - _sTime) _sTime = (long)(Time.time-Time.time%1);
        if (_attack1) if (_delay <= Time.time-Time.time%1 - _now) _attack1 = false;
    }
}
