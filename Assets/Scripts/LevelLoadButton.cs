using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelLoadButton : MonoBehaviour {
    public string levelName;

	void Start () {
        GetComponent<Button>().onClick.AddListener(() => LevelLoadManager.LoadLevel(levelName));
	}
}
