using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypes : MonoBehaviour
{
    // Vars:
    public static WeaponTypes Instance;
    public WeaponData[] weaponData;
    // Structs:
    public struct WeaponData
    {
        public float overheatMax;
        public int overheatDecay;
        public int overheatAddition;
        public int bulletVelocity;
        public int bulletDamage;
        public int bulletLifetime;
        public int bulletAmount;
        public WeaponData(float overheatMax, int overheatDecay, int overheatAddition, int bulletVelocity, int bulletDamage, int bulletLifetime, int bulletAmount)
        {
            this.overheatMax = overheatMax;
            this.overheatDecay = overheatDecay;
            this.overheatAddition = overheatAddition;
            this.bulletVelocity = bulletVelocity;
            this.bulletDamage = bulletDamage;
            this.bulletLifetime = bulletLifetime;
            this.bulletAmount = bulletAmount;
        }
    }
    public enum WEAPONTYPE
    {
        NULL, BOW, LEVERACTION, BOLTACTION, ASSAULTRIFLE, SHOTGUN, AKIMBOSMG, LASER
    }


    // Interface:
    public WeaponData GetData(WEAPONTYPE type)
    {
        return weaponData[(int)type];
    }
    // Utils:
    private void Awake()
    {
        Instance = this;
        //
        SetupWeaponData();
    }
    private void SetupWeaponData()
    {
        weaponData = new WeaponData[8];
        weaponData[(int)WEAPONTYPE.NULL] = new WeaponData(0, 0, 0, 0, 0, 0, 0); 
        weaponData[(int)WEAPONTYPE.BOW] = new WeaponData(100, 2, 20, 20, 2, 50, 1); 
        weaponData[(int)WEAPONTYPE.LEVERACTION] = new WeaponData(100, 1, 30, 40, 6, 40, 1);
        weaponData[(int)WEAPONTYPE.BOLTACTION] = new WeaponData(100, 2, 60, 50, 5, 50, 1); 
        weaponData[(int)WEAPONTYPE.ASSAULTRIFLE] = new WeaponData(100, 2, 40, 30, 3, 30, 1); 
        weaponData[(int)WEAPONTYPE.SHOTGUN] = new WeaponData(100, 2, 80, 30, 3, 15, 5);
        weaponData[(int)WEAPONTYPE.AKIMBOSMG] = new WeaponData(100, 2, 40, 40, 8, 20, 2); 
        weaponData[(int)WEAPONTYPE.LASER] = new WeaponData(100, 1, 100, 100, 30, 15, 1);
    }
}
