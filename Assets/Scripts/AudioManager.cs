using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class AudioManager : MonoBehaviour{


    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] soundSources;

    [SerializeField] private AudioClip[] soundClips, musicCllips;

    public void PlaySound(int id) {
        if (UIManager.isSoundsEnabled) {
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

    public void PlayMusic(int id) {
        if (UIManager.isMusicEnabled) {
            if (id >= 0 && id < musicCllips.Length) {
                musicSource.Stop();
                musicSource.clip = musicCllips[id];
                musicSource.Play();
            }
        }
    }

    public void StopMusic() {
        musicSource.Stop();
    }

}


public enum CurrentSound {
    None,
    Select,
    Win
}