using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float defaultAcceleration, driftAcceleration;
    [SerializeField] float defaultTurnRate, driftTurnRate, turnSpd, turnRecalibration;
    [SerializeField] float boostPower, defaultDrag, driftDrag;
    float currentSpeed, activeTurnRate, currentAcceleration, currentTurnRate;
    Transform cameraTrfm;
    [SerializeField] TrailRenderer[] tireTrails;

    [SerializeField] Transform velocityBarTrfm;

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
        rb.drag = defaultDrag;
        currentAcceleration = defaultAcceleration;
        currentTurnRate = defaultTurnRate;
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            Boost();
        }

        //where you want to look/be - where you are
        //forward = target.position - trfm.position;
        //trfm.roattion += (forward - trfm.forward) * .1f;
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

            if (Input.GetKey(KeyCode.LeftShift))
            {
                EnterDrift();
            }
        }

        //velocityBarTrfm.localScale = new Vector3(rb.velocity.magnitude, .3f, 1);
        
    }

    void Accelerate()
    {
        //bool: drifting, braking
        //not brake -> still drift: entering drift via hand brake... travel at an angle

        if (driftLockTimer > 0)
        {

        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity -= up * currentAcceleration * .5f;
        }
        else
        {
            rb.velocity += up * currentAcceleration;
            currentSpeed = rb.velocity.magnitude;
        }
    }

    void Steer()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (activeTurnRate < turnSpd)
            {
                activeTurnRate += currentTurnRate;
                if (activeTurnRate > turnSpd) { activeTurnRate = turnSpd; }
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (activeTurnRate > -turnSpd)
            {
                activeTurnRate -= currentTurnRate;
                if (activeTurnRate > turnSpd) { activeTurnRate = turnSpd; }
            }
        }

        trfm.Rotate(Vector3.forward * activeTurnRate);

        if (activeTurnRate > 0)
        {
            activeTurnRate -= turnRecalibration;
            if (activeTurnRate < 0) { activeTurnRate = 0; }
        }
        if (activeTurnRate < 0)
        {
            activeTurnRate += turnRecalibration;
            if (activeTurnRate > 0) { activeTurnRate = 0; }
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
        if (driftLockTimer < 1)
        {
            drifting = true;
            tireTrails[0].emitting = true;
            tireTrails[1].emitting = true;

            rb.drag = driftDrag;
            currentAcceleration = driftAcceleration;
            currentTurnRate = driftTurnRate;
        }

        driftLockTimer = 20;
    }
    void ExitDrift()
    {
        drifting = false;
        tireTrails[0].emitting = false;
        tireTrails[1].emitting = false;

        rb.drag = defaultDrag;
        currentAcceleration = defaultAcceleration;
        currentTurnRate = defaultTurnRate;
    }
}
