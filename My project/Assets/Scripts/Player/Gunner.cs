using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static WeaponTypes;

public class Gunner : MonoBehaviour
{
    // Refs:
    PhotonView photonView;
    BulletPool bulletPool;
    GameObject activeBullet;
    Transform tipTransform;
    Rigidbody2D carRb;
    Rigidbody2D gunRb;
    GunnerUI gunUI;

    Transform cameraTrfm;

    // Vars:
    // HandleRotation:
    Vector2 mousePosition;
    Vector2 transformPosition;
    // Shoot:
    [SerializeField] float fireCooldown = 0.6f;
    float cd = 0;

    //
    private Transform barrelTransform;
    [SerializeField] int selectedWeaponIndex = 0;
    public WEAPONTYPE[] availableWeapons;
    public int[] weaponOverheats;
    private WeaponData selectedWeaponData;

    // angular velocity
    Quaternion lastRot;
    Vector3 angularVel;

    // Start is called before the first frame update
    void Start()
    {
        barrelTransform = transform.GetChild(2);
        availableWeapons = new WEAPONTYPE[] { WEAPONTYPE.BOW, WEAPONTYPE.LEVERACTION, WEAPONTYPE.SHOTGUN, WEAPONTYPE.LASER };
        weaponOverheats = new int[] { 0, 0, 0, 0 };
        UpdateWeapon(0);

        //
        photonView = GetComponent<PhotonView>();
        bulletPool = GetComponentInChildren<BulletPool>();
        tipTransform = transform.GetChild(0);
        gunRb = GetComponent<Rigidbody2D>();
        gunUI = FindObjectOfType<GunnerUI>();
        cameraTrfm = CameraController.cameraTransform;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            HandleRotation();
            HandleShooting();
            HandleWeaponSelect();
            DecreaseWeaponOverheats();
        }
    }

    private void HandleRotation()
    {
        // Get positions:
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transformPosition = transform.position;
        // Get angle:
        float newAngle = Mathf.Atan2(mousePosition.y - transformPosition.y, mousePosition.x - transformPosition.x) * Mathf.Rad2Deg;
        float currentAngle = transform.rotation.eulerAngles.z;
        float offsetAngle = Mathf.DeltaAngle(currentAngle, newAngle);
        // Rotate gun:
        lastRot = transform.rotation;

        transform.rotation = Quaternion.Euler(Vector3.forward * (currentAngle + 0.3f * offsetAngle));

        float dt = Time.deltaTime;
        Quaternion q1 = lastRot;
        Quaternion q2 = transform.rotation;
        angularVel = (2 / dt) * new Vector3(q1[0] * q2[1] - q1[1] * q2[0] - q1[2] * q2[3] + q1[3] * q2[2],
        q1[0] * q2[2] + q1[1] * q2[3] - q1[2] * q2[0] - q1[3] * q2[1],
        q1[0] * q2[3] - q1[1] * q2[2] + q1[2] * q2[1] - q1[3] * q2[0]);
        angularVel = new Vector3(0, 0, angularVel.x);
    }
    private void HandleShooting()
    {
        if (!carRb)
        {
            if(transform.parent)
            {
                carRb = transform.parent.GetComponent<Rigidbody2D>();
            }
        }
        Vector2 vel = Vector2.zero;

        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }
        if (cd <= 0 && Input.GetKey(KeyCode.Mouse0))
        {
            if (CheckWeaponOverheat())
            {
                return;
            }
            cd = fireCooldown;
            if (carRb)
            {
                vel = carRb.velocity;
            }
            // HENRY: here's the RPC call to shoot, pass in whatever you want to send to server here
            if (selectedWeaponData.bulletAmount == 1)
            {
                photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, angularVel, PhotonNetwork.ServerTimestamp);
            }
            else
            {
                StartCoroutine(ShootMultiple(selectedWeaponData.bulletAmount, vel));
            }
            AddWeaponOverheat();
        }
    }
    private IEnumerator ShootMultiple(int bulletAmount, Vector2 vel)
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        if (bulletAmount == 5)
        {
            // Shotgun:
            for (int i = 0; i < 5; i++)
            {
                photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, angularVel, PhotonNetwork.ServerTimestamp);
                yield return delay;
            }
            yield break;
        }
        if (bulletAmount == 2)
        {
            photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, angularVel, PhotonNetwork.ServerTimestamp);
            yield return delay;
            yield return delay;
            photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, angularVel, PhotonNetwork.ServerTimestamp);
        }
    }
    private void HandleWeaponSelect()
    {
        if (Input.GetKey(KeyCode.Q)) {
            if (availableWeapons[0] != WEAPONTYPE.NULL)
            {
                UpdateWeapon(0);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (availableWeapons[1] != WEAPONTYPE.NULL)
            {
                UpdateWeapon(1);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (availableWeapons[2] != WEAPONTYPE.NULL)
            {
                UpdateWeapon(2);
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (availableWeapons[3] != WEAPONTYPE.NULL)
            {
                UpdateWeapon(3);
            }   
        }
    }
    private void UpdateWeapon(int index)
    {
        barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(false);
        WEAPONTYPE type = availableWeapons[index];
        selectedWeaponIndex = index;
        barrelTransform.Find(type.ToString()).gameObject.SetActive(true);
        selectedWeaponData = WeaponTypes.Instance.GetData(type);
    }
    private void AddWeaponOverheat()
    {
        weaponOverheats[selectedWeaponIndex] += selectedWeaponData.overheatAddition;
    }
    private bool CheckWeaponOverheat()
    {
        if (weaponOverheats[selectedWeaponIndex] + selectedWeaponData.overheatAddition > selectedWeaponData.overheatMax)
        {
            return true;
        }
        return false;
    }
    private void DecreaseWeaponOverheats()
    {
        if (GunnerUI.Instance != null)
        {
            GunnerUI.Instance.UpdateOverheatBar(weaponOverheats[selectedWeaponIndex] / selectedWeaponData.overheatMax);
        }
        for (int i = 0; i < 4; i++)
        {
            weaponOverheats[i] = Mathf.Max(weaponOverheats[i] - WeaponTypes.Instance.weaponData[(int)availableWeapons[i]].overheatDecay, 0);
        }
    }

    public void ShuffleWeapons(WEAPONTYPE weapon0 = WEAPONTYPE.NULL, WEAPONTYPE weapon1 = WEAPONTYPE.NULL, WEAPONTYPE weapon2 = WEAPONTYPE.NULL, WEAPONTYPE weapon3 = WEAPONTYPE.NULL)
    {
        if (photonView.IsMine)
        {
            WEAPONTYPE w0 = weapon0 == WEAPONTYPE.NULL ? availableWeapons[0] : weapon0;
            WEAPONTYPE w1 = weapon1 == WEAPONTYPE.NULL ? availableWeapons[1] : weapon1;
            WEAPONTYPE w2 = weapon2 == WEAPONTYPE.NULL ? availableWeapons[2] : weapon2;
            WEAPONTYPE w3 = weapon3 == WEAPONTYPE.NULL ? availableWeapons[3] : weapon3;
            photonView.RPC("RPC_AssignWeapons", RpcTarget.All, w0, w1, w2, w3);
        }
    }

    [PunRPC]
    public void RPC_HandleShooting(Vector2 pos, Quaternion rot, Vector2 origVel, Vector3 origAngularVel, int timeInMillis)
    {
        // HENRY: So we want the same type of prediction for rotation as we have for position.
        // Predict the angle of our gun barrel by calculating the angular velocity (or some other smart way)

        int deltaTimeInMillis = PhotonNetwork.ServerTimestamp - timeInMillis;

        //Debug.Log(origAngularVel);
        float angularVelMagnitude = origAngularVel.z;
        Vector2 angularVelCalibration = -transform.right * angularVelMagnitude * ((float)deltaTimeInMillis / 1000);
        Vector2 delta = origVel * ((float)deltaTimeInMillis / 1000) + angularVelCalibration;

        Quaternion deltaRot = Quaternion.Euler(origAngularVel * ((float)deltaTimeInMillis / 1000));

        bulletPool.SpawnBullet(availableWeapons[selectedWeaponIndex], pos + delta, rot*deltaRot);
    }

    [PunRPC]
    public void RPC_AssignWeapons(WEAPONTYPE weapon0, WEAPONTYPE weapon1, WEAPONTYPE weapon2, WEAPONTYPE weapon3)
    {
        availableWeapons[0] = weapon0;
        availableWeapons[1] = weapon1;
        availableWeapons[2] = weapon2;
        availableWeapons[3] = weapon3;
    }
}
