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



}
