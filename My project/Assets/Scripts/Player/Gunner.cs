using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    // Refs:
    PhotonView photonView;
    BulletPool bulletPool;
    GameObject activeBullet;
    Transform tipTransform;
    Rigidbody2D carRb;

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
    [SerializeField] int selectedWeaponIndex;
    public WEAPONTYPE[] availableWeapons;
    public int[] weaponOverheats;
    int overHeatTimer = 0;
    private Transform overheatBarMask;
    private Transform overheatBar;



    public enum WEAPONTYPE
    {
        NULL = 0, BOW = 5, LEVERACTION = 7, BOLTACTION = 10, ASSAULTRIFLE = 6, SHOTGUN = 10, AKIMBOSMG = 8, LASER = 20
    }

    // Start is called before the first frame update
    void Start()
    {
        barrelTransform = transform.GetChild(2);
        overheatBar = transform.GetChild(3);
        overheatBarMask = overheatBar.GetChild(0);
        availableWeapons = new WEAPONTYPE[] { WEAPONTYPE.BOW, WEAPONTYPE.LEVERACTION, WEAPONTYPE.SHOTGUN, WEAPONTYPE.LASER };
        weaponOverheats = new int[] { 0, 0, 0, 0 };
        selectedWeaponIndex = 0;
        barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(true);

        //
        photonView = GetComponent<PhotonView>();
        bulletPool = GetComponentInChildren<BulletPool>();
        tipTransform = transform.GetChild(0);

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
            overheatBar.rotation = Quaternion.identity;
            overheatBar.transform.position = (Vector2)transform.position + 2 * Vector2.down;
            overheatBarMask.transform.localScale = Vector2.right * Mathf.Lerp(0, 6, weaponOverheats[selectedWeaponIndex] / 20f) + Vector2.up * 2f;
            overHeatTimer++;
            if (overHeatTimer < 15)
            {
                return;
            }
            overHeatTimer = 0;
            for (int i = 0; i < 4; i++)
            {
                weaponOverheats[i] = Mathf.Max(weaponOverheats[i] - 1, 0);
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            cameraTrfm.position = transform.position + Vector3.forward * -10;
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
        transform.rotation = Quaternion.Euler(Vector3.forward * (currentAngle + 0.3f * offsetAngle));
    }
    private void HandleShooting()
    {
        if (!carRb)
            carRb = transform.parent.GetComponent<Rigidbody2D>();
        Vector2 vel = Vector2.zero;
        if (cd > 0)
        {
            cd -= Time.deltaTime;
        }
        if (cd <= 0 && Input.GetKey(KeyCode.Mouse0))
        {
            cd = fireCooldown;
            if (carRb)
            {
                vel = carRb.velocity;
            }
            if (weaponOverheats[selectedWeaponIndex] >= 20)
            {
                return;
            }
            photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, PhotonNetwork.ServerTimestamp);
            HandleWeaponOverheats();
        }
    }
    private void HandleWeaponSelect()
    {
        if (Input.GetKey(KeyCode.Q)) {
            if (availableWeapons[0] != WEAPONTYPE.NULL)
            {
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(false);
                selectedWeaponIndex = 0;
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(true);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (availableWeapons[1] != WEAPONTYPE.NULL)
            {
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(false);
                selectedWeaponIndex = 1;
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(true);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (availableWeapons[2] != WEAPONTYPE.NULL)
            {
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(false);
                selectedWeaponIndex = 2;
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(true);
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (availableWeapons[3] != WEAPONTYPE.NULL)
            {
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(false);
                selectedWeaponIndex = 3;
                barrelTransform.Find(availableWeapons[selectedWeaponIndex].ToString()).gameObject.SetActive(true);
            }   
        }
    }
    private void HandleWeaponOverheats()
    {
        weaponOverheats[selectedWeaponIndex] += (int)availableWeapons[selectedWeaponIndex];
    }

    [PunRPC]
    public void RPC_HandleShooting(Vector2 pos, Quaternion rot, Vector2 origVel, int timeInMillis)
    {
        int deltaTimeInMillis = PhotonNetwork.ServerTimestamp - timeInMillis;
        
        Vector2 delta = origVel * ((float)deltaTimeInMillis / 1000);

        bulletPool.SpawnBullet(availableWeapons[selectedWeaponIndex], pos + delta, rot);
    }
}
