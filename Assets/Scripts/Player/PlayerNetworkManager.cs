using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkManager : NetworkBehaviour {
    
    //=============Player网络相关逻辑===============
    private NetworkVariable<Vector3> playerPosition = 
        new NetworkVariable<Vector3>(Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//同步玩家的位置


    private NetworkVariable<Quaternion> playerRotation = 
        new NetworkVariable<Quaternion>(Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//同步玩家的旋转

    private NetworkVariable<float> moveAmount = 
        new NetworkVariable<float>(0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//同步玩家的移动量

    private PlayerAnimatorManager playerAnimatorManager;//玩家动画管理器的引用

    private void Awake() {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }


    public Vector3 PlayerPosition
    {
        get { return playerPosition.Value; }
        set { playerPosition.Value = value; }
    }

    public Quaternion PlayerRotation
    {
        get { return playerRotation.Value; }
        set { playerRotation.Value = value; }
    }

    public float MoveAmount
    {
        get { return moveAmount.Value; }
        set { moveAmount.Value = value; }
    }

    [ServerRpc]
    public void PlayTargetAnimationServerRpc(string targetAnim){
        PlayTargetAnimationClientRpc(targetAnim);
    }

    [ClientRpc]
    public void PlayTargetAnimationClientRpc(string targetAnim){
        playerAnimatorManager.PlayTargetAnimation(targetAnim);
    }


}
