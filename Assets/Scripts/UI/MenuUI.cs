using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : SingletonMonobehaviour<MenuUI>
{
    GameManager gameManager;

    [SerializeField] CanvasGroup menuCG;
    [SerializeField] CanvasGroup shopCG;
    [SerializeField] CanvasGroup settingsCG;

    [Header("Shop")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] ShopItemUI[] shopItems;

    [Header("Settings")]
    [SerializeField] Image musicImage;

    [SerializeField] Image touchInputImage;
    [SerializeField] Image buttonsInputImage;

    protected override void Awake()
    {
        base.Awake();

        SetMoneyText();
        SetInputType();
        SetAlphaOfImage(musicImage, Settings.isMusicOn);

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < shopItems.Length; i++) shopItems[i].index = i;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Settings.money += 1500;
            SetMoneyText();
        }
    }

    //menu
    //buttons
    public void PlayButton()
    {
        LoadingSceneManager.I.StartGame();
    }
    public void ShopButton()
    {
        ToggleCG(false, menuCG);
        ToggleCG(true, shopCG);
    }
    public void SettingsButton()
    {
        ToggleCG(false, menuCG);
        ToggleCG(true, settingsCG);
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    //methods

    //shop
    //buttons
    public void CloseShopButton()
    {
        ToggleCG(false, shopCG);
        ToggleCG(true, menuCG);
    }


    //Settings
    //buttons
    public void MusicButton()
    {
        Settings.isMusicOn = !Settings.isMusicOn;
        AudioManager.I.ToggleSound();
        SetAlphaOfImage(musicImage, Settings.isMusicOn);
    }

    public void TouchInputButton()
    {
        if (!Settings.isTouchInput)
        {
            Settings.isTouchInput = true;
            SetInputType();
        }
    }
    public void ButtonsInputButton()
    {
        if (Settings.isTouchInput)
        {
            Settings.isTouchInput = false;
            SetInputType();
        }
    }

    public void CloseSettingsButton()
    {
        ToggleCG(false, settingsCG);
        ToggleCG(true, menuCG);
    }



    //other methods
    public void SetMoneyText() => moneyText.text = Settings.money.ToString();
    public void SetInputType()
    {
        SetAlphaOfImage(touchInputImage, Settings.isTouchInput);
        SetAlphaOfImage(buttonsInputImage, !Settings.isTouchInput);
    }

    public void SetAlphaOfImage(Image image, bool isFullAlpha) => image.color = new Color(0, 0, 0, (isFullAlpha ? 1 : 0.5f));


    void ToggleCG(bool val, CanvasGroup CG)
    {
        if (val) gameManager.Open(CG, 0.5f);
        else gameManager.Close(CG, 0.5f);
    }
}
