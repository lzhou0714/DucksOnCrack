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

    public void DisplayUI(Gunner.WEAPONTYPE weapon)
    {
        if (!gameObject.activeSelf)
            return;
        switch (weapon)
        {
            case Gunner.WEAPONTYPE.BOW:
                bowUI.SetActive(true);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Gunner.WEAPONTYPE.BOLTACTION:
                bowUI.SetActive(false);
                sniperUI.SetActive(true);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Gunner.WEAPONTYPE.AKIMBOSMG:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(true);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(false);
                break;
            case Gunner.WEAPONTYPE.ASSAULTRIFLE:
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(true);
                shotgunUI.SetActive(false);
                break;
            case Gunner.WEAPONTYPE.LASER:  // Supposed to be shotgun
                bowUI.SetActive(false);
                sniperUI.SetActive(false);
                dualSMGUI.SetActive(false);
                assaultUI.SetActive(false);
                shotgunUI.SetActive(true);
                break;
        }
    }
}
