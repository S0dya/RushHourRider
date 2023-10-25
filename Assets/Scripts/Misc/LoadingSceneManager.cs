using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : SingletonMonobehaviour<LoadingSceneManager>
{
    [SerializeField] CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] Image LoadingBarFill;
    [SerializeField] Camera loadingCam;

    protected override void Awake()
    {
        base.Awake();

    }

    public void StartGame()
    {
        AudioManager.I.EventInstancesDict["MusicMenu"].start();
        AudioManager.I.EventInstancesDict["Ambience"].start();
        StartCoroutine(LoadSceneCor(-1, 1));
    }

    public void LoadMenu()
    {
        AudioManager.I.EventInstancesDict["MusicGame"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.I.EventInstancesDict["Ambience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.I.EventInstancesDict["MusicMenu"].start();
        StartCoroutine(LoadSceneCor(2, 1));
    }

    public void LoadGame()
    {
        AudioManager.I.EventInstancesDict["MusicMenu"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.I.EventInstancesDict["Ambience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.I.EventInstancesDict["MusicGame"].start();
        StartCoroutine(LoadSceneCor(1, 2));
    }

    public void RestartGame()
    {
        StartCoroutine(LoadSceneCor(2, 2));
    }

    IEnumerator LoadSceneCor(int sceneToClose, int SceneToOpen)
    {
        loadingCam.enabled = true;
        if (sceneToClose != -1) SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToOpen, LoadSceneMode.Additive);

        OpenLoadingScreen();

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);
            SetFillAmount(progression);

            yield return null;
        }

        CloseLoadingScreen();
        loadingCam.enabled = false;
    }

    public void OpenLoadingScreen()
    {
        AudioManager.I.ToggleSound(false);
        SetFillAmount(0);
        GameManager.I.Open(loadingScreenCanvasGroup, 0.1f);
    }
    public void CloseLoadingScreen()
    {
        AudioManager.I.ToggleSound(true);
        GameManager.I.Close(loadingScreenCanvasGroup, 0.2f);
    }

    public void SetFillAmount(float progression)
    {
        LoadingBarFill.fillAmount = progression;
    }
}
