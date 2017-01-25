using UnityEngine;
using System.Collections;

public class DestroyerTrigger : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("destroyied");
        Destroy(collision.gameObject);
    }
}
