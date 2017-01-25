using UnityEngine;
using System.Collections;

enum PatrolAIState {
    Patroling,
    Hunting
}

public class PatrolAI : PlayerController {
    public AudioClip spawnClip;
    public AudioClip attackClip;

    public float speed = 5f;
    public float attackRange = 1;
    public float attackDelay = .2f;
    public float attackDelayVariance = .1f;

    float realAttackDelay;
    float timer = 0;

    public Player target;
    public Transform leftBoundary;
    public Transform rightBoundary;

    PatrolAIState state = PatrolAIState.Patroling;

    bool playerInPatrolArea {
        get {
            try {
                return target.transform.position.x > leftBoundary.position.x && target.transform.position.x < rightBoundary.position.x && Mathf.Abs(target.transform.position.y - transform.position.y) < 2f;
            } catch {
                return false;
            }
        }
    }

    bool playerInAttackRange {
        get {
            return Mathf.Abs(target.transform.position.x - transform.position.x) < attackRange * transform.localScale.y;
        }
    }

    protected override void Start() {
        base.Start();
        player.audioSource.clip = spawnClip;
        player.audioSource.Play();
    }

    void Update() {
        if(player.dead)
            return;
        switch(state) {
            case PatrolAIState.Patroling:
                Patrol();
                break;
            case PatrolAIState.Hunting:
                Hunt();
                break;
        }
    }    

    void Patrol() {
        if(playerInPatrolArea) {
            state = PatrolAIState.Hunting;
        } else {
            if(player.facingLeft && transform.position.x < leftBoundary.position.x) {
                player.facingLeft = false;
            }
            else if(!player.facingLeft && transform.position.x > rightBoundary.position.x) {
                player.facingLeft = true;
            }

            player.h_axis = speed * player.facingSign;
        }
    }    

    void Hunt() {
        if(!playerInPatrolArea) {
            state = PatrolAIState.Patroling;
        } else if(playerInAttackRange) {
            Attack();
        } else {
            player.h_axis = speed * -Mathf.Sign(transform.position.x - target.transform.position.x);
        }
    }

    void Attack() {
        if(timer > attackDelay) {
            realAttackDelay = attackDelay + Random.Range(-attackDelayVariance, attackDelayVariance);
            timer = 0;
            anim.SetTrigger("attack");
            player.audioSource.clip = attackClip;
            player.audioSource.Play();
        } else {
            timer += Time.deltaTime;
        }
    }

    protected override void Attack(Player p) {
        p.TakeDamage(weapon.damage);
        p.GetComponent<Rigidbody2D>().AddForce(new Vector2(weapon.knockBack * player.facingSign, 0), ForceMode2D.Impulse);
    }
}
