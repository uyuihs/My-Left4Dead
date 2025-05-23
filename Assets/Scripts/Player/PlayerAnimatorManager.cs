using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerAnimatorManager : NetworkBehaviour
{
    //=============Player移动动画相关逻辑===============
    private Animator animator;
    private PlayerNetworkManager playerNetworkManager;//玩家网络管理器的引用
    private int horizontalHash;//缓存哈希值，减少计算量
    private int verticalHash;
    private float moveAmount;//移动量0,0.5,1
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");
    }
    public void Move(float _moveAmount)
    {
        moveAmount = _moveAmount;
    }

    private void PerformAnimatorMove()
    {
        //设置动画参数
        animator.SetFloat(horizontalHash, 0f, MagicNumber.Singleton.smoothTime, Time.fixedDeltaTime);//平滑过渡播放动画
        animator.SetFloat(verticalHash, moveAmount, MagicNumber.Singleton.smoothTime, Time.fixedDeltaTime);
    }


    public void PlayTargetAnimation(string targetAnim)
    {//播放目标动画
        animator.applyRootMotion = PlayerMoveStatus.Singleton.IsEnableRootMotion();
        animator.CrossFade(targetAnim, MagicNumber.Singleton.smoothTime);
    }


    public void SetAnimator(AnimatorOverrideController _animator)
    {
        animator.runtimeAnimatorController = _animator;
    }


    private void FixedUpdate()
    {
        if (IsOwner)
        {
            playerNetworkManager.MoveAmount = moveAmount;
        }
        else
        {
            moveAmount = playerNetworkManager.MoveAmount;
        }
        PerformAnimatorMove();
    }


    

}
