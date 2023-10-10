using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Rope : MonoBehaviour
{
    public GameObject hook;

    private SpriteRenderer renderer_;
    private BoxCollider2D coll;
    void Start() {
        renderer_ = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (Player.Instance.ropeState == 1) {
            MoveInDirection(Vector2.right, 35f);
        }

        float distance = Vector2.Distance(hook.transform.position, Player.Instance.transform.position);

        Vector3 centerPosition = (Player.Instance.transform.position + hook.transform.position) / 2f;

        transform.position = centerPosition;

        transform.rotation = hook.transform.rotation;

        renderer_.size = new Vector2(distance, renderer_.size.y);
        coll.size = renderer_.size;

        if (distance > 16) {
            Player.Instance.ropeState = 0;
            Destroy(hook);
            Destroy(gameObject);
        }
    }
    
    public static Vector3 CalcFacingPos(Vector3 position, Vector2 rot, float dist, Vector3 offset)
    {
        Vector3 pos = new Vector3(position.x, position.y, position.z);

        float radY = rot.y * Mathf.Deg2Rad;
        float radX = rot.x * Mathf.Deg2Rad;

        float y = -Mathf.Sin(radX) * dist;

        float manipulator = Mathf.Cos(radX);
        float x = -Mathf.Sin(radY) * manipulator * dist;
        float z = Mathf.Cos(radY) * manipulator * dist;

        pos.y += offset.y + y;
        pos.x += offset.x + x;
        pos.z += offset.z + z;

        return pos;
    }

    public void MoveInDirection(Vector3 direction, float moveSpeed)
    {
        Vector3 moveVector = direction * moveSpeed * Time.deltaTime;
        hook.transform.Translate(moveVector);
    }
}
