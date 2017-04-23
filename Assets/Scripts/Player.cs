using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerGun
{
    Pistol,
    Rifle,
}

public class Player : MonoBehaviour {

    public float speed = 1.0f;
    public GameObject bulletPrefab;

    private Rigidbody2D rb2d;
    private Health health;

    private float nextHitTime = 0.1f;
    private float nextHitTimer = 0.0f;
    private bool canBeHit = true;

    private int maxHP = 20;

    private Gun[] guns;
    private PlayerGun currentGun = PlayerGun.Pistol;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.health = maxHP;
        UpdateHealthBar();

        guns = new Gun[2];
        guns[0] = new Pistol(this);
        guns[1] = new Rifle(this);

        UpdateCurrentWeapon();
    }

    private void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            UpdateMovement();

            guns[(int)currentGun].Update();

            UpdateHitTimer();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentGun = PlayerGun.Pistol;
                UpdateCurrentWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentGun = PlayerGun.Rifle;
                UpdateCurrentWeapon();
            }

        }
    }

    private void UpdateMovement()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb2d.position += inputDir * speed * Time.deltaTime;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.Find("Graphics").rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void UpdateHitTimer()
    {
        nextHitTimer -= Time.deltaTime;
        if (nextHitTimer <= 0)
        {
            canBeHit = true;
            nextHitTimer = nextHitTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!health.IsDead)
            {
                if (canBeHit)
                {
                    health.TakeDamage(collision.gameObject.GetComponent<Enemy>().damage);
                    UpdateHealthBar();
                    canBeHit = false;
                }
            }
            else
            {
                Transform gameOver = FindObjectOfType<Canvas>().transform.Find("Menus/GameOver");
                gameOver.gameObject.SetActive(true);
                gameOver.transform.Find("Points").GetComponent<Text>().text = "You got "+ GameManager.Instance.Score +" points";
                gameOver.transform.Find("Wave").GetComponent<Text>().text = "You made it to wave " + GameManager.Instance.CurrentWave;
                GameManager.Instance.Pause(true, false);

            }
        }
    }

    private void UpdateHealthBar()
    {
        health.health = Mathf.Clamp(health.health, 0, maxHP);
        Transform healthGO = FindObjectOfType<Canvas>().transform.Find("HUD/Health");
        healthGO.Find("ProgressBar").GetComponent<ProgressBar>().progress = (float)health.health / maxHP;
        healthGO.Find("Text").GetComponent<Text>().text = "HP: " + health.health;
    }

    private void UpdateCurrentWeapon()
    {
        FindObjectOfType<Canvas>().transform.Find("HUD/CurrentWeapon/Text").GetComponent<Text>().text = "Current Weapon: " + guns[(int)currentGun].Name;
    }

    public void GiveHealth(int amount)
    {
        health.GiveHealth(amount);
        UpdateHealthBar();
    }

}
