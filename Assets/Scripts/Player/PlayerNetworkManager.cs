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

    private NetworkVariable<uint> endurance = 
        new NetworkVariable<uint>(10,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//设置耐久

    private NetworkVariable<uint> currentStamina = 
        new NetworkVariable<uint>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//设置耐力值

    private NetworkVariable<uint> maxStamina =
        new NetworkVariable<uint>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);//设置最大耐力值

    private PlayerAnimatorManager playerAnimatorManager;//玩家动画管理器的引用
    private PlayerUIManager playerUIManager;//玩家UIHUD管理器的引用
    private PlayerPanelDataManager playerPanelDataManager;//玩家面板数据管理器的引用

    private void Awake() {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerUIManager = GameObject.FindObjectOfType<PlayerUIManager>();
        playerPanelDataManager = GetComponent<PlayerPanelDataManager>();
    }


    public override void OnNetworkSpawn() {
        base.OnNetworkDespawn();
        if (IsOwner) {
            HandlePlayerStaminaSync();

        }
    }

    private void HandlePlayerStaminaSync(){
        currentStamina.OnValueChanged += playerUIManager.playerUIHUDManager.SetnewStamina;
        currentStamina.OnValueChanged += NetwrokValueDebug;

        maxStamina.Value = playerPanelDataManager.PanelDataPointToStaminaValue(endurance.Value);//设置耐力值
        currentStamina.Value = maxStamina.Value;
        playerUIManager.playerUIHUDManager.SetMaxStamina(maxStamina.Value);//设置耐力条的最大长度
    }

    public void NetwrokValueDebug(uint oldval, uint newval){
    //    if(newval < oldval) Debug.Log(currentStamina.Value);
    }

    public Vector3 PlayerPosition {
        get { return playerPosition.Value; }
        set { playerPosition.Value = value; }
    }

    public Quaternion PlayerRotation {
        get { return playerRotation.Value; }
        set { playerRotation.Value = value; }
    }

    public float MoveAmount {
        get { return moveAmount.Value; }
        set { moveAmount.Value = value; }
    }

    public uint CurrentStamina{
        get { return currentStamina.Value; }
    }

    public void DecStamina(uint value) {
        currentStamina.Value = value > currentStamina.Value ? 0 : currentStamina.Value - value;
    }

    public void IncStamina(uint value){
        uint result = currentStamina.Value + value;
        currentStamina.Value = result <= maxStamina.Value ? result : maxStamina.Value;
    }

    public uint Endurance{
        get { return endurance.Value; }
        set { endurance.Value = value; }
    }


    public uint MaxStamina{
        get { return maxStamina.Value; }
        set { maxStamina.Value = value; }
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
