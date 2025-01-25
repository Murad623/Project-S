using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : LazyLoadManager<Player>
{
    private int _health = 100;
    public int Health{
        get { return _health; }
        set {
            if (value >= 0) _health = value;
            else _health = 0;
        }
    }
    private float _sTime = 0;
    private float _delay = 0.5f;
    private static int _attack = 0;
    public static int Attack{
        get { return _attack; }
        set { 
            if(value <= 3 && value >= 0){
                _attack = value;
                onAttack?.Invoke(_attack);
            }
        }
    }
    private static int _attackCount = 0;
    public static int AttackCount{
        get { return _attackCount; }
        set { 
            if(value <= 2 && value >= 0) _attackCount = value;
            if(value == 0) Attack = value;
        }
    }
    private bool _defend = false;
    public bool Defend {
        get { return _defend; }
    }
    public static event Action<int> onAttack;
    public static event Action<bool> onDefend;
    public static event Action<int> onGetHit;
    public static event Action onRevive;
    public static event Action onDizzy;
    private void Update() {
        // Attack
        if (Input.GetMouseButtonDown(0)){
            if (Time.time - _sTime <= _delay) Attack++;
            else Attack = 1;
            _sTime = Time.time;
            AttackCount++;
        }
        // Defend
        if (Input.GetMouseButtonDown(1)) onDefend?.Invoke(true);
        if (Input.GetMouseButtonUp(1)) onDefend?.Invoke(false);

        // if (_delay <= Time.time-Time.time%1 - _sTime) _sTime = (long)(Time.time-Time.time%1);

        // damage test
        if(Input.GetKeyDown(KeyCode.Q)){
            GetHit(40);
            Dizzy();
        }
        // revive test
        if (Input.GetKeyDown(KeyCode.R)){
            Revive(50);
        }
        // dizzy test
        if (Input.GetKeyDown(KeyCode.E)){
            Dizzy();
        }
    }
    public void GetHit(int damage){
        if(Health > 0){
            Health -= damage;
            onGetHit?.Invoke(damage);
        }
    }
    public void Revive(int heal){
        Health += heal;
        onRevive?.Invoke();
    }
    public void Dizzy(){
        onDizzy?.Invoke();
    }
}
