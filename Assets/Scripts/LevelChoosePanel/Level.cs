using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  关卡选择脚本 
 *  
 *          获取关卡选择的点击事件
 */
public class Level : MonoBehaviour {

    public int starNum;
    public int scores;

    private Button UnplayBtn;
    private Button playedBtn;

    private GameObject choose;

    private void Start()
    {
        choose = GameObject.Find("UI").transform.Find("Choose").gameObject;
        playedBtn = transform.GetChild(1).GetComponent<Button>();
        UnplayBtn = transform.GetChild(2).GetChild(2).GetComponent<Button>();
        UnplayBtn.onClick.AddListener(ChooseLevelBtnOnClick);
        playedBtn.onClick.AddListener(ChooseLevelBtnOnClick);
    }

    public void ChooseLevelBtnOnClick()
    {   
        //将当前选择的关卡存入关卡管理单例中
        LevelsManager.Instance.currentChooseLevelNum = int.Parse(playedBtn.transform.GetChild(0).GetComponent<Text>().text);
        //TODO:选择关卡
        Debug.Log(LevelsManager.Instance.currentChooseLevelNum);
        choose.SetActive(true);
    }


}
