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

    protected override void Awake()
    {
        base.Awake();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < shopItems.Length; i++) shopItems[i].index = i;
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

    //other methods
    void ToggleCG(bool val, CanvasGroup CG)
    {
        if (val) gameManager.Open(CG, 0.5f);
        else gameManager.Close(CG, 0.5f);
    }
}
