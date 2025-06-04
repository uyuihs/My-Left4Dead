using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
public class PlayerSetup : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsLocalPlayer)
        {
            PlayerCamera.Singleton.cinemachineTarget = GetComponentInChildren<PlayerCameraRoot>().transform;
            PlayerCamera.Singleton.InitCamera();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        PlayerCamera.Singleton.hasInited = false;
    }
}
