using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAnimationManager : MonoBehaviour
{
    private Animator _monsterAnimator;
    private bool _checkSenseSomething = false;
    private bool _checkSenseSomethingST = false;
    public bool CheckSenseSomethingST {
        get { return _checkSenseSomethingST; }
    }
    private bool _checkSenseSomethingRPT = false;
    public bool CheckSenseSomethingRPT {
        get { return _checkSenseSomethingRPT; }
    }
    private void OnEnable() {
        GetComponent<FieldOfView>().onCanSee += CanSee;
        GetComponent<Monster>().onSenseSomethingST += SenseSomethingST;
        GetComponent<Monster>().onSenseSomethingRPT += SenseSomethingRPT;
        _monsterAnimator = GetComponent<Animator>();
    }
    private void Update() {
        if(_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingST")){
            // _checkSenseSomething = true;
            _checkSenseSomethingST = true;
        }
        if(_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingRPT")){
            // _checkSenseSomething = true;
            _checkSenseSomethingRPT = true;
        }
        // if (_checkSenseSomething &&
        // !_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingST") &&
        // !_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingRPT")){
        //     _checkSenseSomething = false;
        //     _checkSenseSomethingST = false;
        //     _checkSenseSomethingRPT = false;
        // }
        if (_checkSenseSomethingST &&
        !_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingST")){
            _monsterAnimator.SetBool("SenseSomethingST",false);
            _checkSenseSomethingST = false;
        }
        if (_checkSenseSomethingRPT &&
        !_monsterAnimator.GetCurrentAnimatorStateInfo(2).IsName("SenseSomethingRPT")){
            _monsterAnimator.SetBool("SenseSomethingRPT",false);
            _checkSenseSomethingRPT = false;
        }
    }
    private void CanSee(bool canSeePlayer){
        _monsterAnimator.SetBool("Attack",canSeePlayer);
    }
    private void SenseSomethingST(bool SenseSomething){
        _monsterAnimator.SetBool("SenseSomethingST",SenseSomething);
    }
    private void SenseSomethingRPT(bool SenseSomething){
        _monsterAnimator.SetBool("SenseSomethingRPT",SenseSomething);
    }
    private void OnDisable() {
        GetComponent<FieldOfView>().onCanSee -= CanSee;
        GetComponent<Monster>().onSenseSomethingST -= SenseSomethingST;
        GetComponent<Monster>().onSenseSomethingRPT += SenseSomethingRPT;
    }
}
