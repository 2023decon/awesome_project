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
    CHASING = 3,
    ATTACK = 4,
}

public class Enemy : MonoBehaviour
{

    public float moveSpeed;
    public float detectRange;
    public float attackRange;

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

    public Vector3 constraintPos = Vector3.zero;

    public GameObject player;

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

            if (constraintPos.z == 1)
            {
                rbody.gravityScale = 0.05f;
                transform.position = new Vector2(constraintPos.x, constraintPos.y);
            } else
            {
                rbody.gravityScale = 3f;
            }
        }
        if (status == StatusEffect.STUNNING)
        {
            if (constraintPos.z == 1)
            {
                //transform.position = new Vector2(constraintPos.x, constraintPos.y);
            }
        }
        else if (status == StatusEffect.CHASING)
        {
            Chase();
        }
        else {
            rbody.gravityScale = gravity;
        }
        if (attackRange < Vector2.Distance(player.transform.position, transform.position)
                /*&& Vector2.Distance(player.transform.position, transform.position) <= detectRange*/)
        {
            status = StatusEffect.CHASING;
            statusDuration = 3.0f;
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
                    statusDuration = 1f;

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
        if (stack >= maxStack && status == StatusEffect.DEFAULT) {
            stack = maxStack;

            StartCoroutine(Become(StatusEffect.AIRBONE, 2.5f));

            if (Player.Instance.transform.position.x < transform.position.x) rbody.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
            else rbody.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
    }

    public void Strike()
    {
        if (status == StatusEffect.AIRBONE)
        {
            int usedStack = maxStack - stack;

            if (usedStack <= 0) return;

            StartCoroutine(Become(StatusEffect.STUNNING, 0.5f * usedStack));

            if (Player.Instance.transform.position.x < transform.position.x) rbody.AddForce(Vector2.right * 6, ForceMode2D.Impulse);
            else rbody.AddForce(Vector2.left * 6, ForceMode2D.Impulse);
        }
    }

    public void Chase()
    {
        if(status == StatusEffect.CHASING)
        {
            if (player.transform.position.x < transform.position.x)
                rbody.velocity = Vector2.left * moveSpeed;
            else if (player.transform.position.x > transform.position.x)
                rbody.velocity = Vector2.right * moveSpeed;

            if (attackRange >= Vector2.Distance(player.transform.position, transform.position)
                /*|| Vector2.Distance(player.transform.position, transform.position) > detectRange*/)
            {
                statusDuration = 0f;
                status = StatusEffect.DEFAULT;
                rbody.velocity = Vector2.zero;
                Debug.Log(status);
            }
        }
    }

    public IEnumerator Become(StatusEffect status_, float duration = 0) {
        status = status_;
        if (status != StatusEffect.DEFAULT) statusDuration = duration;
        else {
            constraintPos = Vector3.zero;
        }

        if (status == StatusEffect.AIRBONE) {
            rbody.velocity = Vector2.up * 15;
        } 

        else if (status == StatusEffect.STUNNING)
        {
            rbody.velocity = Vector2.down * 30;

            yield return new WaitForSeconds(0.3f);
        }
    }

    public virtual void OnHurt(int damage, bool succeed){}
    public virtual void OnDeath(){}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpSensor"))
        {
            //rbody.gravityScale = 1;
            //rbody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        }
    }
}