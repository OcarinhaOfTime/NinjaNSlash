using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OnDieEvent : UnityEvent<Player> { }

public class Player : MonoBehaviour {
    public float moveForce = 60;
    public float maxSpeed = 10;
    public int maxLife = 100;
    public float collisionRepelForce = 1;
    public UnityEvent onTakeDamage;
    public AudioClip dieClip;
    public int _life;

    Color original;    

    public UnityEvent<Player> onDie { get; private set; }

    public int life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
            StartCoroutine(UpdateHUDRoutine());
        }
    }

    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    public LayerMask whatIsGround;

    protected Rigidbody2D rb2d;
    protected Animator anim;
    protected SpriteRenderer sr;
    public LifeMeter lifeMeter;

    [HideInInspector]
    public AudioSource audioSource;

    public bool grounded { get; private set; }
    public bool dead { get; private set; }
    public float h_axis { get; set; }

    public bool facingLeft
    {
        get
        {
            return transform.localScale.x < 0;
        }
        set
        {
            float x = (value) ? Mathf.Abs(transform.localScale.x) * -1 : Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
    }

    public int facingSign { get { return (facingLeft) ? -1 : 1; } }

    IEnumerator UpdateHUDRoutine() {
        lifeMeter.Show();
        lifeMeter.UpdateValue(_life / (float)maxLife);
        sr.color = new Color(.1f, 0, 0);
        yield return new WaitForSeconds(.1f);
        sr.color = original;
        yield return new WaitForSeconds(3);
        lifeMeter.Hide();
    }

    void Awake() {
        onDie = new OnDieEvent();
        _life = maxLife;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if(lifeMeter == null)
            lifeMeter = GetComponentInChildren<LifeMeter>();
    }

    // Use this for initialization
    protected virtual void Start() {        
        if(!lifeMeter)
            lifeMeter = GetComponentInChildren<LifeMeter>();

        original = sr.color;
    }

    protected virtual void FixedUpdate() {
        if(dead)
            return;

        grounded = Physics2D.OverlapCircle(groundCheck.position, .2f, whatIsGround);
        anim.SetBool("grounded", grounded);

        if(rb2d.velocity.x < maxSpeed && h_axis > 0 || rb2d.velocity.x > -maxSpeed && h_axis < 0)
            rb2d.AddForce(new Vector2(h_axis * moveForce, 0));

        rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);

        if((facingLeft && h_axis > 0.01f) || (!facingLeft && h_axis < -0.01f))
            Flip();

        anim.SetFloat("speed", Mathf.Abs(h_axis));
        anim.SetFloat("v_speed", rb2d.velocity.y);
    }

    protected void Flip() {
        facingLeft = !facingLeft;
    }

    public virtual void TakeDamage(int amount, float knockBack) {
        rb2d.AddForce(new Vector2(knockBack, 0), ForceMode2D.Impulse);
        TakeDamage(amount);
        onTakeDamage.Invoke();
    }

    public virtual void TakeDamage(int amount) {
        if(dead)
            return;
        if(life <= amount) {
            life = 0;
            Die();
        } else {
            life -= amount;
        }
    }

    public void Die() {
        anim.SetTrigger("die");
        sr.color = Color.white;
        dead = true;
        StopAllCoroutines();
        rb2d.velocity = Vector2.zero;
        foreach(var col in GetComponents<Collider2D>())
            col.enabled = false;
        rb2d.isKinematic = true;
        StartCoroutine(DestroyRoutine());
    }

    protected virtual IEnumerator DestroyRoutine() {
        audioSource.clip = dieClip;
        audioSource.Play();
        yield return new WaitForSeconds(3);
        onDie.Invoke(this);
        Destroy(gameObject);
    }

    protected virtual void SelfDestruct() {
        onDie.Invoke(this);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy")) {
            Vector2 dir = (collision.collider.transform.position - transform.position).normalized;
            dir.x *= collisionRepelForce;
            collision.collider.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
        }
    }

    public void RecoverLife(int ammount) {
        life += ammount;
        life = Mathf.Min(maxLife, _life);
    }
}
