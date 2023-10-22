using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float lifeTime;
    public string message;
    private float _lifeTime = 0;
    private Text text;

    public Canvas canvas;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        _lifeTime += Time.deltaTime;

        text.text = message;

        var col = text.color;
        col.a = lifeTime - _lifeTime;

        text.color = col;

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f * Time.deltaTime);

        if (_lifeTime > lifeTime) {
            Destroy(canvas.gameObject);
        }
    }

    public void SetColor(Color color) {
        text.color = color;
    }
}
