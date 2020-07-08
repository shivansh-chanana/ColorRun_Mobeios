using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;

[System.Serializable]
public struct SoundList {
    public string soundClipName;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour
{
    public List<SoundList> soundLists;

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySfx(int audioNum) {
        EazySoundManager.PlaySound(soundLists[audioNum].audioClip);
    }

    public void PlayMusic() {
        EazySoundManager.PlayMusic(soundLists[4].audioClip , 0.2f,true , true);
    }
}
