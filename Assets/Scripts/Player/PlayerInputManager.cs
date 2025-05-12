using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInputManager :  NetworkBehaviour{

    //=============Player输入相关逻辑===============
    private InputController inputController;
    private Vector2 playerMove;//玩家移动方向
    private Vector2 cameraMove;//摄像机移动方向
    private PlayerLocomotionManager playerLocomotionManager;//玩家控制器的引用

    private void Awake() {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        if(inputController == null){
            inputController = new InputController();
            inputController.PlayerMove.Move.performed += ctx => playerMove = ctx.ReadValue<Vector2>();
            inputController.CameraMove.Move.performed += ctx => cameraMove = ctx.ReadValue<Vector2>(); 
        }

        inputController.Enable();
    }

    private void Update() {
        if(IsOwner){
            playerLocomotionManager.Move(playerMove);
            PlayerCamera.Singleton.Rotate(cameraMove);
        }
    }
}
