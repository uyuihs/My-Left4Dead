using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStatus : MonoBehaviour{   
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
        moveFlags = D3 | D2 | D1;
    }

    //============Player移动相关属性的标志位==================

    /// <summary>
    /// 用位运算表示使能动作，当为1时，表示enable,当为0时，表示disable
    /// 最高位为D31,最低位为D0
    /// 初始状态：未锁定animation
    /// </summary>
    // [Header("flags"), SerializeField]
    private uint moveFlags;//标志位
    private const uint D0 = 0x00000001;//播放不可以打断动画的Mask
    private const uint D1 = 0x00000002;//允许角色移动的Mask
    private const uint D2 = 0x00000004;//允许角色旋转的Mask
    private const uint D3 = 0x00000008;//允许角色使用RootMotion

    //===========检测、使能、使不能=============================
    private bool DetectMoveFlags(uint flag) { return (moveFlags & flag) == flag; }
    private PlayerMoveStatus EnableMoveFlags(uint flag) { moveFlags |= flag; return this; }
    private PlayerMoveStatus DisableMoveFlags(uint flag) { 
        moveFlags = ((moveFlags & flag ) == flag) ? moveFlags^flag : moveFlags;
        return this; 
    }


    //===========该位表示角色是否在播放不可打断的动画=============
    public bool IsAnimationLocked() { return DetectMoveFlags(D0); }
    public PlayerMoveStatus EnableAnimationLocked() { return EnableMoveFlags(D0); }
    public PlayerMoveStatus DisableAnimationLocked() { return DisableMoveFlags(D0); }

    //===========该位表示角色是否允许移动======================
    public bool IsEnableMove() { return DetectMoveFlags(D1); }
    public PlayerMoveStatus EnableMove() { return EnableMoveFlags(D1); }
    public PlayerMoveStatus DisableMove() { return DisableMoveFlags(D1); }

    //==========该位表示角色是否允许旋转=======================
    public bool IsEnableRoate() { return DetectMoveFlags(D2); }
    public PlayerMoveStatus EnableRotate() { return EnableMoveFlags(D2); }
    public PlayerMoveStatus DisableRotate() { return DisableMoveFlags(D2); }

    //==========该位表示角色是否允RootMotion=======================
    public bool IsEnableRootMotion() { return DetectMoveFlags(D3); }
    public PlayerMoveStatus EnableRootMotion() { return EnableMoveFlags(D3); }
    public PlayerMoveStatus DisableRootMotion() { return DisableMoveFlags(D3);  }
}
