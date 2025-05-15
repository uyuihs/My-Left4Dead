using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour {//控制silderbar的值

    //sliderbar 的引用
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public virtual void SetState(uint value) {
        slider.value = value;
    }

    public virtual void SetMaxState(uint max) {
        slider.maxValue = max;
        slider.value = max; 
    }
}
