using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReloader : MonoBehaviour {

    public Transform target;
         
	void Update () {
        if(target.position.y < transform.position.y)
            SceneManager.LoadScene("game_over");
	}
}
