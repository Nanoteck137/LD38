using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int bulletDamage = 1;
    public float destroyTime;

    private float timer;

    private void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            timer += Time.deltaTime;
            if (timer >= destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

}
