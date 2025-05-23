using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoadSolt : MonoBehaviour
{
    public GameObject currentWeaponModel;

    private void UnloadAndDestoryWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModle(WeaponItem waepon)
    {
        UnloadAndDestoryWeapon();
        if (waepon == null) { return; }

        GameObject weaponModel = Instantiate(waepon.itemModle, transform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
        currentWeaponModel = weaponModel;
    }
}
