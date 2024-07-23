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

    public Enums.Lasers currentLaserEffect;

    int playerScoreRate, spawnScore, spawnScoreRate;
    public List<GameObject> enemyPrefabs = new();
    List<GameObject> effectPrefabs = new();
    GameObject colorAssistPrefab;
    List<GameObject> laserPrefabs = new();
    int[] effectCount = new int[Enum.GetNames(typeof(Enums.EnemyEffects)).Length - 1];

    public GameObject allEnemies;
    [SerializeField]
    GameObject mainCanvas;
    bool isGameInitialized = false, spawnSequenceCalled = false;

    string sceneName;
    GameObject gameStart;
    TextMeshProUGUI counter;
    GameObject pauseStuff, pauseButton;
    GameObject timeScaleButton;

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
        LoadPrefabs();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Main":
                if (!isGameInitialized)
                {
                    AssignMissingMain();
                    isGameInitialized = true;
                }
                StartCoroutine(CountdownSequence());
                break;
            case "LoseScreen":
                StopAllCoroutines();
                GameObject.Find("Final Score").GetComponent<TextMeshProUGUI>().text = 
                    "Final Score: " + PlayerScore;

                isGameInitialized = false;
                spawnSequenceCalled = false;
                break;
            case "Settings":
                AssignMissingSettings();
                break;
            case "Menu":
                StopAllCoroutines();
                Time.timeScale = 1;

                isGameInitialized = false;
                spawnSequenceCalled = false;
                break;
            default:
                break;
        }
    }

    private void AssignMissingMain()
    {
        healthText = GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        allEnemies = GameObject.Find("AllEnemies");
        gameStart = GameObject.Find("Game Start");
        counter = GameObject.Find("Counter").GetComponent<TextMeshProUGUI>();
        pauseStuff = GameObject.Find("Pause Stuff");
        pauseStuff.SetActive(false);
        pauseButton = GameObject.Find("PauseButton");
        timeScaleButton = GameObject.Find("TimeScale");
        mainCanvas = GameObject.Find("Canvas");

        PlayerScore = 0;
        playerScoreRate = 2;
        PlayerHealth = 100;
        spawnScore = 10;
        spawnScoreRate = 2;
        chillMultiplier = 1;
        evolvedEnemyCount = 0;
        generalSpeedMultiplier = 1;

        LoadPrefabs();
        gameStartTime = Time.time;

        Instantiate(laserPrefabs[(int)currentLaserEffect], Vector3.zero, Quaternion.identity);
    }

    private void LoadPrefabs()
    {
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
        laserPrefabs.Clear();
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Lasers)).Length; i++)
        {
            laserPrefabs.Add((GameObject)Resources.Load("Prefabs/Lasers/Laser" + ((Enums.Lasers)i).ToString()));
        }

        colorAssistPrefab = (GameObject)Resources.Load("Prefabs/Effects/ColorAssistText");
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
        else if (sceneName == "Settings")
        {
            colorAssistText.text = EnabledOrDisabled(colorAssistEnabled);
            lowDetailText.text = EnabledOrDisabled(lowDetailEnabled);
        }
    }

    IEnumerator CountdownSequence()
    {
        timeScaleButton.SetActive(false);
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
        timeScaleButton.SetActive(true);
        pauseButton.SetActive(true);
        timeScaleButton.GetComponent<TimeScaleButton>().ResetIndex();

        if (!spawnSequenceCalled)
        {
            StartCoroutine(SpawnSequence());
            StartCoroutine(ScoreIncrementSequence());

            spawnSequenceCalled = true;
        }
    }

    IEnumerator SpawnSequence()
    {            
        List<Enums.Enemies> readyEnemies = new();
        // if the game has gone on for 5 minutes, only spawn final bosses and generals
        if (Time.time - gameStartTime >= 300)
        {
            readyEnemies.Add(Enums.Enemies.FinalBoss);
            readyEnemies.Add(Enums.Enemies.FinalBoss);
            readyEnemies.Add(Enums.Enemies.FinalBoss);
            readyEnemies.Add(Enums.Enemies.FinalBoss);
            readyEnemies.Add(Enums.Enemies.General);
        }
        else
        {
            // check available units with the current spawn score
            for (int i = 0; i < Enum.GetNames(typeof(Enums.Enemies)).Length; i++)
            {
                Enemy enemyScript = enemyPrefabs[i].GetComponent<Enemy>();
                if (spawnScore >= enemyScript.spawnScore)
                {
                    // then check spawn time
                    if (Time.time - gameStartTime >= enemyScript.firstSpawnTime)
                    {
                        readyEnemies.Add((Enums.Enemies)i);
                        // if it is that enemy's era, add it again
                        if (enemyScript.preferedEra == currentEra)
                        {
                            readyEnemies.Add((Enums.Enemies)i);
                            // if the era is end, add it again
                            if (currentEra == Enums.Era.End)
                            {
                                readyEnemies.Add((Enums.Enemies)i);
                            }
                        }
                    }
                }
            }
        }

        if (readyEnemies.Count > 0)
        {
            // chance for the spawned enemy to have a random effect, only one at a time though
            // all effects have a 1 in 10 chance of happening except normal
            Enums.EnemyEffects effectToGive = Enums.EnemyEffects.Normal;
            int randomIndex = Rand.Range(0, readyEnemies.Count);
            Enums.Enemies enemyToSpawn = readyEnemies[randomIndex];            
            
            // final bosses will not have effects, so it just won't choose any effects
            if (enemyToSpawn != Enums.Enemies.FinalBoss)
            {
                bool didChooseEffect = false;
                for (int i = 0; i < effectCount.Length; i++)
                {
                    // if 10 enemies have spawned without a specific effect, the next effect to give is that one
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
                    // spawn it with that effect
                    SpawnEnemy(enemyToSpawn, effectToGive);
                }
                else
                {
                    // randomly decide the effect
                    // 1 in 10 separate odds for each one
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
                    if (didChooseEffect)
                    {
                        SpawnEnemy(enemyToSpawn, effectToGive);
                    }
                    else
                    {
                        SpawnEnemy(enemyToSpawn, Enums.EnemyEffects.Normal);
                    }
                }

                // increase the effect count of every effect that wasn't given
                // final boss spawns don't cause this to happen
                for (int i = 0; i < effectCount.Length; i++)
                {
                    if (i != (int)effectToGive)
                    {
                        effectCount[i]++;
                    }
                }
            }
            else
            {
                // final boss catch
                SpawnEnemy(enemyToSpawn, Enums.EnemyEffects.Normal);
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

        Vector3 spawnPos = new(randX, 0, 9);

        // assign the direction angle for the enemies to take
        // in Unity the degrees are a compass, 0 degrees is North, and rotation follows clockwise
        // for the variable ignore the outer 20 degrees and the center 70 degrees
        float angle = Rand.Range(1, 3) == 1 ? Rand.Range(110.0f, 145.0f) : Rand.Range(215.0f, 250.0f);

        GameObject newEnemy = Instantiate(enemyPrefabs[(int)enemyType]);
        newEnemy.transform.position = spawnPos;
        newEnemy.transform.parent = allEnemies.transform;
        newEnemy.GetComponent<Enemy>().Angle = angle;
        newEnemy.tag = "Enemy";

        spawnScore -= newEnemy.GetComponent<Enemy>().spawnScore;

        // account for the effect
        // final bosses don't spawn with effects
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
        playerScoreRate++;
        spawnScoreRate++;
        if (currentEra == Enums.Era.Late || currentEra == Enums.Era.End)
        {
            playerScoreRate++;
            spawnScoreRate++;
        }
        PlayerScore += playerScoreRate;
        StartCoroutine(ScoreIncrementSequence());
    }

    string EnabledOrDisabled(bool x)
    {
        return x ? "ENABLED" : "DISABLED";
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
        // note: the end of early game is 20 seconds, not the start
        // which is why the early enum holds a value of 20
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
        timeScaleButton.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseStuff.SetActive(false);
        counter.text = "3";
        gameStart.SetActive(true);
        StartCoroutine(CountdownSequence());
    }

    public void LoadSettingsScene()
    {
        StartCoroutine(LoadSceneAsync("Settings"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene);

        GameObject.Find("Back").SetActive(false);
        mainCanvas.SetActive(false);
    }

    public void UnloadSettingsScene()
    {
        StartCoroutine(UnloadSceneAsync("Settings"));
    }

    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        Scene main = SceneManager.GetSceneByName("Main");
        SceneManager.SetActiveScene(main);
        mainCanvas.SetActive(true);
    }
}