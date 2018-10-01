using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
    public static AudioPlayer instance;
    private void Awake()
    {
        instance = this;
        clipManager = new ClipsManager();
        audioSManager = new AudioSourceManager(gameObject );

    }

    ClipsManager clipManager;
    AudioSourceManager audioSManager;

    //实现播放
    public void Play(string audioName)
    {
       SingleClip tmpSingleClip = clipManager.GetClips(audioName);//通过参数获取音乐名字

        AudioSource tmpSource = audioSManager.GetFreeAudioS();//获取空闲的audiosource
        if (tmpSingleClip != null)
        {
            tmpSingleClip.Play(tmpSource);//实现播放
        }
        else Debug.LogError("clip is null");
        audioSManager.FreeAudioS();//
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void StopPlay(string clipName)
    {
       AudioSource tmpAudioS= audioSManager.GetPlayAudioS(clipName);
        tmpAudioS.Stop();
    }
    /// <summary>
    /// 
    /// </summary>
    public void StopAllAudioS()
    {
        audioSManager.StopAll();
    }

}
