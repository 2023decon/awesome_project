using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    float firstDir = 0f;
    int multiKey = 0;
    public float moveSpeed;
    public int attackDamage;
    public float ropeSpeed;
    public float gravity;

    public int ropeState = 0;

    public int direction;

    public GameObject facing;
    public Rope rope;
    public Rope ropeNow;
    public GameObject hook;

    public bool attackingAirbone = false;

    private static Player instance;
    public static Player Instance {
        get {
            if (!instance) return null;
            return instance;
        }
    }

    private Rigidbody2D rbody;
    private BoxCollider2D boxCollider;

    private void Awake() {
        instance = this;

        rbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        float input = GetAxisRaw(KeyCode.A, KeyCode.D);

        if (ropeState != 0) input = 0;

        if (input != 0) direction = (int)input;
        float speed = moveSpeed;

        if (attackingAirbone) {
            rbody.gravityScale = 0.02f;
            speed = moveSpeed / 3;

            CheckAirbone();
        } else {
            if (ropeState == 2)
            {
                rbody.gravityScale = 0.5f;
            } else
            {
                rbody.gravityScale = gravity;
            }
        }

        if (!CheckIsObstacle(direction)) transform.position = new Vector2(transform.position.x + input * speed * Time.deltaTime, transform.position.y);

        SetFacingCursor();

        if (ropeState == 0) {
            if (Input.GetMouseButtonDown(1))
            {
                if (!attackingAirbone) CastRope();
            }
            else if (Input.GetMouseButtonDown(0)) {
                MeleeAttack();
            }
            else if (Input.GetKeyDown(KeyCode.W)) {
                if (!attackingAirbone) FlowAttack();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (attackingAirbone) StrikeAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    bool CheckIsObstacle(int dir) {
        var cols = Physics2D.OverlapBoxAll(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y) + new Vector3(0.3f * dir, 0.2f, 0), boxCollider.size / 2, 0, LayerMask.GetMask("tile"));

        bool isExists = false;
        if (cols.Length > 0)
        {
            isExists = true;
        }

        return isExists;
    }

    void SetFacingCursor() {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - facing.transform.position.y;
        float dx = mousePos.x - facing.transform.position.x;

        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        facing.transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);
    }

    void CastRope() {
            ropeState = 1;

            ropeNow = Instantiate(rope, transform.position + new Vector3(0, 0.5f), Quaternion.identity);
            var hk = Instantiate(hook, transform.position + new Vector3(0, 0.5f), Quaternion.identity);

            hk.transform.rotation = facing.transform.rotation;
            ropeNow.hook = hk;
    }

    void MeleeAttack() {
        float coefficient = 1.00f;  //일반 공격 계수
        var enemies = Physics2D.OverlapCircleAll(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y) + new Vector3(2 * direction, 1f, 0), 1.5f, LayerMask.GetMask("enemy"));

        for (int i = 0; i < enemies.Length; i++) {
            var enemy = enemies[i];
            if (!enemy) continue;

            enemy.GetComponent<Enemy>().Hurt(Mathf.FloorToInt(attackDamage * coefficient));
        }
    }

    void FlowAttack() {
        var enemy = Physics2D.OverlapCircle(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y) + new Vector3(2f * direction, 1f, 0), 1.5f, LayerMask.GetMask("enemy"));
        if (enemy == null) return;

        enemy.GetComponent<Enemy>().Flow();
    }

    void StrikeAttack()
    {
        if (!attackingAirbone)
        {
            return;
        }

        var enemy = Physics2D.OverlapCircle(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y) + new Vector3(2f * direction, 1f, 0), 1.5f, LayerMask.GetMask("enemy"));
        if (enemy == null) return;

        enemy.GetComponent<Enemy>().Strike();
    }

    void CheckAirbone() {
        if (ropeState != 0) return;

        var enemies = Physics2D.OverlapCircleAll(transform.position + new Vector3(boxCollider.offset.x, boxCollider.offset.y), 2f, LayerMask.GetMask("enemy"));

        bool isAirbone = false;
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i] == null) return;

            var enemy = enemies[i].GetComponent<Enemy>();

            if (enemy.status == StatusEffect.AIRBONE) {
                isAirbone = true;
                break;
            }
        }

        if (!isAirbone) {
            attackingAirbone = false;
            Cam.Instance.OutAirbone();
        }
    }

    void Jump() {
        rbody.velocity = new Vector2(0, 10);
    }

    float GetAxisRaw(KeyCode key1, KeyCode key2) {
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
