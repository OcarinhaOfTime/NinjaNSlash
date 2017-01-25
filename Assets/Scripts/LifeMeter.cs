using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeMeter : MonoBehaviour {
    CanvasGroup cg;
    Slider slider;
    void Awake() {
        cg = GetComponent<CanvasGroup>();
        slider = GetComponent<Slider>();
    }

	public void UpdateValue(float v) {
        slider.value = v;
    }

    public void Show() {
        if(cg)
            cg.alpha = 1;
    }

    public void Hide() {
        if(cg)
            cg.alpha = 0;
    }
}
