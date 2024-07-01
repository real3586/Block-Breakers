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

    #region All Variables
    public int PlayerHealth { get; set; }
    [SerializeField] TextMeshProUGUI healthText;

    public int PlayerScore { get; set; }
    [SerializeField] TextMeshProUGUI scoreText;

    int spawnScore, spawnScoreRate;
    List<GameObject> enemyPrefabs = new();
    List<GameObject> effectPrefabs = new();
    GameObject colorAssistPrefab;
    int[] effectCount = new int[Enum.GetNames(typeof(Enums.EnemyEffects)).Length - 1];

    public GameObject allEnemies;

    string sceneName;
    GameObject gameStart;
    TextMeshProUGUI counter;
    GameObject pauseStuff, pauseButton;

    public float chillMultiplier = 1;
    public float detailMultiplier = 1;
    public float generalSpeedMultiplier = 1;

    public int evolvedEnemyCount = 0;

    float gameStartTime;
    Enums.Era currentEra;
    #endregion

    #region Settings Variables
    public bool colorAssistEnabled;
    TextMeshProUGUI colorAssistText;

    public bool lowDetailEnabled;
    TextMeshProUGUI lowDetailText;
    #endregion

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

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Main":
                AssignMissingMain();
                StartCoroutine(CountdownSequence(true));
                break;
            case "LoseScreen":
                StopAllCoroutines();
                GameObject.Find("Final Score").GetComponent<TextMeshProUGUI>().text = 
                    "Final Score: " + PlayerScore;
                break;
            case "Settings":
                AssignMissingSettings();
                break;
            case "Menu":
                StopAllCoroutines();
                break;
        }
    }
    private void AssignMissingMain()
    {
        PlayerScore = 0;
        PlayerHealth = 100;
        spawnScore = 10;
        spawnScoreRate = 1;
        chillMultiplier = 1;
        evolvedEnemyCount = 0;
        generalSpeedMultiplier = 1;
        healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        allEnemies = GameObject.Find("AllEnemies");
        gameStart = GameObject.Find("Game Start");
        counter = GameObject.Find("Counter").GetComponent<TextMeshProUGUI>();
        pauseStuff = GameObject.Find("Pause Stuff");
        pauseStuff.SetActive(false);
        pauseButton = GameObject.Find("PauseButton");

        enemyPrefabs.Clear();
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Enemies)).Length; i++)
        {
            enemyPrefabs.Add((GameObject)Resources.Load("Prefabs/Enemies/Enemy" + ((Enums.Enemies)i).ToString()));
        }

        effectPrefabs.Clear();
        for (int i = 0; i < effectCount.Length; i++)
        {
            effectPrefabs.Add((GameObject)Resources.Load("Prefabs/Effects/Effect" + ((Enums.EnemyEffects)i).ToString()));
        }

        colorAssistPrefab = (GameObject)Resources.Load("Prefabs/Effects/ColorAssistText");
        gameStartTime = Time.time;
    }

    private void AssignMissingSettings()
    {
        colorAssistText = GameObject.Find("Color Assist Text").GetComponent<TextMeshProUGUI>();

        lowDetailText = GameObject.Find("Low Detail Text").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (sceneName == "Main")
        {
            if (PlayerHealth <= 0)
            {
                SceneManager.LoadScene("LoseScreen");
            }
            healthText.text = PlayerHealth.ToString();
            scoreText.text = PlayerScore.ToString();

            detailMultiplier = lowDetailEnabled ? 0.5f : 1;
        }
        else if (sceneName == "Settings")
        {
            colorAssistText.text = EnabledOrDisabled(colorAssistEnabled);
            lowDetailText.text = EnabledOrDisabled(lowDetailEnabled);
        }

        currentEra = SetCurrentEra(Time.time - gameStartTime);
        if (GameObject.Find("EnemyGeneral(Clone)"))
        {
            generalSpeedMultiplier = 1.15f;
        }
        else
        {
            generalSpeedMultiplier = 1;
        }
    }

    IEnumerator CountdownSequence(bool isGameStart)
    {
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        counter.text = "3";
        yield return new WaitForSecondsRealtime(1);
        counter.text = "2";

        yield return new WaitForSecondsRealtime(1);
        counter.text = "1";

        yield return new WaitForSecondsRealtime(1);
        gameStart.SetActive(false);
        Time.timeScale = 1;
        pauseButton.SetActive(true);

        if (isGameStart)
        {
            StartCoroutine(SpawnSequence());
            StartCoroutine(ScoreIncrementSequence());
        }
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

        // check the spawn time and remove enemies that cannot be spawned yet
        List<GameObject> readyEnemiesFiltered = new();
        foreach (GameObject enemy in readyEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.firstSpawnTime <= Time.time - gameStartTime)
            {
                // if it is that enemy's era, add it twice
                if (enemyScript.PreferedEra == currentEra)
                {
                    readyEnemiesFiltered.Add(enemy);
                    // if the era is end, add it again
                    if (currentEra == Enums.Era.End)
                    {
                        readyEnemiesFiltered.Add(enemy);
                    }
                }
                readyEnemiesFiltered.Add(enemy);
            }
        }

        if (readyEnemiesFiltered.Count > 0)
        {
            // chance for the spawned enemy to have a random effect, only one at a time though
            // all effects have a 1 in 10 chance of happening except normal
            Enums.EnemyEffects effectToGive = Enums.EnemyEffects.Normal;
            bool didChooseEffect = false;
            for (int i = 0; i < effectCount.Length; i++)
            {
                // if 10 enemies have spawned without an effect, the next effect to give is that one
                if (effectCount[i] >= 10)
                {
                    effectToGive = (Enums.EnemyEffects)i;
                    didChooseEffect = true;

                    // reset the counter
                    effectCount[i] = 0;
                    break;
                }
            }

            if (didChooseEffect)
            {
                // spawn it, take away spawn score equal to the amount it took to spawn
                int randomIndex = Rand.Range(0, readyEnemiesFiltered.Count);
                GameObject enemyToSpawn = readyEnemiesFiltered[randomIndex];
                spawnScore -= enemyToSpawn.GetComponent<Enemy>().spawnScore;
                SpawnEnemy((Enums.Enemies)randomIndex, effectToGive);
            }
            else
            {
                // randomly decide the effect
                // 10% separate odds for each one
                for (int i = 0; i < effectCount.Length; i++)
                {
                    int randomInteger = Rand.Range(1, 11);
                    if (randomInteger == 1)
                    {
                        effectToGive = (Enums.EnemyEffects)i;
                        didChooseEffect = true;
                        break;
                    }
                }

                // give the effect
                // same code from above
                int randomIndex = Rand.Range(0, readyEnemiesFiltered.Count);
                GameObject enemyToSpawn = readyEnemiesFiltered[randomIndex];
                spawnScore -= enemyToSpawn.GetComponent<Enemy>().spawnScore;
                if (didChooseEffect)
                {
                    SpawnEnemy((Enums.Enemies)randomIndex, effectToGive);
                }
                else
                {
                    SpawnEnemy((Enums.Enemies)randomIndex, Enums.EnemyEffects.Normal);
                }
            }

            // increase the effect count of every effect that wasn't given
            for (int i = 0; i < effectCount.Length; i++)
            {
                if (i != (int)effectToGive)
                {
                    effectCount[i]++;
                }
            }
        }
        yield return new WaitForSeconds(1);

        // gain more spawn score
        spawnScore += spawnScoreRate;
        StartCoroutine(SpawnSequence());
    }

    void SpawnEnemy(Enums.Enemies enemyType, Enums.EnemyEffects effect)
    {
        float randX = Rand.Range(-2.0f, 2.0f);
        float randZ = Rand.Range(8.0f, 9.0f);

        Vector3 spawnPos = new(randX, 0, randZ);

        // assign the direction angle for the enemies to take
        // in Unity the degrees are a compass, 0 degrees is North, and rotation follows clockwise
        // for the variable ignore the outer 20 degrees and the center 60 degrees
        float angle = Rand.Range(1, 3) == 1 ? Rand.Range(110.0f, 150.0f) : Rand.Range(210.0f, 250.0f);

        GameObject newEnemy = Instantiate(enemyPrefabs[(int)enemyType]);
        newEnemy.transform.position = spawnPos;
        newEnemy.transform.parent = allEnemies.transform;
        newEnemy.GetComponent<Enemy>().Angle = angle;
        newEnemy.tag = "Enemy";

        // account for the effect
        if (effect != Enums.EnemyEffects.Normal)
        {
            GameObject newEffect = Instantiate(effectPrefabs[(int)effect]);
            newEffect.transform.parent = newEnemy.transform;
            newEffect.transform.localPosition = Vector3.up * 2;
            newEffect.transform.localScale = Vector3.one;
            newEffect.SetActive(true);

            if (effect == Enums.EnemyEffects.Explosion)
            {
                // destroys the death effect
                Destroy(newEnemy.transform.Find("DeathEffects").gameObject);

                // explosion is the new death effect
                newEnemy.GetComponent<Enemy>().deathEffect = newEffect;
                newEffect.GetComponent<EffectExplosion>().explosionDamage = newEnemy.GetComponent<Enemy>().maxHealth;
            }

            if (colorAssistEnabled)
            {
                GameObject newText = Instantiate(colorAssistPrefab, newEffect.transform, false);
                newText.GetComponent<TextMeshPro>().text = EffectTypeToString(effect);
                newText.GetComponent<ColorAssist>().parent = newEnemy;
            }
        }
    }

    IEnumerator ScoreIncrementSequence()
    {
        yield return new WaitForSeconds(6);
        PlayerScore += 2;
        spawnScoreRate++;
        if (currentEra == Enums.Era.Late || currentEra == Enums.Era.End)
        {
            spawnScoreRate++;
        }
        StartCoroutine(ScoreIncrementSequence());
    }

    string EnabledOrDisabled(bool x)
    {
        if (x)
        {
            return "ENABLED";
        }
        else
        {
            return "DISABLED";
        }
    }

    string EffectTypeToString(Enums.EnemyEffects effect)
    {
        return effect switch
        {
            Enums.EnemyEffects.Power => "P",
            Enums.EnemyEffects.Reload => "r",
            Enums.EnemyEffects.Explosion => "e",
            Enums.EnemyEffects.Chill => "c",
            Enums.EnemyEffects.FireRate => "f",
            _ => null,
        };
    }

    Enums.Era SetCurrentEra(float currentGameTime)
    {
        // note: the end of early game is 10 seconds, not the start
        // which is why the early enum holds a value of 10
        if (currentGameTime <= (int)Enums.Era.Early)
        {
            return Enums.Era.Early;
        }
        else if (currentGameTime <= (int)Enums.Era.Mid)
        {
            return Enums.Era.Mid;
        }
        else if (currentGameTime <= (int)Enums.Era.Late)
        {
            return Enums.Era.Late;
        }
        else
        {
            return Enums.Era.End;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseStuff.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseStuff.SetActive(false);
        counter.text = "3";
        gameStart.SetActive(true);
        StartCoroutine(CountdownSequence(false));
    }
}