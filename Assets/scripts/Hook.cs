using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) {
        if (Player.Instance.ropeState != 1) return;

        if ((LayerMask.GetMask("tile") & (1 << other.gameObject.layer)) != 0) {
            Player.Instance.ropeState = 2;

        }
        else if ((LayerMask.GetMask("enemy") & (1 << other.gameObject.layer)) != 0) {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            if (enemy.status == StatusEffect.AIRBONE) {
                Player.Instance.ropeState = 2;
                Player.Instance.attackingAirbone = true;
                enemy.statusDuration = 1f;

                enemy.constraintPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1);

                if (Player.Instance.transform.position.x > enemy.transform.position.x) {
                    transform.position = enemy.transform.position + new Vector3(1.25f, 0.5f);
                } else {
                    transform.position = enemy.transform.position + new Vector3(-1.25f, 0.5f);
                }
            }
        }
    }

    private void Update() {
        if (Player.Instance.ropeState == 2) {
            Player.Instance.transform.position = Vector2.MoveTowards(Player.Instance.transform.position, transform.position, 50f * Time.deltaTime);

            if (Vector2.Distance(Player.Instance.transform.position, transform.position) < 0.8f || Vector2.Distance(Player.Instance.transform.position - new Vector3(0, 0.5f), transform.position) < 0.8f) {
                Player.Instance.ropeState = 0;
                
                Destroy(gameObject);
                Destroy(Player.Instance.ropeNow.gameObject);

                Player.Instance.ropeNow = null;

                if (Player.Instance.attackingAirbone) Cam.Instance.InAirbone();
            }
        }
    }
}
