using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    GameObject[] enemies;
    public bool earlySpawn = false;

    public float spawnDelay = 3f;
    public float spawnDelayVariance = 1f;
    public int maxSpawns = 1;

    private float realSpawnDelay;
    private float timer = 0;
    protected List<GameObject> spawns = new List<GameObject>();

    void Start() {
        if(earlySpawn)
            Spawn();
    }

    void Update() {
        if(spawns.Count >= maxSpawns)
            return;

        if(timer > spawnDelay) {
            realSpawnDelay = spawnDelay + Random.Range(-spawnDelayVariance, spawnDelayVariance);
            timer = 0;
            Spawn();
        } else {
            timer += Time.deltaTime;
        }
    }

    void Spawn() {
        var enemy = Instantiate(enemies[Random.Range(0, enemies.Length)]);
        enemy.transform.position = transform.position;
        enemy.SetActive(true);
        enemy.GetComponent<Player>().onDie.AddListener((foe) => {
            spawns.Remove(foe.gameObject);
        });

        spawns.Add(enemy);
    }
}
