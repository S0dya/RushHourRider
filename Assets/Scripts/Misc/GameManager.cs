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
        //Settings.firstTime = false;
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
        /*
        for (int i = 0; i < Settings.soundVolume.Length; i++)
        {
            PlayerPrefs.SetFloat($"Volume {i}", Settings.soundVolume[i]);
        }

        PlayerPrefs.SetInt("firstTime", Settings.firstTime ? 0 : 1);
        */
    }

    public void LoadData() 
    {
        /*
        Settings.firstTime = (PlayerPrefs.GetInt("firstTime") == 0);
        if (Settings.firstTime) return;

        for (int i = 0; i < Settings.soundVolume.Length; i++)
        {
            Settings.soundVolume[i] = PlayerPrefs.GetFloat($"Volume {i}");
        }
        */
    }
}
