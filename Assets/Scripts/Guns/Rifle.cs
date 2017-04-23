using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun {

    public Rifle(Player player)
        : base(player)
    {
        name = "Rifle";
        nextBulletTime = 0.08f;
        ammo = 30;
        maxAmmo = 30;
        bulletSpeed = 13.0f;
    }

}
