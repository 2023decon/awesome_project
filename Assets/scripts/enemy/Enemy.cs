using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum StatusEffect {
    DEFAULT = 0,
    AIRBONE = 1,
    STUNNING = 2,
}

public class Enemy : MonoBehaviour
{

    public float moveSpeed;

    public StatusEffect status;
    public float statusDuration;
    public Slider stackDisplay;

    public int health;
    public int maxHealth;

    //stack of airbone
    public int stack;
    public int maxStack;
    public bool useStack = true;
    public float gravity;

    //time of invincibility
    private float invincibility;
    private Rigidbody2D rbody;

    private Vector3 constraintPos = Vector3.zero;

    void Awake()
    {
        status = StatusEffect.DEFAULT;
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (invincibility > 0) {
            invincibility -= Time.fixedDeltaTime;
        }

        if (statusDuration > 0) {
            statusDuration -= Time.fixedDeltaTime;
        } else if (status != StatusEffect.DEFAULT) {
            StartCoroutine(Become(StatusEffect.DEFAULT));
        }

        stackDisplay.value = (float)stack / maxStack;

        if (status == StatusEffect.AIRBONE) {
            rbody.gravityScale = 0.02f;

            if (constraintPos.z == 1) transform.position = new Vector2(constraintPos.x, transform.position.y);
        } else {
            rbody.gravityScale = gravity;
        }
    }

    public bool Hurt(int damage) {
        if (invincibility <= 0) {
            health -= damage;

            DamageHandler.Instance.ShowDamage(damage, transform.position + new Vector3(-0.5f + UnityEngine.Random.Range(0f, 1f), 1f));
            

            if (useStack) {
                if (status != StatusEffect.AIRBONE) {
                    stack++;
                } else {
                    stack--;
                    statusDuration = 2;

                    if (stack < 1) {
                        Debug.Log("return");
                        StartCoroutine(Become(StatusEffect.DEFAULT));
                    }
                }
            }

            OnHurt(damage, true);

            return true;
        } else {
            OnHurt(damage, false);

            return false;
        }
    }

    public void Flow() {
        if (stack >= maxStack) {
            stack = maxStack;

            StartCoroutine(Become(StatusEffect.AIRBONE, 2));

            if (Player.Instance.transform.position.x < transform.position.x) rbody.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
            else rbody.AddForce(Vector2.left * 15, ForceMode2D.Impulse);
        }
    }

    public IEnumerator Become(StatusEffect status_, float duration = 0) {
        status = status_;
        if (status != StatusEffect.DEFAULT) statusDuration = duration;
        else {
            constraintPos = Vector2.zero;
        }

        if (status == StatusEffect.AIRBONE) {
            rbody.velocity = Vector2.up * 20;

            yield return new WaitForSeconds(0.2f);
            
            constraintPos = new Vector3(transform.position.x, transform.position.y, 1);
            rbody.velocity = Vector3.zero;
        }
    }

    public virtual void OnHurt(int damage, bool succeed){}
    public virtual void OnDeath(){}
}
