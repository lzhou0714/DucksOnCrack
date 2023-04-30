using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float acceleration, topSpeed, turnRate, turnSpd, turnRecalibration, boostPower;
    float currentSpeed, currentTurnRate;
    Transform cameraTrfm;

    PhotonView pv;

    Transform trfm;
    public Rigidbody2D rb;

    Vector2 up;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
        cameraTrfm = CameraController.cameraTransform;
    }

    private void Update()
    {
        // if(pv.IsMine)
        // {
        //     cameraTrfm.position = trfm.position + Vector3.forward * -10;
        // }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.T))
        {
            if(pv.IsMine)
            {
                up = trfm.up;

                Accelerate();
                Steer();
            }
        }
        
    }

    void Accelerate()
    {
        if (!Input.GetKey(KeyCode.S))
        {
            rb.velocity += up * acceleration;

            currentSpeed = rb.velocity.magnitude;
        }
        else
        {
            rb.velocity -= up * acceleration;
        }
    }

    void Steer()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (currentTurnRate < turnSpd)
            {
                currentTurnRate += turnRate;
                if (currentTurnRate > turnSpd) { currentTurnRate = turnSpd; }
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (currentTurnRate > -turnSpd)
            {
                currentTurnRate -= turnRate;
                if (currentTurnRate > turnSpd) { currentTurnRate = turnSpd; }
            }
        }

        trfm.Rotate(Vector3.forward * currentTurnRate);

        if (currentTurnRate > 0)
        {
            currentTurnRate -= turnRecalibration;
            if (currentTurnRate < 0) { currentTurnRate = 0; }
        }
        if (currentTurnRate < 0)
        {
            currentTurnRate += turnRecalibration;
            if (currentTurnRate > 0) { currentTurnRate = 0; }
        }
    }

    void Boost()
    {
        rb.velocity = up * boostPower;
    }
}
