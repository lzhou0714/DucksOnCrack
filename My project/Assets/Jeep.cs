using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeep : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (every2) { EveryTwo(); }
    }

    void EveryTwo()
    {
        ChaseTarget();
    }
}
