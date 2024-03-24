using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabs;
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private Player player;

    private void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnEnemy();
        }
    }

    private IEnumerator Start()
    {
        float timer = 0f;

        while (true)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                }
            }

            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                timer = 0f;

                enemies[Random.Range(0, enemies.Count)].gameObject.SetActive(true);

                if (CheckAllEnemyActive())
                {
                    SpawnEnemy();
                }
            }

            yield return null;
        }
    }

    private Enemy SpawnEnemy()
    {
        GameObject instances = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], parent);

        instances.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].position;

        Enemy instanceEnemyComponent = instances.GetComponent<Enemy>();

        instanceEnemyComponent.target = player.transform;

        enemies.Add(instanceEnemyComponent);

        return instanceEnemyComponent;
    }

    private bool CheckAllEnemyActive()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
