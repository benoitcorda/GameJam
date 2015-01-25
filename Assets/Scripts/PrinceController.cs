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
	public float levelDuration;
    
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
	private float levelTimer;
    
    // Use this for initialization
    void Start ()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
        moodSpriteRenderer = moodMeter.GetComponent<SpriteRenderer> ();

        sprites = Resources.LoadAll<Sprite> ("clingy/Prince-Sprite");
        moodSprites = Resources.LoadAll<Sprite> ("clingy/Princess-Meter");

        moodAverage = 1.0f;
		
		levelTimer = levelDuration;
    }

    void OnCollisionEnter2D (Collision2D collision)
    {

    }

	void ChooseNewEntryLevel()
	{
		if  (moodAverage <= 0.33333333333334) {
			LevelState.levelState = LevelState.State.Clingy;
		} else if (moodAverage <= 0.66666666666667) {
			LevelState.levelState = LevelState.State.Chatty;
		} else {
			LevelState.levelState = LevelState.State.Clingy;;
		}

		Application.LoadLevel((int)LevelState.levelState);
    }
    
    void UpdateMoodMeter (Vector2 v)
    {
        float delta = 0.003f;
        float maxAngle = 50.0f;
        thresholds = new float[]{1.0f, 4.0f, 8.0f, 12.0f};

        var ppVector = (princess.transform.position - gameObject.transform.position);

        float distance = (ppVector).magnitude;
        float moodDelta = (distance < thresholds[1] ? +1 : (distance < thresholds[2] ? 0 : -1));

        var tendency = 0;
        if ((v.magnitude > 1e-4) && (ppVector.magnitude > 1e-4))
        {
            ppVector.Normalize();
            tendency = (Vector2.Dot(v, ppVector) >= 0 ? +1 : -1);
        }
        
        if (distance < thresholds[0]) // spike mood if we touch
        {
          moodAverage = 1.0f;
        }
        else 
        {
          moodAverage = Mathf.Max(0.0f, Mathf.Min(1.0f, moodAverage + delta * moodDelta));
        }

        if (moodAverage <= 0.16666666666667) {
            moodSpriteRenderer.sprite = moodSprites [0];
        } else if (moodAverage <= 0.33333333333334) {
            moodSpriteRenderer.sprite = moodSprites [1];
        } else if (moodAverage <= 0.66666666666667) {
            moodSpriteRenderer.sprite = moodSprites [2];
        } else if (moodAverage <= 0.83333333333333) {
            moodSpriteRenderer.sprite = moodSprites [3];
        } else {
            moodSpriteRenderer.sprite = moodSprites [4];
        }

        // change needle
        float needleAngle = Mathf.Lerp(-maxAngle, maxAngle, distance / thresholds[3]);
        needle.transform.eulerAngles = new Vector3(0.0f, 0.0f, needleAngle);

//        print ("distance:" + distance + " moodDelta:" + moodDelta + "avg:" + moodAverage + "needle: " + needleAngle);
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
                spriteRenderer.sprite = sprites [5];
                break;
            case States.Down:
                spriteRenderer.sprite = sprites [0];
                break;
            case States.Left:
                spriteRenderer.sprite = sprites [15];
                break;
            case States.Right:
                spriteRenderer.sprite = sprites [11];
                break;
            case States.StillRight:
            case States.StillLeft:
            case States.StillUp:
            case States.StillDown:
                spriteRenderer.sprite = sprites [0];
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

		
		
		// update level timer
		levelTimer -= Time.deltaTime;
		if (levelTimer <= 0.0f) {
            ChooseNewEntryLevel();
        }
		

    }
}
