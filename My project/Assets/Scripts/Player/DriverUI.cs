using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriverUI : MonoBehaviour
{
    // Vars:
    [SerializeField] Image healthBar;
    [SerializeField] Image speedometer;
    // Refs:
    public static DriverUI Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void UpdateHealthBar(float healthPercent)
    {
        if (!gameObject.activeSelf)
            return;
        healthBar.fillAmount = healthPercent;
    }
    public void UpdateSpeedometer(float speed)
    {
        if (!gameObject.activeSelf)
            return;
        speedometer.fillAmount = speed / 25f;
    }
}
