using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject target;
    public float speed = 1.0f;
    public int scoreValue = 2;
    public int damage = 1;
    public int hp = 1;

    private Rigidbody2D rb2d;
    private Health health;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.health = hp;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            Vector3 targetDir = (target.transform.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, target.transform.position) > 1.0f)
                rb2d.position += new Vector2(targetDir.x, targetDir.y) * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            health.TakeDamage(collision.gameObject.GetComponent<Bullet>().bulletDamage);
            if (health.IsDead)
            {
                GameManager.Instance.AddScore(scoreValue);
                GameManager.Instance.enemies--;
                GameManager.Instance.UpdateHUD();

                Destroy(gameObject);
            }
        }
    }

}
