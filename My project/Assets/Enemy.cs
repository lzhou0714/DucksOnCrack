using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] Transform targetTrfm;
    [SerializeField] float acceleration, turnRate, trackingRange, attackingRange;
    [SerializeField] GameObject bullet;

    int firingTimer;

    Vector2 vect2;
    protected bool every2;

    protected new void Start()
    {
        base.Start();

        targetPositions = new Vector3[5];
        turnRate *= Random.Range(.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (calculateTimer > 0) { calculateTimer--; }
        else
        {
            calculateTimer = 10;
            CalculatePredictedPos();
        }

        if (every2) { EveryTwo(); }
        every2 = !every2;
    }

    void EveryTwo()
    {
        
    }

    protected void ChaseTarget()
    {
        TurnTowardsTarget(Vector2.Distance(targetTrfm.position, trfm.position) / rb.velocity.magnitude);
        vect2 = trfm.up;
        rb.velocity += vect2 * acceleration;
    }

    protected bool TargetInRange(float range)
    {
        return
        (
            Mathf.Abs(targetTrfm.position.x - trfm.position.x) < range
            && Mathf.Abs(targetTrfm.position.y - trfm.position.y) < range
            && Vector2.Distance(targetTrfm.position, trfm.position) < range
        );
    }

    protected void TurnTowardsTarget(float predictionTime = 0)
    {
        trfm.up += ((GetPredictedPos(predictionTime) - trfm.position) - trfm.up) * turnRate;
    }

    [SerializeField] Vector3[] targetPositions;
    Vector3 predictedOffset;
    int addPos, calculateTimer;
    public Vector3 GetPredictedPos(float seconds)
    {
        int newestPos = addPos - 3;
        if (newestPos < 0) { newestPos += 5; }
        predictedOffset = (targetTrfm.position - targetPositions[addPos]) * seconds * .5f;
        predictedOffset += (targetTrfm.position - targetPositions[newestPos]) * 4 * seconds * .5f;
        return predictedOffset + targetTrfm.position;
    }
    private void CalculatePredictedPos()
    {
        targetPositions[addPos] = targetTrfm.position;
        addPos++;
        if (addPos > 4) { addPos = 0; }
    }
}
