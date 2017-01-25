using UnityEngine;
using System.Collections;

public abstract class PlayerController : MonoBehaviour {
    protected Player player;
    protected Animator anim;
    protected Rigidbody2D rb2d;
    protected Weapon weapon;

    // Use this for initialization
    protected virtual void Start () {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>();
        weapon.onAttackStay.AddListener((col) => {
            var p = col.GetComponent<Player>();

            if(col.gameObject == gameObject || p == null)
                return;

            Attack(p);
        });
    }

    protected abstract void Attack(Player p);
}
