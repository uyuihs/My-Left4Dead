using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNumber : MonoBehaviour
{
    //=============单根模式===============
    public static MagicNumber Singleton;
    private void Awake() {
        if(Singleton == null){
            Singleton = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    //=============魔数相关属性===============
    public float upperEps = 0.5f; //浮点精度数误差上限，0.5能被IEEE754表示
    public float lowerEps = 0.05f;//浮点数精度误差下限
    public float zeroEps = (float)1e-10;//表示接近0的浮点数

    public float movespeed = 3f;//玩家移动速度,一秒移动2
    public float rotatespeed = 4f;//玩家旋转速度
    public float smoothTime = 0.2f;//平滑时间
}