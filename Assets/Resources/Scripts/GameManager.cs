using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int PlayerHealth { get; set; }
    [SerializeField] TextMeshProUGUI healthText;

    public int Score { get; set; }
    [SerializeField] TextMeshProUGUI scoreText;

    int spawnScore = 10, spawnScoreRate = 1;

    List<GameObject> enemyPrefabs = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        AssignMissing();
    }

    private void AssignMissing()
    {
        PlayerHealth = 100;
        healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();

        enemyPrefabs.Clear();
        enemyPrefabs.Add((GameObject)Resources.Load("Prefabs/Enemies/EnemyBasic"));
    }

    private void Update()
    {
        if (PlayerHealth <= 0)
        {
            Debug.Log("you lose lol");
        }
        healthText.text = PlayerHealth.ToString();
        scoreText.text = Score.ToString();
    }

    IEnumerator SpawnSequence()
    {
        // check available units with the current spawn score
        List<GameObject> readyEnemies = new();
        foreach (GameObject enemy in enemyPrefabs)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (spawnScore >= enemyScript.spawnScore)
            {
                readyEnemies.Add(enemy);
            }
        }

        // spawn it, take away spawn score equal to the amount it took to spawn
        GameObject enemyToSpawn = readyEnemies[Random.Range(0, readyEnemies.Count)];
        spawnScore -= enemyToSpawn.GetComponent<Enemy>().spawnScore;

        yield return new WaitForSeconds(1);
        // gain more spawn score
        StartCoroutine(SpawnSequence());
    }

    void SpawnEnemy(Enums.Enemies enemyType)
    {

    }

    IEnumerator ScoreIncrementSequence()
    {
        yield return new WaitForSeconds(5);
        Score += 2;
        StartCoroutine(ScoreIncrementSequence());
    }
}
