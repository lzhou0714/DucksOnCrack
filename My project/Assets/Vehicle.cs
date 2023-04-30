using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] float acceleration, topSpeed, turnRate, turnSpd, turnRecalibration, boostPower;
    float currentSpeed, currentTurnRate;
    [SerializeField] Transform cameraTrfm;

    Transform trfm;
    Rigidbody2D rb;

    Vector2 up;

    void Start()
    {
        trfm = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cameraTrfm.position = trfm.position + Vector3.forward * -10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        up = trfm.up;

        Accelerate();
        Steer();

        
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
