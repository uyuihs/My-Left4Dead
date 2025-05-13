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
    private PlayerAnimatorManager playerAnimatorManager;
    private float moveAmount;

    private void Awake() {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>(); 
        
    }
    private void OnEnable(){
        if(inputController == null){
            inputController = new InputController();
            inputController.PlayerMove.Move.performed += ctx => playerMove = ctx.ReadValue<Vector2>();
            inputController.CameraMove.Move.performed += ctx => cameraMove = ctx.ReadValue<Vector2>(); 
        }

        inputController.Enable();
    }

    private void GetMoveAmount(){
        moveAmount = Mathf.Clamp01(Mathf.Abs(playerMove.x) + Mathf.Abs(playerMove.y));

        //计算moveAmount
        moveAmount = Mathf.Clamp01(Mathf.Abs(playerMove.x) + Mathf.Abs(playerMove.y));
        //moveAmount 只为0、0.5、1,表示静止不动，walk,run
        if(moveAmount <MagicNumber.Singleton.upperEps && moveAmount >MagicNumber.Singleton.lowerEps){
            moveAmount = 0.5f;
        }
        else if(moveAmount <MagicNumber.Singleton.lowerEps){
            moveAmount = 0f;
        }
        else{
            moveAmount = 1f;
        }
    }

    private void Update() {
        if(IsOwner){
            GetMoveAmount();
            playerLocomotionManager.Move(playerMove, moveAmount);
            PlayerCamera.Singleton.Rotate(cameraMove);
            playerAnimatorManager.Move(moveAmount);
        }
    }
}
