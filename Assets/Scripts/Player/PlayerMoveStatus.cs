using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStatus : LocalFlags{   
    //单根模式
    public static PlayerMoveStatus Singleton;
    private void Awake() {
        if(Singleton == null){
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        moveFlags = RootMotion | Rotate | Move ;
    }

    //============Player移动相关属性的标志位==================

    /// <summary>
    /// 用位运算表示使能动作，当为1时，表示enable,当为0时，表示disable
    /// 初始状态：未锁定animation
    /// </summary>
    // [Header("flags"), SerializeField]
    private uint moveFlags;//标志位
    private const uint AnimationLocked = 0x00000001;//播放不可以打断动画的Mask
    private const uint Move = 0x00000002;//允许角色移动的Mask
    private const uint Rotate = 0x00000004;//允许角色旋转的Mask
    private const uint RootMotion = 0x00000008;//允许角色使用RootMotion
    private const uint Sprint = 0x00000010;//角色是否在冲刺的Mask

    //===========检测、使能、使不能=============================
    private bool DetectMoveFlags(uint flag) { return (moveFlags & flag) == flag; }
    private PlayerMoveStatus EnableMoveFlags(uint flag) { moveFlags |= flag; return this; }
    private PlayerMoveStatus DisableMoveFlags(uint flag) { 
        moveFlags = ((moveFlags & flag ) == flag) ? moveFlags^flag : moveFlags;
        return this; 
    }


    //===========该位表示角色是否在播放不可打断的动画=============
    public bool IsAnimationLocked() { return DetectMoveFlags(AnimationLocked); }
    public PlayerMoveStatus EnableAnimationLocked() { return EnableMoveFlags(AnimationLocked); }
    public PlayerMoveStatus DisableAnimationLocked() { return DisableMoveFlags(AnimationLocked); }

    //===========该位表示角色是否允许移动======================
    public bool IsEnableMove() { return DetectMoveFlags(Move); }
    public PlayerMoveStatus EnableMove() { return EnableMoveFlags(Move); }
    public PlayerMoveStatus DisableMove() { return DisableMoveFlags(Move); }

    //==========该位表示角色是否允许旋转=======================
    public bool IsEnableRoate() { return DetectMoveFlags(Rotate); }
    public PlayerMoveStatus EnableRotate() { return EnableMoveFlags(Rotate); }
    public PlayerMoveStatus DisableRotate() { return DisableMoveFlags(Rotate); }

    //==========该位表示角色是否允RootMotion=======================
    public bool IsEnableRootMotion() { return DetectMoveFlags(RootMotion); }
    public PlayerMoveStatus EnableRootMotion() { return EnableMoveFlags(RootMotion); }
    public PlayerMoveStatus DisableRootMotion() { return DisableMoveFlags(RootMotion);  }

    //==========角色是否在冲刺====================================
    public bool IsSprint() { return DetectMoveFlags(Sprint); }
    public PlayerMoveStatus SetSprint() { return EnableMoveFlags(Sprint); }
    public PlayerMoveStatus UnsetSprint() { return DisableMoveFlags(Sprint);  }
}
