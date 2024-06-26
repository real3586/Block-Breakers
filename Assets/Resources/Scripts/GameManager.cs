using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rand = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int PlayerHealth { get; set; }
    [SerializeField] TextMeshProUGUI healthText;

    public int Score { get; set; }
    [SerializeField] TextMeshProUGUI scoreText;

    int spawnScore = 10, spawnScoreRate = 1;
    List<GameObject> enemyPrefabs = new();

    GameObject allEnemies;

    string sceneName;
    GameObject gameStart;
    TextMeshProUGUI counter;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Main":
                AssignMissing();
                StartCoroutine(GameStart());
                break;
        }
    }
    private void AssignMissing()
    {
        PlayerHealth = 100;
        healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        allEnemies = GameObject.Find("AllEnemies");
        gameStart = GameObject.Find("Game Start");
        counter = GameObject.Find("Counter").GetComponent<TextMeshProUGUI>();

        enemyPrefabs.Clear();
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Enemies)).Length; i++)
        {
            enemyPrefabs.Add((GameObject)Resources.Load("Prefabs/Enemy" + ((Enums.Enemies)i).ToString()));
        }
    }

    private void Update()
    {
        if (sceneName == "Main")
        {
            if (PlayerHealth <= 0)
            {
                Debug.Log("you lose lol");
            }
            healthText.text = PlayerHealth.ToString();
            scoreText.text = Score.ToString();
        }
    }

    IEnumerator GameStart()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        counter.text = "2";

        yield return new WaitForSecondsRealtime(1);
        counter.text = "1";

        yield return new WaitForSecondsRealtime(1);
        gameStart.SetActive(false);
        Time.timeScale = 1;

        StartCoroutine(SpawnSequence());
        StartCoroutine(ScoreIncrementSequence());
    }

    IEnumerator SpawnSequence()
    {
        // check available units with the current spawn score
        List<GameObject> readyEnemies = new();
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Enemies)).Length; i++)
        {
            Enemy enemyScript = enemyPrefabs[i].GetComponent<Enemy>();
            if (spawnScore >= enemyScript.spawnScore)
            {
                readyEnemies.Add(enemyPrefabs[i]);
            }
        }

        // spawn it, take away spawn score equal to the amount it took to spawn
        if (readyEnemies.Count > 0)
        {
            int randomIndex = Rand.Range(0, readyEnemies.Count);
            GameObject enemyToSpawn = readyEnemies[randomIndex];
            spawnScore -= enemyToSpawn.GetComponent<Enemy>().spawnScore;
            SpawnEnemy((Enums.Enemies)randomIndex);
        }

        yield return new WaitForSeconds(1);

        // gain more spawn score
        spawnScore += spawnScoreRate;
        StartCoroutine(SpawnSequence());
    }

    void SpawnEnemy(Enums.Enemies enemyType)
    {
        float randX = Rand.Range(-2.0f, 2.0f);
        float randZ = Rand.Range(8.0f, 9.0f);

        Vector3 spawnPos = new(randX, 0, randZ);

        // use the unit circle, quadrants 3 and 4, ignores the extreme 30 degrees
        // in Unity, 0 degrees is North, and rotation follows clockwise (like a compass)
        float angle = Rand.Range(120.0f, 240.0f);

        GameObject newEnemy = Instantiate(enemyPrefabs[(int)enemyType]);
        newEnemy.transform.position = spawnPos;
        newEnemy.transform.parent = allEnemies.transform;
        newEnemy.GetComponent<Enemy>().Angle = angle;
        newEnemy.tag = "Enemy";
    }

    IEnumerator ScoreIncrementSequence()
    {
        yield return new WaitForSeconds(5);
        Score += 2;
        spawnScoreRate++;
        StartCoroutine(ScoreIncrementSequence());
    }
}