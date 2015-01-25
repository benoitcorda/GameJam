using UnityEngine;

public class PrinceController : MonoBehaviour
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

    private float moodAverage;
    private float moodCurrent;
    public int direction;
    public float maxSpeed;
    public GameObject moodMeter;
    public GameObject needle;
    public GameObject princess;
    private SpriteRenderer spriteRenderer;          // Reference to the player's animator component.
    private SpriteRenderer moodSpriteRenderer;          // Reference to the player's animator component.
    private Sprite[] sprites;
    private Sprite[] moodSprites;
    private Animator anim;          // Reference to the player's animator component.
    private Collider coll;
    private States state;
    private string playerNumber;
    private float[] thresholds;

    // Use this for initialization
    void Start ()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
        moodSpriteRenderer = moodMeter.GetComponent<SpriteRenderer> ();

        sprites = Resources.LoadAll<Sprite> ("clingy/Princess-Sprite");
        moodSprites = Resources.LoadAll<Sprite> ("clingy/Princess-Meter");

        moodAverage = 1.0f;
    }

    void OnCollisionEnter2D (Collision2D collision)
    {

    }

    void UpdateMoodMeter (Vector2 v)
    {
        float delta = 0.003f;
        float maxAngle = 40.0f;
        thresholds = new float[]{4.0f, 8.0f, 12.0f};

        var ppVector = (princess.transform.position - gameObject.transform.position);

        float distance = (ppVector).magnitude;
        float moodDelta = (distance < thresholds[0] ? +1 : (distance < thresholds[1] ? 0 : -1));

        var tendency = 0;
        if ((v.magnitude > 1e-4) && (ppVector.magnitude > 1e-4))
        {
            ppVector.Normalize();
            tendency = (Vector2.Dot(v, ppVector) >= 0 ? +1 : -1);
        }
        
        moodAverage = Mathf.Max(0.0f, Mathf.Min(1.0f, moodAverage + delta * moodDelta));

        if (moodAverage <= 0.33333) {
            moodSpriteRenderer.sprite = moodSprites [tendency==1 ? 1 : 0];
        } else if (moodAverage <= 0.6666) {
            moodSpriteRenderer.sprite = moodSprites [tendency==-1 ? 1 : (tendency==0 ? 2 : 3)];
        } else {
            moodSpriteRenderer.sprite = moodSprites [tendency==-1 ? 3 : 4];
        }

        print ("distance:" + distance + " moodDelta:" + moodDelta + "avg:" + moodAverage + "tendency: " + tendency);


        // change needle
        float needleAngle = Mathf.Lerp(-maxAngle, maxAngle, distance / thresholds[2]);
        needle.transform.eulerAngles = new Vector3(0.0f, 0.0f, needleAngle);
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

		var v = new Vector2(x, y);
        UpdateMoodMeter(v);
    }


    // Update is called once per frame
    void Update ()
    {
        int playerNumber = 1;
        bool active = true;

        if (active) {
            float x = Input.GetAxis ("Horizontal");
            float y = Input.GetAxis ("Vertical");

            // The Speed animator parameter is set to the absolute value of the horizontal input.
//      anim.SetFloat("SpeedX", x);
//      anim.SetFloat("SpeedY", y);

            UpdateHeroState (x, y);

            rigidbody2D.velocity = new Vector2 (maxSpeed * x, maxSpeed * y);
        } else {
            anim.SetFloat ("SpeedX", 0f);
            anim.SetFloat ("SpeedY", 0f);
            rigidbody2D.velocity = Vector2.zero;

            state = States.StillDown;
        }
    }
}
