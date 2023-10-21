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
    [field: SerializeField] public EventReference Ambience { get; private set; }

    /*
    [field: Header("Enverenment")]
    [field: SerializeField] public EventReference RoomCleared { get; private set; }

    [field: Header("Player")]
    [field: SerializeField] public EventReference PlayerStepSound { get; private set; }
    [field: SerializeField] public EventReference ShootSound { get; private set; }

    [field: Header("UI")]
    [field: SerializeField] public EventReference ButtonPress { get; private set; }
    */

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
        /*

        EventInstancesDict.Add("MusicMenu", CreateInstance(MusicMenu));
        EventInstancesDict.Add("MusicGame", CreateInstance(MusicGame));
        EventInstancesDict.Add("Ambience", CreateInstance(Ambience));

        EventInstancesDict.Add("PlayerStepSound", CreateInstance(PlayerStepSound));
        EventInstancesDict.Add("ShootSound", CreateInstance(ShootSound));

        EventInstancesDict.Add("RoomCleared", CreateInstance(RoomCleared));
        EventInstancesDict.Add("ButtonPress", CreateInstance(ButtonPress));
        */
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


    public void PlayOneShot(string sound)
    {
        EventInstancesDict[sound].start();
    }


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

    public void ToggleSound(bool val)
    {
        RuntimeManager.GetBus("bus:/").setVolume(val ? 1 : 0);
    }
}