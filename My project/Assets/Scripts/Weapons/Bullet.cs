using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gunner;

public class Bullet : MonoBehaviour
{
    // Vars:
    private int lifetimeTicks = 50;
    private int damage = 0;
    private float velocity = 0;

    public bool isActive = false;
    public Rigidbody2D rigidBody;
    public BulletPool bulletPool;

    private WEAPONTYPE currentType;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DecreaseLife();
    }

    // Interface:
    public void Activate(WEAPONTYPE type)
    {
        gameObject.SetActive(true);
        isActive = true;
        lifetimeTicks = 5;
        transform.parent = null;
        currentType = type;
        // Set stats:
        switch (type)
        {
            case WEAPONTYPE.BOW:
                velocity = 20f;
                damage = 5;
                lifetimeTicks = 50;
                break;
            case WEAPONTYPE.LEVERACTION:
                velocity = 40f;
                damage = 6;
                lifetimeTicks = 40;
                break;
            case WEAPONTYPE.ASSAULTRIFLE:
                velocity = 30f;
                damage = 3;
                lifetimeTicks = 30;
                break;
            case WEAPONTYPE.SHOTGUN:
                velocity = 30f;
                damage = 10;
                lifetimeTicks = 15;
                break;
            case WEAPONTYPE.AKIMBOSMG:
                velocity = 40f;
                damage = 8;
                lifetimeTicks = 20;
                break;
            case WEAPONTYPE.LASER:
                velocity = 100f;
                damage = 30;
                lifetimeTicks = 10;
                break;
        }
        transform.Find(type.ToString()).gameObject.SetActive(true);
        rigidBody.velocity = transform.right * velocity;
    }

    // Utils:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HPEntity hpEntity = collision.gameObject.GetComponent<HPEntity>();
        if (hpEntity != null )
        {
            hpEntity.TakeDamage(damage);
        }
    }
    private void DecreaseLife()
    {
        if (isActive)
        {
            lifetimeTicks--;
            if (lifetimeTicks <= 0)
            {
                transform.Find(currentType.ToString()).gameObject.SetActive(false);
                bulletPool.DeleteBullet(this);
            }
        }
    }
}
