using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) {
        if (Player.Instance.ropeState != 1) return;

        if ((LayerMask.GetMask("tile") & (1 << other.collider.gameObject.layer)) == 0) {
            return;
        }

        Player.Instance.ropeState = 2;
    }

    private void Update() {
        if (Player.Instance.ropeState == 2) {
            Player.Instance.transform.position = Vector2.MoveTowards(Player.Instance.transform.position, transform.position, 0.5f);

            if (Vector2.Distance(Player.Instance.transform.position, transform.position) < 0.8f || Vector2.Distance(Player.Instance.transform.position - new Vector3(0, 0.5f), transform.position) < 0.8f) {
                Player.Instance.ropeState = 0;
                
                Destroy(gameObject);
                Destroy(Player.Instance.ropeNow.gameObject);

                Player.Instance.ropeNow = null;
            }
        }
    }
}
