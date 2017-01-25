using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    public GameManager gm;
    public GameObject boss;
    bool spawned = false;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(!spawned && collision.CompareTag("Player")) {
            StartCoroutine(SpawnBoss());
            spawned = true;
        }        
    }


    IEnumerator SpawnBoss() {
        float timer = 0;
        while(timer < 3) {
            timer += Time.deltaTime;
            gm.audioSource.volume = 1 - timer / 3f;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        gm.audioSource.Stop();
        gm.audioSource.clip = gm.bossTheme;
        gm.audioSource.volume = 1;
        gm.audioSource.Play();
        boss.SetActive(true);
        boss.GetComponent<Player>().onDie.AddListener(gm.ClearLevel);
    }
}
