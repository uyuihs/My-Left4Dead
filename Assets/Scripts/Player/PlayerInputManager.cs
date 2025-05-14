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
    private bool isSprinting = false;
    private bool isWalk = false;

    //=============Debug输入相关===============
    private Vector3 debugToTheWall;

    //============翻滚相关输入===============
    private bool rollInput = false;


    private void Awake() {
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>(); 
        
    }
    private void OnEnable(){
        if(inputController == null){
            inputController = new InputController();
            inputController.PlayerMove.Move.performed += ctx => playerMove = ctx.ReadValue<Vector2>();
            inputController.CameraMove.Move.performed += ctx => cameraMove = ctx.ReadValue<Vector2>(); 
            inputController.PlayerMove.Sprinting.performed += ctx => isSprinting = true;
            inputController.PlayerMove.Sprinting.canceled += ctx => isSprinting = false;
            inputController.PlayerMove.Walk.performed += ctx => isWalk = true;
            inputController.PlayerMove.Walk.canceled += ctx => isWalk = false;
            inputController.PlayerMove.Dodge.performed += ctx => rollInput = true;

            //Debug输入
            DebugInput();
        }

        inputController.Enable();
    }

    private void DebugInput(){//Debug输入
        inputController.PlayerDebug.ToTheWall.performed += ctx =>{
            debugToTheWall = new Vector3(6.8f, 11f, -9f);
            playerLocomotionManager.transform.position = debugToTheWall;
        };
    }

    private void GetMoveAmount(){
        //计算moveAmount
        moveAmount = Mathf.Abs(playerMove.x) > MagicNumber.Singleton.zeroEps ? 
            MagicNumber.Singleton.upperEps: Mathf.Abs(playerMove.y) > MagicNumber.Singleton.zeroEps?
            MagicNumber.Singleton.upperEps: MagicNumber.Singleton.zeroEps;//若有一个输入，则为0.5
        //moveAmount 只为0、1、2、4,表示静止不动，walk,run
        if(moveAmount <= MagicNumber.Singleton.upperEps &&
            moveAmount >MagicNumber.Singleton.lowerEps && isWalk){
            moveAmount = 0.5f;
        }
        else if(moveAmount < MagicNumber.Singleton.lowerEps){
            moveAmount = MagicNumber.Singleton.zeroEps;
        }
        else {//若没按下ctrl,则是跑步
            moveAmount = 1f;
        }
        if(moveAmount > MagicNumber.Singleton.zeroEps && isSprinting){//若按下shift,则为冲刺
            moveAmount = 2f;
        }
    }

    private void HandleDodgeInput(){
        if(rollInput){
            rollInput = false;
            playerLocomotionManager.AtemptedPerformDodge();
        }
    }

    private void Update() {
        if(IsOwner){
            GetMoveAmount();
            playerLocomotionManager.Move(playerMove, moveAmount);
            PlayerCamera.Singleton.Rotate(cameraMove);
            playerAnimatorManager.Move(moveAmount);
            HandleDodgeInput();
        }
    }
}
