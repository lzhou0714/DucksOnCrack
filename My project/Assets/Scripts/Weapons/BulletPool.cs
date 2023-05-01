using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponTypes;

public class BulletPool : MonoBehaviour
{
    // Vars:
    // Refs:
    [SerializeField] GameObject bulletPrefab;
    private List<Bullet> bulletList = new List<Bullet>();

    [SerializeField] private Vehicle vehicle;


    private void OnEnable()
    {
        for (int i = 0; i < 10; i++)
        {
            InstantiateBullet();
        }
        //vehicle = transform.root.GetComponent<Vehicle>();
    }

    // Interface:
    public Bullet SpawnBullet(WEAPONTYPE type, Vector2 position, Quaternion rotation)
    {
        if (vehicle == null)
        {
            vehicle = transform.root.GetComponent<Vehicle>();
        }

        Bullet bullet = bulletList.Find(b => !b.isActive);
        // If pool empty:
        if (bullet == null)
        {
            bullet = InstantiateBullet();
        }
        // Activate and return:
        bullet.bulletPool = this;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.Activate(type);
        if (vehicle != null)
        {
            bullet.rigidBody.velocity += vehicle.rb.velocity;
        }
        return bullet;
    }
    public void DeleteBullet(Bullet bullet)
    {
        bullet.transform.parent = transform;
        bullet.gameObject.SetActive(false);
        bullet.isActive = false;
    }
    // Utils:
    private Bullet InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.SetActive(false);
        bullet.transform.parent = transform;
        bulletList.Add(bulletScript);
        return bulletScript;
    }
}
