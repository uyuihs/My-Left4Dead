using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour {
    private StatusBarUI staminaBar;

    private void Awake() {
        staminaBar = GameObject.Find("StaminaBar").GetComponent<StatusBarUI>();
    }

    public void SetnewStamina(uint oldValue, uint newValue){
        staminaBar.SetState(newValue);
    }
    public void SetMaxStamina(uint maxValue){
        staminaBar.SetMaxState(maxValue);
    }


}
