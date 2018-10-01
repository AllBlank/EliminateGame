using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipsManager  {

    string[] clipsName;//音乐名字
    SingleClip[]singleClips;//存放singleclip的数组

    public ClipsManager()
    {
        Initial();
    } 
    
	
	void Initial () {
        //一般从配置文件读取txt
        clipsName = new string[] { "BubbleButton", "Button", "ButtonLight", "Close", "Lose","Magical","Magical02","Plop", "Rain", "Thunder", "Win" };
        singleClips = new SingleClip[clipsName .Length ];
        for (int i = 0; i < clipsName.Length ; i++)
        {
            AudioClip tmpClip=   Resources.Load<AudioClip>(clipsName[i]);
            //把Load到的clip添加到singleClip里面
            SingleClip tmpsingleClip = new SingleClip(tmpClip);
            //再存到数组里面
            singleClips[i]=tmpsingleClip;
        }

    }
	/// <summary>
    /// 提供一个接口获取clip
    /// 通过名字获取音乐片段
    /// </summary>
   public SingleClip GetClips(string audioName)
    {
        int index = -1;//标志位
        for (int i = 0; i < clipsName.Length ; i++)
        {
            if (audioName .Equals (clipsName[i]))
            {
                index = i;
            }
            
        }
        if (index != -1)
        {
            return singleClips[index];
        }
        else
        {
            return null;
        }
    }
	
}
