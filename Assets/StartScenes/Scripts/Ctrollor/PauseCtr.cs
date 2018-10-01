using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCtr : UIbase  {
    
    public class Ctro
    {

        GameObject tmpUI = GameObject.Find("Background");



        public void OnResumeClick()
        {
            Time.timeScale = 1;
            tmpUI.transform.Find("Pause").gameObject.SetActive(false);
        }
        /// <summary>
        /// 重新开始游戏
        /// </summary>
        public void OnRestartClick()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameScene" + LevelsManager.Instance.currentChooseLevelNum);
        }
        /// <summary>
        /// 退出
        /// </summary>
        public void OnQuitClick()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MenuScene");
        }
        bool isMusic = true;
       // Transform AudiosManager = GameObject.Find("AudioS").transform;

        public void OnMusicClick()
        {
            //if (isMusic )
            //{
            //    AudiosManager.GetComponent<AudioSource>().enabled = false;
            //    isMusic = false;
            //}
            //else
            //{
            //    AudiosManager.GetComponent<AudioSource>().enabled = true ;
            //    isMusic = true;
            //}
        }
        public void OnSoundsClick()
        {

        }
    }
        // Use this for initialization
        void Start () {
        Ctro c = new Ctro();
        AddButtonListen("Music_n", c.OnMusicClick);
        AddButtonListen("Resume_n", c.OnResumeClick);
        AddButtonListen("Restart_n", c.OnRestartClick);
        AddButtonListen("Quit_n", c.OnQuitClick);
	}
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
