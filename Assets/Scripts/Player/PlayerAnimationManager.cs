using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationManager : LazyLoadManager<PlayerAnimationManager>
{
    public static Animator playerAnimator;
    private bool _checkAttack = false;
    private bool _checkAttack1 = false;
    private bool _checkAttack2 = false;
    private bool _checkMove = false;
    private bool _setSTime = false;
    private float _sTime = 0;
    private float _delay = 2.0f;
    private void OnEnable(){
        Player.onAttack += Attack;
        Player.onDefend += Defend;
        Player.onGetHit += GetHit;
        Player.onRevive += Revive;
        Player.onDizzy += Dizzy;
        PlayerMove.onMoveAnim += MoveAnim;
    }
    private void Start(){
        playerAnimator = Player.Get().GetComponentInChildren<Animator>();
    }
    private void Update(){
        if(playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack01")){
            _checkAttack = true;
            _checkAttack1 = true;
        }
        if(playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack02")){
            _checkAttack = true;
            _checkAttack2 = true;
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("GetHit")){
            playerAnimator.SetBool("Hit",false);
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("Die")){
            playerAnimator.SetBool("Die",false);
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("DieRecover")){
            playerAnimator.SetBool("Revive",false);
            _checkMove = true;
        }
        if(playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("Dizzy")){
            if(_setSTime){
                _sTime = Time.time;
                _setSTime = false;
            }
            if (playerAnimator.GetBool("Dizzy") && _delay <= Time.time - _sTime){
                playerAnimator.SetBool("Dizzy", false);
                _checkMove = true;
            }
        }
        if(_checkMove && 
        !playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("DieRecover") &&
        !playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("Dizzy")){
            PlayerMove.canMove = true;
            _checkMove = false;
        }
        if(_checkAttack &&
        !playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack01") &&
        !playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack02")){
            Player.Attack = 0;
            _checkAttack = false;
            print("Attacks checked");
        }
        if (_checkAttack1 &&
        !playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack01")){
            Player.AttackCount--;
            _checkAttack1 = false;
        }
        if (_checkAttack2 &&
        !playerAnimator.GetCurrentAnimatorStateInfo(2).IsName("Attack02")){
            Player.AttackCount--;
            _checkAttack2 = false;
        }
    }
    private void Attack(int attack){
        playerAnimator.SetInteger("Attack",attack);
    }
    private void Defend(bool defend){
        playerAnimator.SetBool("Defend",defend);
    }
    private void MoveAnim(float MoveSpeed){
        playerAnimator.SetFloat("MoveSpeed",MoveSpeed);
    }
    private void GetHit(int damage){
        if (Player.Get().Health > 0) playerAnimator.SetBool("Hit",true);
        else playerAnimator.SetBool("Die",true);
    }
    private void Revive(){
        playerAnimator.SetBool("Revive", true);
        playerAnimator.SetBool("Dizzy", false);
    }
    private void Dizzy(){
        if(!playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("Dizzy")){
            playerAnimator.SetBool("Dizzy", true);
            _sTime = Time.time;
            _setSTime = true;
        }
    }
    private void OnDisable(){
        Player.onAttack -= Attack;
        Player.onDefend -= Defend;
        Player.onGetHit -= GetHit;
        Player.onRevive -= Revive;
        Player.onDizzy -= Dizzy;
        PlayerMove.onMoveAnim -= MoveAnim;
    }
}
