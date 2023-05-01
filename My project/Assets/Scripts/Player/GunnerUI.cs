using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerUI : MonoBehaviour
{
    [SerializeField] GameObject bowUI;
    [SerializeField] GameObject sniperUI;
    [SerializeField] GameObject dualSMGUI;
    [SerializeField] GameObject assaultUI;
    [SerializeField] GameObject shotgunUI;

    public enum Weapon
    {
        Bow,
        Sniper,
        DualSMG,
        Assault,
        Shotgun,
    }

    public void DisplayUI(Weapon weapon)
    {
        if (!gameObject.activeSelf)
            return;
        switch (weapon)
        {
            case Weapon.Bow:
                bowUI.SetActive(true);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Weapon.Sniper:
                bowUI.SetActive(false);
                sniperUI.SetActive(true);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Weapon.DualSMG:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(true);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Weapon.Assault:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(true);
                shotgunUI.SetActive(false);
                break;
            case Weapon.Shotgun:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(true);
                break;
        }
    }
}
