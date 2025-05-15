using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLocomotionManager : NetworkBehaviour {
    
    //=============Player移动相关逻辑===============
    private Rigidbody rb;
    private Vector2 playerMove;//玩家移动的输入
    private float moveVertical;//垂直移动量
    private float moveHorizontal;//水平移动量
    private float moveAmount;//移动量0,0.5,1
    private Vector3 moveDirection = Vector3.zero;//移动方向
    private Quaternion targetRotation = Quaternion.identity;//目标旋转
    private Vector3 targetDirection = Vector3.zero;
    private PlayerNetworkManager playerNetworkManager;//玩家网络管理器的引用

    //============Player下落、跳跃相关逻辑===========
    [SerializeField] private LayerMask groundLayer;
    private PlayerAnimatorManager playerAnimatorManager;

    //============翻滚相关逻辑================
    private Vector3 rollDirection = Vector3.zero;


    //============debug相关输入==============
    private Vector3 lastPosition = Vector3.zero;

    //============耐力相关===================
    private uint rollDec = 10;
    private const uint sprintDecNum = 5;
    private float sprintDecTick = 0;
    private const float SprintTickLimit = 1f;
    private const float staminaTickLimit = 2.5f;//计时器大于该门限值，开始回复耐力
    private float staminaTickCount = 0;
    private const float staminaTickBlockTimeLimit = 1f;//恢复的单位为1秒
    private float staminaTickBlockTime = 0;
    private uint staminaRecover = 3;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        lastPosition = transform.position;
    }

    //接受input的输入
    public void Move(Vector2 move, float _moveAmount){
        //拿到x和y方向的输入
        playerMove = move;
        moveVertical = playerMove.y;
        moveHorizontal = playerMove.x;
        moveAmount = _moveAmount;
    }

    private void HandlePlayerMovement(){
        if (!PlayerMoveStatus.Singleton.IsEnableMove()) { return; }
        
        //玩家的移动方向以屏幕方向为基，水平移动量和竖直移动量的线性组合
        moveDirection = 
            (PlayerCamera.Singleton.transform.right * moveHorizontal  + 
            PlayerCamera.Singleton.transform.forward * moveVertical).normalized * MagicNumber.Singleton.movespeed;

        moveDirection.y = 0;//y轴不参与移动
        rb.MovePosition(rb.position + moveDirection * moveAmount * Time.fixedDeltaTime);
        PlayerCamera.Singleton.SetPosition(transform.position);
        
        //若是在奔跑，则每秒减少耐力值
        if(PlayerMoveStatus.Singleton.IsSprint()){
            sprintDecTick += Time.fixedDeltaTime;
            if(sprintDecTick >= SprintTickLimit){
                sprintDecTick = MagicNumber.Singleton.zeroEps;
                playerNetworkManager.DecStamina(sprintDecNum);
            }
        }
    }

    private void HandlePlayerRotation(){
        if(!PlayerMoveStatus.Singleton.IsEnableRoate()) { return; }

        //玩家的旋转的旋转方向以屏幕方向为基，水平移动量和竖直移动量的线性组合
        targetDirection = moveDirection == Vector3.zero? transform.forward : moveDirection;
        targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, MagicNumber.Singleton.smoothTime);
        transform.rotation = targetRotation;
    }

    //实现玩家移动
    private void PerformMove(){
        //处理玩家移动，同步玩家位置
        if(IsOwner){
            HandlePlayerMovement();
            HandlePlayerRotation();
            AtemptedRecoverStamina();
            playerNetworkManager.PlayerPosition = transform.position;
            playerNetworkManager.PlayerRotation = transform.rotation;            
        }
        else {
            transform.position = playerNetworkManager.PlayerPosition;
            transform.rotation = playerNetworkManager.PlayerRotation;
        }
    }

    public void AtemptedPerformDodge(){
        if(PlayerMoveStatus.Singleton.IsAnimationLocked()) { return; }//当前在播放不可以打断的动画，返回

        if(playerNetworkManager.CurrentStamina <= 0) { return; }

        rollDirection  = 
            (PlayerCamera.Singleton.transform.right * moveHorizontal  + 
            PlayerCamera.Singleton.transform.forward * moveVertical).normalized;

            Quaternion rotation = Quaternion.LookRotation(rollDirection);
            transform.rotation = rotation;

            //玩家状态：允许根运动、锁定动画，不允许切换
            PlayerMoveStatus.Singleton.EnableAnimationLocked();
            PlayerMoveStatus.Singleton.EnableRootMotion();
            //TODO：播放动画
            
        if (moveAmount > MagicNumber.Singleton.zeroEps){//非静止状态时，翻滚
            playerNetworkManager.PlayTargetAnimationServerRpc("Rolling");
        }

        else {//否则为向后跳跃
            playerNetworkManager.PlayTargetAnimationServerRpc("BackStep");
        }

        playerNetworkManager.DecStamina(rollDec);
    }

    public void AtemptedRecoverStamina(){
        //没有处于动画锁定状态、冲刺状态、或当前耐力值等于最大耐力值时，不能恢复
        if (PlayerMoveStatus.Singleton.IsAnimationLocked()|| 
            PlayerMoveStatus.Singleton.IsSprint() || 
            playerNetworkManager.CurrentStamina == playerNetworkManager.MaxStamina) {
                staminaTickCount = 0;
                return;
        }
        staminaTickCount += Time.fixedDeltaTime;

        if(staminaTickCount >= staminaTickLimit){
            staminaTickBlockTime += Time.fixedDeltaTime;
            if(staminaTickBlockTime >= staminaTickBlockTimeLimit){
                staminaTickBlockTime = 0;
                staminaTickBlockTime = MagicNumber.Singleton.zeroEps;
                playerNetworkManager.IncStamina(staminaRecover);
            }
        }

    }


    private void FixedUpdate() {
        PerformMove();

    }

}
