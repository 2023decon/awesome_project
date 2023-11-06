using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public Button startButton;
    public Sprite start_default;
    public Sprite start_hover;
    public Button quitButton;
    public Sprite quit_default;
    public Sprite quit_hover;
    public GameObject lab;
    public Image background;

    private float tick = 0;
    private bool goIn = false;

    public void HoverStart() {
        startButton.image.sprite = start_hover;
        startButton.transform.localScale = new Vector2(1.1f, 1.1f);
    }
    public void UnHoverStart() {
        startButton.image.sprite = start_default;
        startButton.transform.localScale = Vector2.one;
    }
    public void OnStart() {
        tick = 0;

        StartCoroutine(_onStart());
    }

    IEnumerator _onStart() {
        goIn = true;
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("gameScene");
    }

    public void HoverQuit() {
        quitButton.image.sprite = quit_hover;
        quitButton.transform.localScale = new Vector2(1.1f, 1.1f);
    }
    public void UnHoverQuit() {
        quitButton.image.sprite = quit_default;
        quitButton.transform.localScale = Vector2.one;
    }
    public void OnQuit() {
        Application.Quit();
    }

    private void Update() {
        tick += Time.deltaTime;

        if (!goIn) {
            if (tick <= 8) lab.transform.localScale = new Vector2(1 + tick * 0.025f, 1 + tick * 0.025f);
            else lab.transform.localScale = new Vector2(1.4f - tick * 0.025f, 1.4f - tick * 0.025f);

            if (tick > 16) {
                tick = 0;
            }
        } else {
            if (tick <= 1) {
                lab.transform.localScale = new Vector2(1 + tick * 2f, 1 + tick * 2f);
                Color col = background.color;

                col.a = 1 - tick;

                background.color = col;
            }
        }
    }
}
