using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
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
            // HENRY: here's the RPC call to shoot, pass in whatever you want to send to server here
            photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, PhotonNetwork.ServerTimestamp);
        }
    }

    [PunRPC]
    public void RPC_HandleShooting(Vector2 pos, Quaternion rot, Vector2 origVel, int timeInMillis)
    {
        // HENRY: So we want the same type of prediction for rotation as we have for position.
        // Predict the angle of our gun barrel by calculating the angular velocity (or some other smart way)

        int deltaTimeInMillis = PhotonNetwork.ServerTimestamp - timeInMillis;
        
        Vector2 delta = origVel * ((float)deltaTimeInMillis / 1000);

        bulletPool.SpawnBullet(pos + delta, rot, 50f);
    }
}
