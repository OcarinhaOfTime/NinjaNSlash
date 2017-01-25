using UnityEngine;

public class CameraFollower : MonoBehaviour {
    public Transform player;
    public Transform leftBoundary;
    public Transform rightBoundary;

    [Range(0, .4f)]
    public float deadZone = .2f;
    [Range(0, .4f)]
    public float verticalDeadZone = .2f;
    public float playerSight = 2;

    float lb;
    float rb;
    float worldDeadZone;
    float worldVerticalDeadZone;
    float height;
    Rigidbody2D rb2d;
    // Use this for initialization
    void Start () {
        var cam = GetComponent<Camera>();
        rb2d = GetComponent<Rigidbody2D>();

        float width = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, 0)).x;
        worldDeadZone = width * deadZone;
        height = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y;
        worldVerticalDeadZone = height * verticalDeadZone;
        lb = leftBoundary.position.x + width;
        rb = rightBoundary.position.x - width;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float _playerSight = (player.lossyScale.x > 0) ? playerSight : -playerSight;
        var delta = player.position + Vector3.right * _playerSight - transform.position;

        float x_vel = (Mathf.Abs(delta.x) > worldDeadZone) ? delta.x : 0;
        float y_vel = (Mathf.Abs(delta.y) > worldDeadZone) ? delta.y : 0;

        rb2d.velocity = new Vector2(x_vel, y_vel);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, lb, rb), Mathf.Clamp(transform.position.y, 0, 11), transform.position.z);
    }
}
