using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSelector : MonoBehaviour
{
    public AudioManager.BGMtype bgmType = AudioManager.BGMtype.None;
    public AudioManager.SFXtype sfxType = AudioManager.SFXtype.None;

    public void PlaySound()
    {
        if (bgmType != AudioManager.BGMtype.None)
            AudioManager.script.Play(bgmType);

        if (sfxType != AudioManager.SFXtype.None)
            AudioManager.script.PlayMultiple(sfxType);
    }
}
