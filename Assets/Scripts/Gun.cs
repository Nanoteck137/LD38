using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun {

    protected string name = string.Empty;
    public string Name
    {
        get { return name; }
    }

    protected float nextBulletTime = 0.08f;
    protected int ammo = 20;
    protected int maxAmmo = 20;
    protected float bulletSpeed = 10.0f;

    protected Player player;

    protected float bulletSpawnTimer = 0.0f;

    protected Gun(Player player)
    {
        this.player = player;
    }

    public virtual void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            if (bulletSpawnTimer <= 0)
            {
                SpawnBullet();
                bulletSpawnTimer = nextBulletTime;
            }
            else
            {
                bulletSpawnTimer -= Time.deltaTime;
            }
        }
    }

    protected virtual void SpawnBullet()
    {
        GameObject bullet = GameObject.Instantiate(player.bulletPrefab, player.transform.Find("Graphics/BulletSpawnPoint").position, Quaternion.identity);
        bullet.transform.SetParent(player.transform);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition3 = (mousePosition - player.transform.position).normalized;
        Vector2 targetPosition = new Vector2(targetPosition3.x, targetPosition3.y);

        bullet.GetComponent<Rigidbody2D>().velocity = targetPosition * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

}
