using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : LazyLoadManager<PlayerMove>
{
    // private CharacterController _charControler;
    private float _sTime = 0;
    private float _delay = 0.5f;
    // private bool _checkDoubleClick = true; // Double click state checker to alternative double click model
    private static float _speed = 1;
    public static float Speed {
        get { return _speed; }
        set { _speed = value; }
    }
    private float _lookSmooth = 0.05f;
    private float _directionSmooth = 0.02f;
    private Vector3 inputDirection = Vector3.zero;
    private Vector3 effectiveDirection = Vector3.zero;
    private float _horizontal, _vertical;
    public static bool canMove = true;
    public static event Action<float> onMoveAnim;
    private void OnEnable(){
        Player.onDizzy += onDizzy;
    }
    private void Start(){
        // _charControler = GetComponent<CharacterController>();
    }
    private void Update(){
        if(canMove && Player.Get().Health > 0){
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            inputDirection.x = _horizontal;
            inputDirection.z = _vertical;
            inputDirection.Normalize();

            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)){
                if(_speed < 2){
                    if (Time.time - _sTime <= _delay) _speed = 2;
                    else _speed = 1;
                    _sTime = Time.time;
                }
            }
            if(_horizontal == 0 && _vertical == 0) _speed = 0;

            // Alternative double click model
            // if(_checkDoubleClick && (_horizontal != 0 || _vertical != 0)){
            //     if(_speed < 2){
            //         if (Time.time - _sTime <= _delay) _speed = 2;
            //         else _speed = 1;
            //         _sTime = Time.time;
            //     }
            //     _checkDoubleClick = false;
            // }
            // if (_horizontal == 0 && _vertical == 0) {
            //     _checkDoubleClick = true;
            //     _speed = 0;
            // }

            if(inputDirection.magnitude > 0.01){
                float lookAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                float effectiveAngel = Mathf.LerpAngle(transform.rotation.eulerAngles.y, lookAngle, _lookSmooth);
                transform.rotation = Quaternion.Euler(0,effectiveAngel,0);
            }
            effectiveDirection = Vector3.Lerp(effectiveDirection, inputDirection, _directionSmooth);
            // _charControler.Move(effectiveDirection * Speed * Time.deltaTime);
            transform.position += effectiveDirection * Speed * Time.deltaTime;
            onMoveAnim?.Invoke(_speed);
        }
    }
    private void onDizzy(){
        canMove = false;
    }
    private void OnDisable(){
        Player.onDizzy -= onDizzy;
    }
}
