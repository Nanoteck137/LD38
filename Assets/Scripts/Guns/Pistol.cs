using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun {

    public Pistol(Player player)
        : base(player)
    {
        name = "Pistol";
        nextBulletTime = 0.005f;
        ammo = 6;
        maxAmmo = 6;
        bulletSpeed = 13.0f;
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
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
}
