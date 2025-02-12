using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private bool _turnToPlayer;
    private bool _lookAround;
    private bool _lookingAround;
    private bool _targetAngleSetted = false;
    private int _lookingTime;
    private int _lookBothSide = 0;
    private int _maxLookAngel = 4;
    private int _minLookAngel = 1;
    private float _distanceBetween;
    private float _targetAngle;
    private float _lookSmooth = 0.05f;
    private Vector3 inputDirection = Vector3.zero;
    private Animator _monsterAnimator;
    private void OnEnable() {
        GetComponent<FieldOfView>().onCanSee += TurnToPlayer;
        GetComponent<Monster>().onLookAround += SenseSomething;
        InvokeRepeating("LookAround",1.4f,5.2f);
    }
    private void Start() {
        _monsterAnimator = GetComponent<MonsterAnimationManager>().MonsterAnimator;
    }
    private void Update() {
        if(_turnToPlayer){
            inputDirection = Player.Get().transform.position - transform.position;
            inputDirection.Normalize();
            Turn((-Mathf.Atan2(inputDirection.z,inputDirection.x) * Mathf.Rad2Deg)+90);
        }
        else{
            if(_lookingAround) TurnAround();
        }
    }
    private void TurnToPlayer(bool turn){
        _turnToPlayer = turn;
    }
    private void SenseSomething(){
        _maxLookAngel = 13;
        _minLookAngel = 6;
        _monsterAnimator.SetBool("SenseSomethingRPT",false);
        _monsterAnimator.SetBool("SenseSomethingST",true);
    }
    private void LookAround(){
        if(!_lookingAround){
            _lookAround = true;
            _lookingAround = true;
            _lookingTime = Random.Range(1,4);
            _targetAngleSetted = false;
            // if(!_monsterAnimator.GetBool("SenseSomethingST"))_monsterAnimator.SetBool("SenseSomethingRPT",true);
        }
    }
    private void TurnAround(){
        if(_lookAround && _lookingTime > 0){
            // Choice turn angle
            if(!_targetAngleSetted){
                if(_lookBothSide == 0){
                    _targetAngle = Random.Range(_minLookAngel,_maxLookAngel)*10;
                    _lookBothSide = 3;
                }
                else if(_lookBothSide == 2) _targetAngle = 360-_targetAngle;
                else _targetAngle = 0;
                _targetAngleSetted = true;
                if(!_monsterAnimator.GetBool("SenseSomethingST"))_monsterAnimator.SetBool("SenseSomethingRPT",true);
            }
            Turn(_targetAngle);
            // if angle equal to turn angle calling wait function
            if(Mathf.Round(transform.rotation.eulerAngles.y) == _targetAngle){
                _lookAround = false;
                _lookBothSide--;
                _targetAngleSetted = false;
                if(_lookBothSide == 0) _lookingTime--;
                if(_lookingTime <= 0){
                    _lookingAround = false;
                    _maxLookAngel = 4;
                    _minLookAngel = 1;
                    _monsterAnimator.SetBool("SenseSomethingRPT",false);
                    return;
                }
                StartCoroutine(nameof(Wait));
            }
        }
    }
    private void Turn(float angle){
        // Calculating angle for smooth turn
        float effectiveAngel = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, _lookSmooth);
        // turn
        transform.rotation = Quaternion.Euler(0,effectiveAngel,0);
    }
    IEnumerator Wait(){
        yield return new WaitForSeconds(1.4f);
        _lookAround = true;
    }
    private void OnDisable() {
        GetComponent<FieldOfView>().onCanSee -= TurnToPlayer;
        GetComponent<Monster>().onLookAround -= SenseSomething;
    }
}
