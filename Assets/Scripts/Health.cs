using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health = 3;

    private bool isDead = false;
    public bool IsDead {
        get { return isDead; }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    public void GiveHealth(int amount)
    {
        health += amount;
    }
}
