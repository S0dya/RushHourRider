using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    public Material[] bikeMaterials;
    [SerializeField] Material[] skyBoxes;

    protected override void Awake()
    {
        base.Awake();

        LoadData();
        Settings.firstTime = false;
    }

    //UI
    public void Open(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 1, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => OpenCompletely(CG));
        tween.setUseEstimatedTime(true);
    } 
    void OpenCompletely(CanvasGroup CG) => CG.blocksRaycasts = true;
    
    public void Close(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 0, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => CloseComletely(CG));
        tween.setUseEstimatedTime(true);
    }
    void CloseComletely(CanvasGroup CG) => CG.blocksRaycasts = false;

    public void FadeIn(CanvasGroup CG, float durationStart) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad);
    public void FadeInAndOut(CanvasGroup CG, float durationStart, float durationEnd) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOut(CG, durationEnd));
    public void FadeOut(CanvasGroup CG, float durationEnd) => LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);

    public void FadeInAndOutAndDestroy(GameObject gO, float durationStart, float durationEnd)
    {
        CanvasGroup CG = gO.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOutAndDestroy(gO, CG, durationEnd));
    }
    public void FadeOutAndDestroy(GameObject gO, CanvasGroup CG, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);
        Destroy(gO);
    }

    //lightning
    public void SetSkyBox()
    {
        RenderSettings.skybox = skyBoxes[Settings.currentColorOfBGI];
    }

    //save/load
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("money", Settings.money);
        PlayerPrefs.SetInt("currentColorOfBikeI", Settings.currentColorOfBikeI);
        PlayerPrefs.SetInt("currentColorOfBGI", Settings.currentColorOfBGI);
        for (int i = 0; i < Settings.itemPrices.Length; i++)
        {
            PlayerPrefs.SetInt($"itemPrice {i}", Settings.itemPrices[i]);
        }

        PlayerPrefs.SetInt("firstTime", Settings.firstTime ? 0 : 1);
        PlayerPrefs.SetInt("isMusicOn", Settings.isMusicOn ? 0 : 1);
        PlayerPrefs.SetInt("isTouchInput", Settings.isTouchInput ? 0 : 1);
    }

    public void LoadData() 
    {
        if (PlayerPrefs.GetInt("firstTime") == 0) return;

        Settings.money = PlayerPrefs.GetInt("money");
        Settings.currentColorOfBikeI = PlayerPrefs.GetInt("currentColorOfBikeI");
        Settings.currentColorOfBGI = PlayerPrefs.GetInt("currentColorOfBGI");
        for (int i = 0; i < Settings.itemPrices.Length; i++)
        {
            Settings.itemPrices[i] = PlayerPrefs.GetInt($"itemPrice {i}");
        }

        Settings.isMusicOn = (PlayerPrefs.GetInt("isMusicOn") == 0);
        Settings.isTouchInput = (PlayerPrefs.GetInt("isTouchInput") == 0);
    }
}
