using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour {
    [SerializeField]
    GameObject levelLoadPanel;

    public static LevelLoadManager instance;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void ReloadLevel() {
        instance.StartCoroutine(instance.LoadLevelRoutine(SceneManager.GetActiveScene().name));
    }

    public static void LoadLevel(string levelName) {
        instance.StartCoroutine(instance.LoadLevelRoutine(levelName));
    }

    IEnumerator LoadLevelRoutine(string levelName) {
        var req = SceneManager.LoadSceneAsync(levelName);
        levelLoadPanel.SetActive(true);
        yield return req;
        levelLoadPanel.SetActive(false);
    }
}
