using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float acceleration, topSpeed, turnRate, turnSpd, turnRecalibration, boostPower, defaultDrag, driftDrag;
    float currentSpeed, currentTurnRate;
    Transform cameraTrfm;
    [SerializeField] TrailRenderer[] tireTrails;

    PhotonView pv;

    Transform trfm;
    public Rigidbody2D rb;

    Vector2 up;

    [SerializeField] bool singlePlayerOverride;
    bool driving, drifting;

    void Start()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
        cameraTrfm = CameraController.cameraTransform;
        if (!singlePlayerOverride) { pv = GetComponent<PhotonView>(); }
    }

    private void Update()
    {
        // if(pv.IsMine)
        // {
        //     cameraTrfm.position = trfm.position + Vector3.forward * -10;
        // }

        if (Input.GetKeyDown(KeyCode.T))
        {
            driving = !driving;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EnterDrift();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (driving)
        {
            if(singlePlayerOverride || pv.IsMine)
            {
                up = trfm.up;

                Accelerate();
                Steer();
                HandleDrifting();
            }

        }
        
    }

    void Accelerate()
    {
        if (driftLockTimer > 0)
        {

        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= up * acceleration;
        }
        else
        {
            rb.velocity += up * acceleration;
            currentSpeed = rb.velocity.magnitude;
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

    void HandleDrifting()
    {
        if (driftLockTimer > 0)
        {
            driftLockTimer--;
        }
        else if (drifting && !Input.GetKey(KeyCode.LeftShift))
        {
            if (Vector2.Angle(rb.velocity, up) < 45)
            {
                ExitDrift();
            }
        }
    }

    int driftLockTimer;
    void EnterDrift()
    {
        drifting = true;
        driftLockTimer = 20;
        tireTrails[0].emitting = true;
        tireTrails[1].emitting = true;

        rb.drag = driftDrag;
    }
    void ExitDrift()
    {
        drifting = false;
        tireTrails[0].emitting = false;
        tireTrails[1].emitting = false;

        rb.drag = defaultDrag;
    }
}
