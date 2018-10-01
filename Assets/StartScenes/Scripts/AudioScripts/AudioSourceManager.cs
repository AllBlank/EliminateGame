using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager  {

    List<AudioSource> allSources;

    GameObject owner;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initial()
    {
        allSources = new List<AudioSource>();
        for (int i = 0; i < 4; i++)
        {
           AudioSource tmpSources= owner.AddComponent<AudioSource>();
            allSources.Add(tmpSources);
        }
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="obj"></param>
    public AudioSourceManager(GameObject obj)
    {
        owner = obj;
        Initial();
        
    }
    /// <summary>
    /// 获取一个空闲的AudioSources
    /// </summary>
    /// <returns></returns>
    public AudioSource GetFreeAudioS()
    {
        for (int i = 0; i < allSources .Count ; i++)
        {
            if (!allSources [i].isPlaying )
            {
                return allSources[i];
            }
        }
        AudioSource tmpSources = owner.AddComponent<AudioSource>();
        allSources.Add(tmpSources);
        return tmpSources;
    }
    /// <summary>
    /// 销毁多余的AudioSources
    /// </summary>
    public void FreeAudioS()
    {

        List<AudioSource> indexList = new List<AudioSource>();//记录空闲的audiosources
        
        for (int i = 0; i < allSources.Count ; i++)
        {
            if (!allSources [i].isPlaying )
            {             
                indexList.Add(allSources [i]);
            }
        }
        if (indexList.Count >3)
        {
            for (int i = 3; i < indexList.Count; i++)
            {
                allSources.Remove(indexList[i]);//清除空闲的audiosources
            }
        }
        
    }
    public void FreeeAudioSources()
    {
        int tmpCount = 0;
        for (int i = 0; i < allSources .Count ; i++)
        {
            AudioSource tmpSources = allSources[i];
            if (!tmpSources.isPlaying )
            {
                tmpCount++;
                if (tmpCount >3)
                {
                    allSources.Remove(tmpSources);
                }
            }
        }
    }
    /// <summary>
    /// 关闭播放的音乐
    /// </summary>
    /// <param name="clipName">音乐片段名字</param>
    /// <returns>返回AudioSource</returns>
    public AudioSource GetPlayAudioS(string clipName)
    {
        for (int i = 0; i < allSources.Count ; i++)
        {
            if (allSources [i].isPlaying )
            {
                if (allSources[i].clip .name.Equals(clipName))
                {
                    return allSources[i];
                }
            }

        }
        return null;
    }
    public void StopAll()
    {
        for (int i = 0; i < allSources .Count ; i++)
        {

            allSources[i].Stop ();
            
        }
       
    }
}
