using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClip  {


    AudioClip audioClip;

    AudioSource audioSource;
    /// <summary>
    /// 在构造函数初始化audioClip
    /// </summary>
    /// <param name="tmpClip">参数</param>
    public SingleClip(AudioClip tmpClip)
    {
        audioClip = tmpClip;
    }
    

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="tmpAudio"></param>
    public void Play(AudioSource tmpAudio)
    {
        audioSource = tmpAudio;
        audioSource.clip = audioClip;
        audioSource.Play();
    }
   
	
}
