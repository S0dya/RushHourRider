using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] CanvasGroup introCG;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoStopped;
    }

    void OnVideoStopped(VideoPlayer source)
    {
        GameManager.I.Close(introCG, 0.1f);
        LoadingSceneManager.I.StartGame();
    }
}
