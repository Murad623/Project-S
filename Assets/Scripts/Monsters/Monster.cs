using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private float _distance;
    public event Action<bool> onSenseSomethingST;
    public event Action<bool> onSenseSomethingRPT;
    public event Action onLookAround;
    private MonsterAnimationManager _monAnimManager;
    private FieldOfView _fov;
    private void Start() {
        _monAnimManager = GetComponent<MonsterAnimationManager>();
        _fov = GetComponent<FieldOfView>();
    }
    private void Update() {
        if(!_monAnimManager.MonsterAnimator.GetBool("Attack") && (!_monAnimManager.CheckSenseSomethingST || !_monAnimManager.CheckSenseSomethingRPT)){
            _distance = Vector3.Distance(transform.position,Player.Get().transform.position);
            if(_distance <= 5 && PlayerMove.Speed > 0){
                onSenseSomethingST?.Invoke(true);
                onLookAround?.Invoke();
                // _fov.angle = 160;
            }
            if(_distance > 5 && _distance <= 15 && PlayerMove.Speed > 1){
                onSenseSomethingRPT?.Invoke(true);
                // _fov.angle = 160;
            }
        }
    }
}
