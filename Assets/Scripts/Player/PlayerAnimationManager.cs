using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationManager : LazyLoadManager<PlayerAnimationManager>
{
    private Animator _playerAnimator;
    private void OnEnable() {
        Player.onAttack += Attack;
        Player.onDefend += Defend;
    }
    private void Start() {
        _playerAnimator = Player.Get().GetComponent<Animator>();
    }
    private void Update() {
        if (_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            _playerAnimator.SetBool("Attack1",false);
        }
        else if (_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            _playerAnimator.SetBool("Attack2",false);
        }
    }
    private void Attack(bool attack1) {
        if (attack1) _playerAnimator.SetBool("Attack1",true);
        else _playerAnimator.SetBool("Attack2",true);
    }
    private void Defend(bool defend){
        _playerAnimator.SetBool("Defend",defend);
    }
    private void OnDisable() {
        Player.onAttack -= Attack;
    }
}
