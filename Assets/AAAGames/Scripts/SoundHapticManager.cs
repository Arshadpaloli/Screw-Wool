using System;
using UnityEngine;
public class SoundHapticManager : MonoBehaviour
{
    public static SoundHapticManager instance;
    public Sounds[] sounds;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetupAudioSources();
    }
    private void Start()
    {
        Vibration.Init();
        //PlayAudioWithOutVibration("BG");
    }

    private void SetupAudioSources()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].GetRead(gameObject.AddComponent<AudioSource>());
        }
    }
    public void PlayAudio(string SoundName, int VIBRATION = 20)
    {
        var s = Array.Find(sounds, sound => sound.name == SoundName);
        s.Play();
        Vibration.VibrateAndroid(VIBRATION);
    }
    public void PlayAudioWithOutVibration(string SoundName)
    {
        /*if (SavedData.GetSoundState().Equals("Off"))
            return;*/

        Sounds s = Array.Find(sounds, sound => sound.name == SoundName);
        s.Play();
    }
    public void StopAudio(string SoundName)
    {
        /*if (SavedData.GetSoundState().Equals("Off"))
  			return;
  			*/
        Sounds s = Array.Find(sounds, sound => sound.name == SoundName);
        s.Stop();
    }
    public void Audio_On()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].AudioSourceState(true);
        }
    }
    public void Audio_Off()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].AudioSourceState(false);
        }
    }
}


