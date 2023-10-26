using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    bool calmMusicIsCurrentlyPlaying;

    Coroutine fadeOutCoroutine;
    Coroutine randomSFXCor;

    [field: Header("Music")]
    [field: SerializeField] public EventReference MusicMenu { get; private set; }
    [field: SerializeField] public EventReference MusicGame { get; private set; }
    [field: SerializeField] public EventReference MusicGameMenu { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference ButtonUI { get; private set; }
    [field: SerializeField] public EventReference BoostPickUp { get; private set; }
    [field: SerializeField] public EventReference Crash { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();
    }

    void Start()
    {
        ToggleSound(Settings.isMusicOn);

        EventInstancesDict.Add("MusicMenu", CreateInstance(MusicMenu));
        EventInstancesDict.Add("MusicGame", CreateInstance(MusicGame));
        EventInstancesDict.Add("MusicGameMenu", CreateInstance(MusicGameMenu));

        EventInstancesDict.Add("ButtonUI", CreateInstance(ButtonUI));
        EventInstancesDict.Add("BoostPickUp", CreateInstance(BoostPickUp));
        EventInstancesDict.Add("Crash", CreateInstance(Crash));
    }

    public void SetParameter(string instanceName, string parameterName, float value)
    {
        EventInstancesDict[instanceName].setParameterByName(parameterName, value);
    }
    public void SetParameterWithCheck(string instanceName, string parameterName, float newValue)
    {
        float currentParameterValue;
        EventInstancesDict[parameterName].getParameterByName(parameterName, out currentParameterValue);

        if (currentParameterValue != newValue)
        {
            EventInstancesDict[parameterName].setParameterByName(parameterName, newValue);
        }
    }


    public void PlayOneShot(string sound) => EventInstancesDict[sound].start();


    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }

    public void ToggleSound() => ToggleSound(Settings.isMusicOn);
    public void ToggleSound(bool val) => RuntimeManager.GetBus("bus:/").setVolume(val ? 1 : 0);
    public void ToggleGameMusic(bool val) => RuntimeManager.GetBus("bus:/MusicGame").setVolume(val ? 1 : 0);
}