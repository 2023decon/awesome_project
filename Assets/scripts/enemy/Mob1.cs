using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob1 : Enemy
{
    public float atkDelay;
    private Animator animator;
    private float atkDelay_ = 0;

    private void Start() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        if (!isOutRange && status == StatusEffect.DEFAULT) {
            atkDelay_ += Time.deltaTime;

            if (atkDelay_ > atkDelay) {
                atkDelay_ = 0;

                animator.ResetTrigger("attack");
                animator.SetTrigger("attack");

                if (Vector2.Distance(player.transform.position, transform.position) <= attackRange) {
                    player.Hurt(attackDamage);
                }
            }
        }
    }

    public override void OnHurt(int damage, bool succeed)
    {
        animator.ResetTrigger("hurt");
        animator.SetTrigger("hurt");

    }
}
