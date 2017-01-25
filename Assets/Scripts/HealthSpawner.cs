using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour {
    public GameObject health;
    public float spawnRate = .5f;

	void Start () {
        GetComponent<Player>().onDie.AddListener(Spawn);
	}

    void Spawn(Player p) {
        if(Random.Range(0f, 1f)< spawnRate) {
            var hi = Instantiate(health);
            hi.transform.position = transform.position;
        }
    }
}
