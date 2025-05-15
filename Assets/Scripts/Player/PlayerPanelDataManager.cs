using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelDataManager : MonoBehaviour{
    public uint PanelDataPointToStaminaValue(uint enduranceValue){
        //根据耐力值设置耐力条的值
        uint staminaValue = enduranceValue * 10;
        return staminaValue;
    
    }
}
