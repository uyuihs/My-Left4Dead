using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    //=============playerCamera为单根模式，一个端只有一个===============
    public static PlayerCamera Singleton;

    //=============相机旋转相关逻辑===============
    private float horizontalRotation;//水平旋转量
    private float verticalRotation;//垂直旋转量
    private float limitRotation = 30f;//限制垂直旋转的范围
    private float totalhorizontalRotation;//总水平旋转量
    private float totalVerticalRotation;//总垂直旋转量
    [SerializeField] private Transform playerCamera;//负责左右旋转
    [SerializeField] private Transform cameraPivot;//负责上下旋转
    private Quaternion targetRotation;//相机旋转的四元数
    
    //=============相机跟随相关逻辑================
    private Vector3 playerPosition = Vector3.zero;//玩家坐标
    private Vector3 followVelocity = Vector3.zero;//跟随速度

    //=============相机碰撞相关逻辑================
    private float defaultCameraPosition;//相机距离玩家的默认位置
    private float targetCameraPosition;//碰撞检测后，相机应该距离玩家的位置
    [SerializeField] private LayerMask mask;//碰撞检测的layer
    private MagicNumber magicNumber = new MagicNumber();

    private void Awake() {
        if(Singleton == null){
            Singleton = this;
        }
        else{
            Destroy(gameObject);
        }
    }


    //获取相机移动输入
    public void Rotate(Vector2 rotate){

        //从input中提取输入
        horizontalRotation = rotate.x;
        verticalRotation = rotate.y;
    
        //计算水平旋转量
        totalhorizontalRotation += horizontalRotation * magicNumber.rotatespeed * Time.deltaTime;

        //计算垂直旋转量，向下为正方向，且在（-30,30）范围之内
        totalVerticalRotation -= verticalRotation * magicNumber.rotatespeed * Time.deltaTime;
        totalVerticalRotation = Mathf.Clamp(totalVerticalRotation, -limitRotation, limitRotation);

    }

    public void SetPosition(Vector3 _position){
        playerPosition = _position;
    }


    private void CameraRoate(){
        //处理相机的水平旋转，绕y轴旋转
        Vector3 rotationNormal = new Vector3(0, totalhorizontalRotation, 0);//相机旋转的法向量
        //平滑旋转
        targetRotation = Quaternion.Euler(rotationNormal);
        playerCamera.transform.rotation = targetRotation;

        //处理相机的垂直旋转，绕x轴旋转
        rotationNormal = new Vector3(totalVerticalRotation, 0, 0);
        targetRotation = Quaternion.Euler(rotationNormal);
        cameraPivot.localRotation = targetRotation;
    }

    private void FollowPlayer(){
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref followVelocity,magicNumber.smoothTime * Time.deltaTime);
    }

    private void CameraCollider(){
        //与相机碰撞的碰撞体
        RaycastHit hit;

        //

    }

    private void LateUpdate() {
        //相机跟随玩家
        //相机绕玩家旋转
        //相机碰撞，避免穿模
        FollowPlayer();
        CameraRoate();
    }

}
