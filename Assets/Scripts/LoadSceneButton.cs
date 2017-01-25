using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour {
    public int sceneIndex = 1;

	void Start () {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneIndex));
	}
}
