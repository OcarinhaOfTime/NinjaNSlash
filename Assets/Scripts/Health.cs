using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public int ammount = 100;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().RecoverLife(ammount);
            Destroy(gameObject);
        }
    }

}
