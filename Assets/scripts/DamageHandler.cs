using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    static private DamageHandler instance;
    static public DamageHandler Instance {
        get {
            if (instance == null) return null;
            else return instance;
        }
    }
    public Canvas textCanvas;
    void Start()
    {
        instance = this;
    }

    public void ShowDamage(int damage, Vector2 pos, Color color = default) {
        var canvas = Instantiate(textCanvas, pos, Quaternion.identity);
        if (canvas == null) return;

        var text = canvas.GetComponentInChildren<DamageText>();

        if (color != default) text.SetColor(color);
        text.message = damage.ToString();
    }
}
