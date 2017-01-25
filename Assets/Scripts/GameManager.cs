using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    static int points = 0;
    [SerializeField]Player player;
    [SerializeField]
    GameObject boss;
    [SerializeField]
    GameObject enemiesGO;
    public AudioClip bossTheme;
    [SerializeField]
    Text scoreTxt;

    Player[] enemies;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        scoreTxt.text = "Score: " + points;
        player.onDie.AddListener((p) =>  SceneManager.LoadScene("game_over"));
        enemies = enemiesGO.GetComponentsInChildren<Player>();
        audioSource = GetComponent<AudioSource>();

        foreach(var foe in enemies) {
            foe.onDie.AddListener((e) => {
                points++;
                scoreTxt.text = "Score: " + points;
            });
        }
    }    

    public void ClearLevel(Player p) {
        points += 10;
        scoreTxt.text = "Score: " + points;
        SceneManager.LoadScene("level2");
    }
}
