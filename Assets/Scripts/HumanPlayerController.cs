using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HumanPlayerController : PlayerController {
    public float attackDelay = .5f;
    public float jumpForce = 18;
    public float glideForce = 19f;
    public float dashForce = 10;

    public int kunais = 5;
    public Text kunaisText;

    float timer;

    [SerializeField]
    Projectile projectile;

    [SerializeField]
    AudioClip swordSwing;

    AudioSource audioSource;

    protected override void Start() {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        //kunaisText.text = "x" + kunais;
    }

    protected override void Attack(Player p) {
        p.TakeDamage(weapon.damage);
        p.GetComponent<Rigidbody2D>().AddForce(new Vector2(weapon.knockBack * player.facingSign, 0), ForceMode2D.Impulse);
    }

    void Update() {
        if(timer < attackDelay) {
            timer += Time.deltaTime;
        } else {
            AttackInput();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && player.grounded) {
            anim.SetBool("glide", false);
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        } else if(Input.GetKey(KeyCode.UpArrow) && !player.grounded) {
            anim.SetBool("glide", true);
            if(rb2d.velocity.y < 0.1f) {
                rb2d.AddForce(new Vector2(0, glideForce));
            }
        } else {
            anim.SetBool("glide", false);
        }
                 

        player.h_axis = Input.GetAxis("Horizontal");
    }

    void AttackInput() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(player.grounded)
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);

            anim.SetTrigger("attack");
            timer = 0;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) && kunais > 0) {
            anim.SetTrigger("shoot");
            timer = 0;
            kunais--;
            kunaisText.text = "x" + kunais;
        }

        if(Input.GetKeyDown(KeyCode.C) && player.grounded) {
            Dash();
            timer = 0;
        }
    }

    void PlaySwordSound() {
        audioSource.clip = swordSwing;
        audioSource.Play();
    }

    void Throw() {
        var inst = Instantiate(projectile.gameObject);
        inst.transform.position = projectile.transform.position;
        inst.GetComponent<Projectile>().dir = player.facingSign;
        inst.SetActive(true);
    }

    void Dash() {
        anim.SetTrigger("dash");
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(-player.facingSign * dashForce, 0), ForceMode2D.Impulse);
    }
}
