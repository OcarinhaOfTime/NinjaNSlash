using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionEvent : UnityEvent<Collider2D> { }

public class Weapon : MonoBehaviour {
    public float knockBack = 10;
    public int damage = 10;

    public UnityEvent<Collider2D> onAttackEnter = new CollisionEvent();
    public UnityEvent<Collider2D> onAttackStay = new CollisionEvent();
    public UnityEvent<Collider2D> onAttackExit = new CollisionEvent();
    
    public void OnTriggerEnter2D(Collider2D collision) {
        onAttackEnter.Invoke(collision);
    }

    public void OnTriggerStay2D(Collider2D collision) {
        onAttackStay.Invoke(collision);
    }

    public void OnTriggerExit2D(Collider2D collision) {
        onAttackExit.Invoke(collision);
    }
}
