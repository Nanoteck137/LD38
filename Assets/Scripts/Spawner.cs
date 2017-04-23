using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float spawnTime = 2.0f;

    public int currentWave = 1;
    public int waveMaxSpawn = 1;
    public int waveIncSpawn = 1;

    private float spawnMinPosition = -2;
    private float spawnMaxPosition = 2;

    private float spawnTimer = 0.0f;

    private int enemiesCount = 0;

    private void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            if (spawnTimer <= 0)
            {
                GameManager.Instance.waveStarted = true;

                if (enemiesCount < waveMaxSpawn)
                {
                    float x = Random.Range(transform.position.x + spawnMinPosition - 0.5f, transform.position.x + spawnMaxPosition + 0.5f);
                    float y = Random.Range(transform.position.y + spawnMinPosition - 0.5f, transform.position.y + spawnMaxPosition + 0.5f);
                    int enemyID = Random.Range(0, GameManager.Instance.EnemiesGOs.Count);

                    GameObject go = Instantiate(GameManager.Instance.EnemiesGOs[enemyID], new Vector3(x, y, 0.0f), Quaternion.identity);
                    go.SetActive(true);
                    go.transform.SetParent(transform);

                    enemiesCount++;
                    GameManager.Instance.enemies++;
                    GameManager.Instance.UpdateHUD();

                    spawnTimer = spawnTime;
                }
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }

    public void NextWave()
    {
        currentWave++;
        spawnTimer = spawnTime;
        waveMaxSpawn += waveIncSpawn;
        enemiesCount = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 minPosition = new Vector3(this.transform.position.x + spawnMinPosition, this.transform.position.y + spawnMinPosition);
        Vector3 maxPosition = new Vector3(this.transform.position.x + spawnMaxPosition, this.transform.position.y + spawnMaxPosition);

        Color color;
        ColorUtility.TryParseHtmlString("#00CED1", out color);
        Gizmos.color = color;

        Gizmos.DrawLine(new Vector3(minPosition.x, minPosition.y), new Vector3(minPosition.x, maxPosition.y));
        Gizmos.DrawLine(new Vector3(minPosition.x, maxPosition.y), new Vector3(maxPosition.x, maxPosition.y));
        Gizmos.DrawLine(new Vector3(maxPosition.x, maxPosition.y), new Vector3(maxPosition.x, minPosition.y));
        Gizmos.DrawLine(new Vector3(maxPosition.x, minPosition.y), new Vector3(minPosition.x, minPosition.y));
    }

}
