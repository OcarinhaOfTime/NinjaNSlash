using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public float shootForce = 1;
    public int damage = 10;
    public float knockBack = 1;
    [HideInInspector]
    public float dir = 1;
    public LayerMask destroyLayer;


    public void Start() {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(shootForce * dir, 0), ForceMode2D.Impulse);
    }



    public void OnTriggerEnter2D(Collider2D collision) {
        var p = collision.GetComponent<Player>();
        if(p != null) {
            p.TakeDamage(damage, knockBack * dir);
        }
            
        Destroy(gameObject);
    }
}
