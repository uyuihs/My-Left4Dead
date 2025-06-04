using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : NetworkBehaviour
{
    //=============playerCamera为单根模式，一个端只有一个===============
    public static PlayerCamera Singleton;

    //=============相机旋转相关逻辑===============
    private float horizontalRotation;//水平旋转量
    private float verticalRotation;//垂直旋转量
    private float minLimitRotation = -30f;//限制垂直旋转的范围
    private float maxLimitRotation = 70f;
    private float totalhorizontalRotation;//总水平旋转量
    private float totalVerticalRotation;//总垂直旋转量
    public Transform cinemachineTarget;//相机跟随的物体

    private CinemachineVirtualCamera playerFollowCamera;
    private CinemachineVirtualCamera playerAimCamera;
    public bool hasInited;
    public float rotateSmoothTime = 0.2f;


    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void InitCamera()
    {
        playerFollowCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        playerFollowCamera.Follow = cinemachineTarget;

        playerAimCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        playerAimCamera.gameObject.SetActive(false);
        if (playerAimCamera) Debug.Log("Find");
        playerAimCamera.Follow = cinemachineTarget;
        hasInited = true;

    }

    //获取相机移动输入
    public void Rotate(Vector2 rotate)
    {

        //从input中提取输入
        horizontalRotation = rotate.x;
        verticalRotation = rotate.y;

        //计算水平旋转量
        totalhorizontalRotation += horizontalRotation * MagicNumber.Singleton.rotatespeed * Time.deltaTime;

        //计算垂直旋转量，向下为正方向，且在（-30,30）范围之内
        totalVerticalRotation -= verticalRotation * MagicNumber.Singleton.rotatespeed * Time.deltaTime;
        totalVerticalRotation = Mathf.Clamp(totalVerticalRotation, minLimitRotation, maxLimitRotation);

    }

    public float GetPlayerRotation()
    {
        return totalhorizontalRotation;
    }


    public void CameraRoate()
    {
        if (hasInited)
        {
            Quaternion targetRotation = Quaternion.Euler(totalVerticalRotation, totalhorizontalRotation, 0);
            cinemachineTarget.rotation = Quaternion.Slerp(cinemachineTarget.rotation, targetRotation, 1);
        }
    }

    private void FixedUpdate()
    {
        CameraRoate();
    }
}
