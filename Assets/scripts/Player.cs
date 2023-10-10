using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    float firstDir = 0f;
    int multiKey = 0;

    public int ropeState = 0;

    public GameObject facing;
    public Rope rope;
    public Rope ropeNow;
    public GameObject hook;

    private static Player instance;
    public static Player Instance {
        get {
            if (!instance) return null;
            return instance;
        }
    }

    private Rigidbody2D rbody;

    private void Awake() {
        instance = this;

        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float input = GetAxisRaw(KeyCode.A, KeyCode.D);

        if (ropeState != 0) input = 0;

        transform.Translate(Vector2.right * input * 0.05f);

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - facing.transform.position.y;
        float dx = mousePos.x - facing.transform.position.x;

        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        facing.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);

        if (Input.GetMouseButtonDown(1) && ropeState == 0)
        {
            ropeState = 1;

            ropeNow = Instantiate(rope, transform.position + new Vector3(0, 0.5f), Quaternion.identity);
            var hk = Instantiate(hook, transform.position + new Vector3(0, 0.5f), Quaternion.identity);

            hk.transform.rotation = facing.transform.rotation;
            ropeNow.hook = hk;

            //rr.transform.position = testPos;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (rbody.velocity.y < 3 && rbody.velocity.y > 0) {
            rbody.velocity = new Vector2(rbody.velocity.x, -3);
        }
    }

    void Jump() {
        Debug.Log("jump");
        rbody.velocity = new Vector2(0, 10);
    }

    float GetAxisRaw(KeyCode key1, KeyCode key2)
    {
        float dir = 0f;

        if (Time.timeScale == 0f) return dir;
        if (Input.GetKey(key1))
        {
            dir = -1f;

            if (Input.GetKeyDown(key1))
            {
                multiKey++;
            }

            if (multiKey == 1)
            {
                firstDir = -1;
            }
        }

        if (Input.GetKey(key2))
        {
            dir = 1f;

            if (Input.GetKeyDown(key2))
            {
                multiKey++;
            }

            if (multiKey == 2)
            {
                firstDir = 1;
            }
        }

        if (multiKey == 2)
        {
            dir = -firstDir;
        }

        if (Input.GetKeyUp(key1) || Input.GetKeyUp(key2))
        {
            multiKey--;
        }

        if (multiKey == 0)
        {
            dir = 0;
        }

        return dir;
    }
}
