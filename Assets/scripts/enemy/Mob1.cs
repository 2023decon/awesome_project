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

            if (atkDelay_ > atkDelay)
            {
                if (player.transform.position.x < transform.position.x)
                    spriteRenderer.flipX = true;
                else if (player.transform.position.x > transform.position.x)
                    spriteRenderer.flipX = false;

                atkDelay_ = 0;

                animator.ResetTrigger("attack");
                animator.SetTrigger("attack");

                StartCoroutine(HitJudgement());
            }
        }
    }

    public override void OnHurt(int damage, bool succeed)
    {
        animator.ResetTrigger("hurt");
        animator.SetTrigger("hurt");

    }

    private IEnumerator HitJudgement()
    { 
        yield return new WaitForSeconds(0.25f);
        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange &&
            ((player.transform.position.x > transform.position.x && !spriteRenderer.flipX) ||
            (player.transform.position.x < transform.position.x && spriteRenderer.flipX)))
        {
            player.Hurt(attackDamage);
        }
    }
}
