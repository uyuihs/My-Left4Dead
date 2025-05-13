using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.Callbacks;

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

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
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
        //玩家的移动方向以屏幕方向为基，水平移动量和竖直移动量的线性组合
        moveDirection = 
            (PlayerCamera.Singleton.transform.right * moveHorizontal  + 
            PlayerCamera.Singleton.transform.forward * moveVertical).normalized * MagicNumber.Singleton.movespeed;

        moveDirection.y = 0;//y轴不参与移动
        rb.MovePosition(rb.position + moveDirection * moveAmount * Time.fixedDeltaTime);
        PlayerCamera.Singleton.SetPosition(transform.position);
    }

    private void HandlePlayerRotation(){
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
            playerNetworkManager.PlayerPosition = transform.position;
            playerNetworkManager.PlayerRotation = transform.rotation;            
        }
        else {
            transform.position = playerNetworkManager.PlayerPosition;
            transform.rotation = playerNetworkManager.PlayerRotation;
        }
    }

    private void FixedUpdate() {
        PerformMove();
    }

}
