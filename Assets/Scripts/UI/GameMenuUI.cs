using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : SingletonMonobehaviour<GameMenuUI>
{
    [SerializeField] CanvasGroup gameMenuCG;

    [SerializeField] Image musicImage;


    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        SetSound();
    }

    //buttons
    public void ResumeButton()
    {
        ToggleGameMenu(false);
    }
    public void HomeButton()
    {
        LoadingSceneManager.I.LoadMenu();
    }
    public void MusicButton()
    {
        Settings.isMusicOn = !Settings.isMusicOn;
        AudioManager.I.ToggleSound(Settings.isMusicOn);
    }

    //methods

    public void ToggleGameMenu(bool val)
    {
        if (val) GameManager.I.Open(gameMenuCG, 0.1f);
        else GameManager.I.Close(gameMenuCG, 0.5f);

        //StartCoroutine();
    }

    void SetSound() => musicImage.color = new Color(255, 255, 255, (Settings.isMusicOn ? 1 : 0.5f));
}
