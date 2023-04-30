using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Vars:
    WaitForSeconds delay = new WaitForSeconds(0.2f);
    // Refs:
    [SerializeField] GameObject bulletPrefab;
    private List<Bullet> bulletList = new List<Bullet>();

    private void OnEnable()
    {
        for (int i = 0; i < 10; i++)
        {
            InstantiateBullet();
        }
        StartCoroutine(SubtractLifetime());
    }

    // Interface:
    public Bullet SpawnBullet(Vector2 position, Quaternion rotation, float velocity)
    {
        Bullet bullet = bulletList.Find(b => !b.isActive);
        // If pool empty:
        if (bullet == null)
        {
            bullet = InstantiateBullet();
        }
        // Activate and return:
        bullet.gameObject.SetActive(true);
        bullet.isActive = true;
        bullet.lifetimeTicks = 5;
        bullet.transform.parent = null;
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.rigidBody.velocity = bullet.transform.right * velocity;
        return bullet;
    }
    public void DeleteBullet(Bullet bullet)
    {
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
    private IEnumerator SubtractLifetime()
    {
        while (true)
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.lifetimeTicks--;
                if (bullet.lifetimeTicks <= 0)
                {
                    DeleteBullet(bullet);
                }
            }
            yield return delay;
        }
    }
}
