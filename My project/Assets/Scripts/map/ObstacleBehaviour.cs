using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ObstacleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        Vector2 velocity = col.GetComponent<Rigidbody2D>().velocity;

        int damage = (int) (velocity.magnitude*5);
        col.GetComponent<HPEntity>().TakeDamage(damage);
        Debug.Log(damage);
    }
}
