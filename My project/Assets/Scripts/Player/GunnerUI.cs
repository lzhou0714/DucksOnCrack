using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunnerUI : MonoBehaviour
{
    [SerializeField] GameObject bowUI;
    [SerializeField] GameObject sniperUI;
    [SerializeField] GameObject dualSMGUI;
    [SerializeField] GameObject assaultUI;
    [SerializeField] GameObject shotgunUI;

    [SerializeField] Image overheatBar;

    public static GunnerUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    public enum Weapon
    {
        Bow,
        Sniper,
        DualSMG,
        Assault,
        Shotgun,
    }

    public void DisplayUI(WeaponTypes.WEAPONTYPE weapon)
    {
        if (!gameObject.activeSelf)
            return;
        switch (weapon)
        {
            case WeaponTypes.WEAPONTYPE.BOW:
                bowUI.SetActive(true);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case WeaponTypes.WEAPONTYPE.BOLTACTION:
                bowUI.SetActive(false);
                sniperUI.SetActive(true);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case WeaponTypes.WEAPONTYPE.AKIMBOSMG:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(true);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case WeaponTypes.WEAPONTYPE.ASSAULTRIFLE:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(true);
                shotgunUI.SetActive(false);
                break;
            case WeaponTypes.WEAPONTYPE.SHOTGUN:  // Supposed to be shotgun
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(true);
                break;
        }
    }

    public void UpdateOverheatBar(float fillAmount)
    {
        if (!gameObject.activeSelf)
            return;
        overheatBar.fillAmount = fillAmount;
    }
}
