using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Extreme
}

[Serializable]
public class SpawnerInfo
{
    public Difficulty difficulty;
    public GameObject[] spawnerPosition;

    public int startMaxSpawningCount;
    public int incrementMaxCount;

    public float spawnTime;
}

[Serializable]
public class EnemyInfo
{
    public string name;
    public Color color;
    public int hp;
    public int damage;
    public int points;
}

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance 
    {
        get { return instance; }
    }

    public static Difficulty difficulty = Difficulty.Medium;
    public static bool showTutorial = true;

    public GameObject playerPrefab;
    public GameObject spawnerPrefab;
    public GameObject enemyPrefab;
    public GameObject healthPickupPrefab;
    public Transform playerSpawnLocation;

    public SpawnerInfo[] spawerInfos;
    public EnemyInfo[] enemyInfos;

    [HideInInspector]
    public int enemies = 0;

    [HideInInspector]
    public bool waveStarted = false;

    private Player player;
    private int score = 0;
    public int Score
    {
        get { return score; }
    }

    private int currentWave = 1;
    public int CurrentWave
    {
        get { return currentWave; }
    }

    private float nextWaveTime = 4.0f;
    private float nextWaveTimer = 1.0f;

    private int numPickupToSpawn = 3;

    private List<Spawner> spawners;
    private List<GameObject> enemiesGO;
    public List<GameObject> EnemiesGOs
    {
        get { return enemiesGO; }
    }

    private bool paused = false;
    public bool Paused
    {
        get { return paused; }
    }

    private Transform menus;

    private void OnEnable()
    {
        instance = this;

        if(showTutorial)
        {
            ShowTutorial();
            Pause(true, false);
            showTutorial = false;
        }

        SpawnPlayer();
        AddScore(score);

        menus = FindObjectOfType<Canvas>().transform.Find("Menus");

        CreateSpawners(difficulty);
        CreateEnemiesGO();
    }

    private void Update()
    {
        if(enemies <= 0 && waveStarted == true)
        {
            nextWaveTimer -= Time.deltaTime;
            if (nextWaveTimer <= 0)
            {
                foreach (Spawner spawner in spawners)
                    spawner.NextWave();

                enemies = 0;
                waveStarted = false;
                currentWave++;

                nextWaveTimer = nextWaveTime;
                if(currentWave % 2 == 0)
                {
                    Transform healthPickups = GameObject.Find("HealthPickups").transform;
                    for (int i = 0; i < numPickupToSpawn; i++)
                    {
                        int index = UnityEngine.Random.Range(0, healthPickups.childCount);
                        Transform newPickupTransform = healthPickups.GetChild(index);
                        if (newPickupTransform.childCount == 0)
                        {
                            GameObject newHealthPickupGO = Instantiate(healthPickupPrefab, newPickupTransform.position, Quaternion.identity);
                            newHealthPickupGO.transform.SetParent(newPickupTransform);
                        }
                    }
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.Escape))
            Pause(!paused);

        //TODO: This should not be here this is only for testing
        if(Input.GetKeyDown(KeyCode.K))
        {
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in allEnemies)
            {
                AddScore(2);
                enemies--;
                UpdateHUD();

                Destroy(enemy.gameObject);
            }
        }
    }

    private void SpawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab, playerSpawnLocation.transform.position, Quaternion.identity);
        player = go.GetComponent<Player>();
        Camera.main.GetComponent<CameraFollow>().target = player.gameObject;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateHUD();
    }
        
    public void UpdateHUD()
    {
        Transform hud = FindObjectOfType<Canvas>().transform.Find("HUD/UpperStats");
        hud.Find("Score").GetComponent<Text>().text = "Score: " + score;

        hud.Find("Enemies Left").GetComponent<Text>().text = "Enemies Left: " + enemies;
        hud.Find("Current Wave").GetComponent<Text>().text = "Current Wave: " + currentWave;
    }

    private void CreateSpawners(Difficulty difficulty)
    {
        SpawnerInfo spawnerInfo = spawerInfos[(int)difficulty];
        GameObject spawnersGO = GameObject.Find("Spawners");
        spawners = new List<Spawner>();

        for (int i = 0; i < spawnerInfo.spawnerPosition.Length; i++)
        {
            GameObject go = Instantiate(spawnerPrefab, spawnerInfo.spawnerPosition[i].transform.position, Quaternion.identity);
            go.transform.SetParent(spawnersGO.transform);

            Spawner spawner = go.GetComponent<Spawner>();
            spawner.currentWave = 1;
            spawner.waveMaxSpawn = spawnerInfo.startMaxSpawningCount;
            spawner.waveIncSpawn = spawnerInfo.incrementMaxCount;
            spawners.Add(spawner);
        }
    }

    private void CreateEnemiesGO()
    {
        enemiesGO = new List<GameObject>();
        GameObject enemiesObject = new GameObject("Enemies");
        foreach(EnemyInfo enemyInfo in enemyInfos)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemyGO.name = enemyInfo.name;
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemy.damage = enemyInfo.damage;
            enemy.hp = enemyInfo.hp;
            enemy.scoreValue = enemyInfo.points;
            enemyGO.transform.Find("Graphics").GetComponent<SpriteRenderer>().color = enemyInfo.color;
            enemyGO.transform.SetParent(enemiesObject.transform);
            enemyGO.SetActive(false);
            enemiesGO.Add(enemyGO);
        }
    }

    public void Pause(bool pause, bool showPauseMenu = true)
    {
        paused = pause;
        Time.timeScale = paused ? 0 : 1;
        if(showPauseMenu)
            menus.Find("Paused").gameObject.SetActive(pause);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        MainMenuManager.selectMenu = false;
        SceneManager.LoadScene("_MAIN_MENU_");
    }

    public void PauseButtonPlay()
    {
        Pause(false);
    }

    public void PauseButtonStartNewGame()
    {
        Time.timeScale = 1;
        MainMenuManager.selectMenu = true;
        SceneManager.LoadScene("_MAIN_MENU_");
    }

    public void ShowTutorial()
    {
        Transform tutorial = FindObjectOfType<Canvas>().transform.Find("Tutorial");
        tutorial.gameObject.SetActive(true);
        tutorial.Find("Intro").gameObject.SetActive(true);
    }

}
