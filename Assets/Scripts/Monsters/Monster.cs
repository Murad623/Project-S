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
    private MonsterAnimationManager _monAnimManager;
    private void Start() {
        _monAnimManager = GetComponent<MonsterAnimationManager>();
    }
    private void Update() {
        if(!_monAnimManager.CheckSenseSomethingST || !_monAnimManager.CheckSenseSomethingRPT){
            _distance = Vector3.Distance(transform.position,Player.Get().transform.position);
            if(_distance <= 5 && PlayerMove.Speed > 0) onSenseSomethingST?.Invoke(true);
            if(_distance > 5 && _distance <= 20 && PlayerMove.Speed > 1) onSenseSomethingRPT?.Invoke(true);
        }
    }
}
