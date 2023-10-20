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
            MoveInDirection(Vector2.right, Player.Instance.ropeSpeed);
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

    public void MoveInDirection(Vector3 direction, float moveSpeed)
    {
        Vector3 moveVector = direction * moveSpeed * Time.deltaTime;
        hook.transform.Translate(moveVector);
    }
}
