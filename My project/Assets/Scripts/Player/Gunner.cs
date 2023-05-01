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
    Rigidbody2D gunRb;

    Transform cameraTrfm;

    // Vars:
    // HandleRotation:
    Vector2 mousePosition;
    Vector2 transformPosition;
    // Shoot:
    [SerializeField] float fireCooldown = 0.6f;
    float cd = 0;

    // angular velocity
    Quaternion lastRot;
    Vector3 angularVel;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        bulletPool = GetComponentInChildren<BulletPool>();
        tipTransform = transform.GetChild(0);
        gunRb = GetComponent<Rigidbody2D>();

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
            cd = fireCooldown;
            if (carRb)
            {
                vel = carRb.velocity;
            }

            // HENRY: here's the RPC call to shoot, pass in whatever you want to send to server here
            photonView.RPC("RPC_HandleShooting", RpcTarget.AllViaServer, new Vector2(tipTransform.position.x, tipTransform.position.y), transform.rotation, vel, angularVel, PhotonNetwork.ServerTimestamp);
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

        bulletPool.SpawnBullet(pos + delta, rot*deltaRot, 50f);
    }
}
