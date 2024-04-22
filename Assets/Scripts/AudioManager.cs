using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class AudioManager : MonoBehaviour{

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] soundSources;

    [SerializeField] private AudioClip[] soundClips;


    private void Awake() {
        //if(Instance == null) {
            Instance = this;
        //}
        
    }

    public void PlaySound(int id) {
        if (id >= 0 && id < soundSources.Length) {
            for (int i = 0; i < soundSources.Length; i++) {
                if (!soundSources[i].isPlaying) {
                    soundSources[i].clip = soundClips[id];
                    //soundSources[i].clip = soundClips[CurrentSound.Select];
                    soundSources[i].Play();
                    break;
                }
            }
        }
    }


}


public enum CurrentSound {
    None,
    Select,
    Win
}