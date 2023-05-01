using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponTypes;

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
    private WeaponData currentData;

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
        currentData = WeaponTypes.Instance.GetData(type);
        velocity = currentData.bulletVelocity;
        damage = currentData.bulletDamage;
        lifetimeTicks = currentData.bulletLifetime;
        //
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
