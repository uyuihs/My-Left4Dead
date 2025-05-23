using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerEquipmentManager : MonoBehaviour
{
    [Header("Current Equipment")]
    public WeaponItem weapon;
    private WeaponLoadSolt weaponLoadSolt;
    private PlayerAnimatorManager playerAnimatorManager;
    private TwoBoneIKConstraint leftHandIK;
    private TwoBoneIKConstraint rightHandIK;
    private RigBuilder rigBuilder;

    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        rigBuilder = GetComponent<RigBuilder>();
        LoadWeaponLoaderSlots();

    }

    private void Start()
    {
        EquipCurrentWeapn();
    }

    private void LoadWeaponLoaderSlots()
    {
        weaponLoadSolt = GetComponentInChildren<WeaponLoadSolt>();
    }

    private void LoadCurrentWeapon()
    {
        weaponLoadSolt.LoadWeaponModle(weapon);
        playerAnimatorManager.SetAnimator(weapon.weaponAnimator);
    }

    private void LoadCurrentWeaponIK()
    {
        leftHandIK = GameObject.Find("WeaponHandIKRigLayer/LeftHandIK").GetComponent<TwoBoneIKConstraint>();
        rightHandIK = GameObject.Find("WeaponHandIKRigLayer/RightHandIK").GetComponent<TwoBoneIKConstraint>();
        TargetIKLeft leftIKTarget = GetComponentInChildren<TargetIKLeft>();
        TargetIKRight rightIKTarget =  GetComponentInChildren<TargetIKRight>();
        if (leftIKTarget)
        {
            leftHandIK.data.target = leftIKTarget.transform;
        }
        if (rightIKTarget)
        {
            rightHandIK.data.target = rightIKTarget.transform;
        }
        rigBuilder.Build();
    }

    private void EquipCurrentWeapn()
    {
        //加载武器，实例化
        //加载IK
        LoadCurrentWeapon();
        LoadCurrentWeaponIK();
    }
}
