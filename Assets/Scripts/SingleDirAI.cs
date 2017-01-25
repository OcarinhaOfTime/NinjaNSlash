using UnityEngine;
using System.Collections;

public class SingleDirAI : PlayerController {
    [Range(-1, 1)]
    public float dir = 0;

    public float attackDelay = 3f;
    public float attackDelayVariance = .5f;

    float realAttackDelay;
    float timer = 0;

    protected override void Start() {
        base.Start();
        realAttackDelay = attackDelay + Random.Range(-attackDelayVariance, attackDelayVariance);
    }

    void Update() {
        if(timer > attackDelay) {
            realAttackDelay = attackDelay + Random.Range(-attackDelayVariance, attackDelayVariance);
            timer = 0;
            anim.SetTrigger("attack");
        } else {
            timer += Time.deltaTime;
        }

        player.h_axis = dir;
    }

    protected override void Attack(Player p) {
        p.TakeDamage(weapon.damage);
        p.GetComponent<Rigidbody2D>().AddForce(new Vector2(weapon.knockBack * player.facingSign, 0), ForceMode2D.Impulse);
    }
}
