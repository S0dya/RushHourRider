using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : SingletonMonobehaviour<GameMenuUI>
{
    [SerializeField] CanvasGroup gameMenuCG;
    [SerializeField] CanvasGroup gameoverCG;

    [SerializeField] Image musicImage;
    [SerializeField] Image AdsImage;

    [SerializeField] TextMeshProUGUI gameoverScoreText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] CanvasGroup[] boostCGs;

    public int score;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        SetAlphaOfImage(musicImage, Settings.isMusicOn);
        Settings.currentTimeScale = Time.timeScale = 1;
    }

    //buttons
    public void PauseButton()
    {
        Player.I.isMenuOpened = true;
        Time.timeScale = 0f;
        ToggleGameMenu(true);
    }
    public void ResumeButton()
    {
        ToggleGameMenu(false);
        Time.timeScale = Settings.currentTimeScale;
        Player.I.isMenuOpened = false;
    }
    public void HomeButton()
    {
        LoadingSceneManager.I.LoadMenu();
    }
    public void MusicButton()
    {
        Settings.isMusicOn = !Settings.isMusicOn;
        AudioManager.I.ToggleSound(Settings.isMusicOn);
        SetAlphaOfImage(musicImage, Settings.isMusicOn);
    }
    //gameover buttons
    public void ReplayButton()
    {
        LoadingSceneManager.I.RestartGame();

    }
    public void PlayAdsButton()
    {
        AdsManager.I.ShowRewardedAd();
    }


    //methods
    void ChangeScore(int val)
    {
        score += val;
        SetScoreText(scoreText);
    }

    public void ToggleGameMenu(bool val)
    {
        if (val) GameManager.I.Open(gameMenuCG, 0.1f);
        else GameManager.I.Close(gameMenuCG, 0.5f);

        //StartCoroutine();
    }

    public void Gameover()
    {
        SetScoreText(gameoverScoreText);
        GameManager.I.Open(gameoverCG, 0);
    }

    public void RewardPlayer()
    {
        score *= 2;
        SetScoreText(gameoverScoreText);
        SetAlphaOfImage(AdsImage, false);
        AdsImage.raycastTarget = false;
    }

    void CountScore()
    {
        Settings.money = score / 5;
    }

    public void ToggleBoost(int i, bool val)
    {
        if (val) GameManager.I.FadeIn(boostCGs[i], 0.3f);
        else GameManager.I.FadeOut(boostCGs[i], 0);
    }

    void SetScoreText(TextMeshProUGUI text) => text.text = score.ToString();
    void SetAlphaOfImage(Image image, bool isFullAlpha) => image.color = new Color(0, 0, 0, (isFullAlpha ? 1 : 0.5f));
}
