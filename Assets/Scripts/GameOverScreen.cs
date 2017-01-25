using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {
    void Start() {
        StartCoroutine("GameOver");
    }

    IEnumerator GameOver() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
