using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUI : SingletonMonobehaviour<GameMenuUI>
{
    GameManager gameManager;

    [SerializeField] Player player;
    [SerializeField] CanvasGroup gameMenuCG;
    [SerializeField] CanvasGroup gameoverCG;
    [SerializeField] CanvasGroup countCG;

    [SerializeField] Image musicImage;
    [SerializeField] Image AdsImage;

    [SerializeField] TextMeshProUGUI gameoverScoreText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] RectTransform scoreAddParent;
    [SerializeField] GameObject scoreAddPrefab;

    [SerializeField] GameObject countTextObj;
    TextMeshProUGUI countText;
    CanvasGroup countTextCG;

    [SerializeField] CanvasGroup[] boostCGs;

    public int score;
    public int scoreMultiplier;

    public int maxCounting;
    //local 
    int curCount;


    protected override void Awake()
    {
        base.Awake();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        countText = countTextObj.GetComponent<TextMeshProUGUI>();
        countTextCG = countTextObj.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        SetAlphaOfImage(musicImage, Settings.isMusicOn);
        Settings.currentTimeScale = Time.timeScale = 1;
    }

    //buttons
    public void PauseButton()
    {
        player.isMenuOpened = true;
        Time.timeScale = 0f;
        ToggleGameMenu(true);
    }
    public void ResumeButton()
    {
        ToggleGameMenu(false);
        gameManager.Open(countCG, 0.2f);

        curCount = maxCounting;
        Counting();
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
    public void AddScore(int val)
    {
        val *= scoreMultiplier;
        GameObject textObj = Instantiate(scoreAddPrefab, scoreAddParent);
        var text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = $"+{val}";
        gameManager.FadeInAndOutAndDestroy(textObj, 1, 0.2f);

        score += val;
        SetScoreText(scoreText);
    }

    public void ToggleGameMenu(bool val)
    {
        if (val) gameManager.Open(gameMenuCG, 0.1f);
        else gameManager.Close(gameMenuCG, 0.5f);

        //StartCoroutine();
    }

    public void Gameover()
    {
        SetScoreText(gameoverScoreText);
        gameManager.Open(gameoverCG, 0);
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
        Settings.money = score/5;
    }

    public void ToggleBoost(int i, bool val)
    {
        if (val) gameManager.FadeIn(boostCGs[i], 0.3f);
        else gameManager.FadeOut(boostCGs[i], 0);
    }

    public void Counting()
    {
        countText.text = curCount.ToString();
        FadeInAndOut(countTextCG, 0.8f, 1.4f);
    }
    void FadeInAndOut(CanvasGroup CG, float durationStart, float durationEnd) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() => FadeOutCounting(CG, durationEnd));
    void FadeOutCounting(CanvasGroup CG, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true);
        if (curCount != 1)
        {
            curCount--;
            Counting();
        }
        else
        {
            Time.timeScale = Settings.currentTimeScale;
            player.isMenuOpened = false;
            gameManager.Close(countCG, 0f);
        }
    }

    void SetScoreText(TextMeshProUGUI text) => text.text = score.ToString();
    void SetAlphaOfImage(Image image, bool isFullAlpha) => image.color = new Color(0, 0, 0, (isFullAlpha ? 1 : 0.5f));
}
