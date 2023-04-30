using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int lifetimeTicks = 5;
    public bool isActive = false;
    public Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HPEntity hpEntity = collision.gameObject.GetComponent<HPEntity>();
        if (hpEntity != null )
        {
            hpEntity.TakeDamage(2);
        }
    }
}
