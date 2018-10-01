using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAudioPlayer : MonoBehaviour {

	public void PlaySounds1()
    {
        AudioPlayer.instance.Play("BubbleButton");
    }
    public void PlaySounds2()
    {
        AudioPlayer.instance.Play("Button");
    }
    public void PlaySounds3()
    {
        AudioPlayer.instance.Play("ButtonLight");
    }
    public void PlaySounds4()
    {
        AudioPlayer.instance.Play("Close");
    }
    public void PlaySounds5()
    {
        AudioPlayer.instance.Play("Lose");
    }
    public void PlaySounds6()
    {
        AudioPlayer.instance.Play("Magical");
    }
    public void PlaySounds7()
    {
        AudioPlayer.instance.Play("Magical02");
    }
    public void PlaySounds8()
    {
        AudioPlayer.instance.Play("Plop");
    }
    public void PlaySounds9()
    {
        AudioPlayer.instance.Play("Rain");
    }
    public void PlaySounds10()
    {
        AudioPlayer.instance.Play("Thunder");
    }
    public void PlaySounds11()
    {
        AudioPlayer.instance.Play("Win");
    }



    /// <summary>
    /// 关闭所有特效
    /// </summary>
    public void StopSounds()
    {
        AudioPlayer.instance.StopAllAudioS();
    }
}
