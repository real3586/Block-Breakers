using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HandbookManager : MonoBehaviour
{
    public static HandbookManager Instance;

    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [SerializeField] TextMeshProUGUI enemyHealthText, enemySpeedText, 
        enemyFirstSpawnText, enemyDefeatText, enemyExtraText;

    GameObject currentEnemy;
    public int currentIndex = 0;

    [SerializeField] float rotationSpeed;

    string[] effects = new string[]
    {
        "None",
        "None",
        "None",
        "None",
        "Moves faster below half health",
        "None",
        "Spawns two mini-clones on death",
        "Takes 5 less damage when hit",
        "Hardened + Tank effects",
        "Gets stronger the more that have spawned",
        "Makes other enemies faster"
    };

    private void Awake()
    {
        ChangeEnemy();
        Instance = this;
    }

    private void Update()
    {
        if (currentIndex <= 0)
        {
            leftButton.gameObject.SetActive(false);
        }
        else
        {
            leftButton.gameObject.SetActive(true);
        }

        if (currentIndex >= GameManager.Instance.enemyPrefabs.Count - 1)
        {
            rightButton.gameObject.SetActive(false);
        }
        else
        {
            rightButton.gameObject.SetActive(true);
        }

        Quaternion continuousRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        currentEnemy.transform.rotation *= continuousRotation;
    }

    public void ChangeEnemy()
    {
        Destroy(currentEnemy); 
        currentEnemy = Instantiate(GameManager.Instance.enemyPrefabs[currentIndex], Vector3.zero, Quaternion.identity);
        currentEnemy.transform.localScale = Vector3.one * currentEnemy.transform.localScale.x;

        Enemy enemyScript = currentEnemy.GetComponent<Enemy>();
        enemyText.text = "Enemy: " + enemyScript.type.ToString();
        enemyHealthText.text = "Health: " + enemyScript.health;
        enemySpeedText.text = "Speed: " + enemyScript.speed;
        enemyFirstSpawnText.text = "First Spawn: " + enemyScript.firstSpawnTime;
        enemyDefeatText.text = "Defeat Score: " + enemyScript.defeatScore;
        enemyExtraText.text = "Effect: " + effects[currentIndex];
    }
}
