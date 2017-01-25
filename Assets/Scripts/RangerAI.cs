using UnityEngine;
using System.Collections;

enum RangerAIState {
    Patroling,
    HuntingRanged,
    HuntingClose
}

public class RangerAI : PlayerController {
    public float speed = 5f;
    public float attackRange = 1;
    public float attackDelay = .2f;
    public float attackDelayVariance = .1f;
    public Projectile bullet;

    float realAttackDelay;
    float timer = 0;

    public Player target;
    public Transform leftBoundary;
    public Transform rightBoundary;

    RangerAIState state = RangerAIState.Patroling;

    bool playerInPatrolArea {
        get {
            try {
                return target.transform.position.x > leftBoundary.position.x && target.transform.position.x < rightBoundary.position.x && Mathf.Abs(target.transform.position.y - transform.position.y) < .5f;
            } catch {
                return false;
            }
        }
    }

    bool playerInAttackRange {
        get {
            return Mathf.Abs(target.transform.position.x - transform.position.x) < attackRange;
        }
    }

    protected override void Start() {
        base.Start();        
    }

    void Update() {
        if(player.dead)
            return;
        switch(state) {
            case RangerAIState.Patroling:
                Patrol();
                break;
            case RangerAIState.HuntingClose:
                Hunt();
                break;
            case RangerAIState.HuntingRanged:
                Hunt();
                break;
        }
    }

    void Patrol() {
        if(playerInPatrolArea) {
            state = RangerAIState.HuntingRanged;
        } else {
            if(player.facingLeft && transform.position.x < leftBoundary.position.x) {
                player.facingLeft = false;
            } else if(!player.facingLeft && transform.position.x > rightBoundary.position.x) {
                player.facingLeft = true;
            }

            player.h_axis = speed * player.facingSign;
        }
    }

    void Hunt() {
        player.h_axis = 0;
        if(!playerInPatrolArea) {
            state = RangerAIState.Patroling;
        } else {
            player.facingLeft = (transform.position.x - target.transform.position.x) > 0;
            Attack();
        }
    }

    void Shoot() {
        var inst = Instantiate(bullet.gameObject);
        inst.transform.position = bullet.transform.position;
        inst.GetComponent<Projectile>().dir = player.facingSign;
        inst.SetActive(true);
        player.audioSource.Play();
    }

    void Attack() {
        if(timer > attackDelay) {
            realAttackDelay = attackDelay + Random.Range(-attackDelayVariance, attackDelayVariance);
            timer = 0;
            anim.SetTrigger("shoot");
        } else {
            timer += Time.deltaTime;
        }
    }

    protected override void Attack(Player p) {
        p.TakeDamage(weapon.damage);
        p.GetComponent<Rigidbody2D>().AddForce(new Vector2(weapon.knockBack * player.facingSign, 0), ForceMode2D.Impulse);
    }
}
