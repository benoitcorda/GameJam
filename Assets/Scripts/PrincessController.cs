using UnityEngine;

public class PrincessController : MonoBehaviour
{
    enum States
    {
        Up,
        Down,
        Left,
        Right,
        StillUp,
        StillDown,
        StillLeft,
        StillRight }
    ;

    public float maxSpeed;

    public GameObject needle;
    public GameObject princess;
    private SpriteRenderer spriteRenderer;          // Reference to the player's animator component.
    private Sprite[] sprites;

    private Animator anim;          // Reference to the player's animator component.
    private Collider coll;
    private States state;
    private string playerNumber;
    private float[] thresholds;

    private Vector3 target;
	private Vector2 trSize;
	private float targetTimer;

    // Use this for initialization
    void Start ()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
        sprites = Resources.LoadAll<Sprite> ("clingy/Princess-Sprite");

		target = gameObject.transform.position;

		trSize = Camera.main.orthographicSize * new Vector2(Screen.width / Screen.height, 1.0f);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {

    }

    void UpdateHeroState (float x, float y)
    {
//      anim.SetFloat("SpeedX", x);
//      anim.SetFloat("SpeedY", y);
        States newState = States.StillDown;

        float ax = Mathf.Abs (x), ay = Mathf.Abs (y);

        if ((x > 0) && (ax >= ay))
            newState = States.Right;
        else if ((x < 0) && (ax >= ay))
            newState = States.Left;
        else if ((y > 0) && (ax <= ay))
            newState = States.Up;
        else if ((y < 0) && (ax <= ay))
            newState = States.Down;
        else {
            newState = state;
            switch (state) {
            case States.Right:
                newState = States.StillRight;
                break;
            case States.Left:
                newState = States.StillLeft;
                break;
            case States.Up:
                newState = States.StillUp;
                break;
            case States.Down:
                newState = States.StillDown;
                break;
            }
        }

        if (newState != state) {
            switch (newState) {
            case States.Up: 
                spriteRenderer.sprite = sprites [10];
                break;
            case States.Down:
                spriteRenderer.sprite = sprites [6];
                break;
            case States.Left:
                spriteRenderer.sprite = sprites [14];
                break;
            case States.Right:
                spriteRenderer.sprite = sprites [7];
                break;
            case States.StillRight:
            case States.StillLeft:
            case States.StillUp:
            case States.StillDown:
                spriteRenderer.sprite = sprites [5];
                break;
            }

            state = newState;
        }
    }


    // Update is called once per frame
    void Update ()
    {

        int playerNumber = 1;
        bool active = true;
		float minDistanceThreshold = trSize.y * 0.1f;

		// if close enough, set new target
		Vector3 pos = gameObject.transform.position;

		targetTimer -= Time.deltaTime;

		if (targetTimer <= 0.0f) {
			target = pos;
		}

		while ((target - pos).magnitude < minDistanceThreshold) 
		{
			target = new Vector3((2.0f * Random.value - 1.0f) * trSize.x, -Random.value  * trSize.y, 0.0f);
			targetTimer = 1.4f;
		}

		Vector3 direction = (target - pos);
		direction.Normalize ();

		Vector2 velocity = maxSpeed * new Vector2 (direction.x, direction.y);
		print ("velocity " + velocity + "target: " + target + "pos: ");

		rigidbody2D.velocity = velocity;

		UpdateHeroState (velocity.x, velocity.y);

    }
}
