using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldCtr : UIbase
{


    Button boom;
    Button addSpecial;
    Button reset;
    Button addTime;


    Text boomText;
    Text addSpecialText;
    Text resetText;
    Text addTimeText;


    List<int> info;

    private void Awake()
    {
        boom = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        addSpecial = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        reset = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        addTime = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();

        boomText = boom.transform.GetChild(0).GetComponent<Text>();
        addSpecialText = addSpecial.transform.GetChild(0).GetComponent<Text>();
        resetText = reset.transform.GetChild(0).GetComponent<Text>();
        addTimeText = addTime.transform.GetChild(0).GetComponent<Text>();

        info = new List<int>();
    }

    // Use this for initialization
    void Start()
    {
        Ctro c = new Ctro();
        AddButtonListen("Bg_n", c.Bg_n);

        List<ArrayList> tempList = SqlitePropDataManager.Instance.AllDataExcuteASC("PlayerPropTable");
        foreach (var item in tempList[0])
        {
            info.Add(System.Convert.ToInt32(item.ToString()));
        }

        boom.onClick.AddListener(BoomClick);
        addSpecial.onClick.AddListener(SpecialClick);
        reset.onClick.AddListener(ResetClick);
        addTime.onClick.AddListener(TimeClick);

        boomText.text = info[1].ToString();
        addSpecialText.text = info[3].ToString();
        resetText.text = info[4].ToString();
        addTimeText.text = info[2].ToString();
    }
    

    // Update is called once per frame
    void Update()
    {

    }


    void BoomClick()
    {
        if (info[1] > 0)
        {
            SqlitePropDataManager.Instance.UpdateBoomData("PlayerPropTable", --info[1]);
            boomText.text = info[1].ToString();
            ItemsManager.instance.BoomSpecial();
        }
    }
    void SpecialClick()
    {
        if (info[3] > 0)
        {
            SqlitePropDataManager.Instance.UpdateSpecialData("PlayerPropTable", --info[3]);
            addSpecialText.text = info[3].ToString();
            ItemsManager.instance.RandomChangeItemType(4);
        }
    }
    void ResetClick()
    {
        if (info[4] > 0)
        {
            SqlitePropDataManager.Instance.UpdateRefreshData("PlayerPropTable", --info[4]);
            resetText.text = info[4].ToString();
            ItemsManager.instance.ResetPosition();
        }
    }
    void TimeClick()
    {
        if (info[2] > 0)
        {
            SqlitePropDataManager.Instance.UpdateTimeData("PlayerPropTable", --info[2]);
            addTimeText.text = info[2].ToString();
            if( PlayerManager.instance.gameType == GameType.Step)
            {
                PlayerManager.instance.levelInfo.PassSteps += 3;
            }
            else
            {
                PlayerManager.instance.levelInfo.PassTimes += 10;
            }
        }
    }

    public class Ctro
    {
       
        GameObject tmpUI = GameObject.Find("Background");
       
        /// <summary>
        /// 暂停界面切换
        /// </summary>
        public void Bg_n()
        {
            
                Time.timeScale = 0;
                tmpUI.transform.Find("Pause").gameObject.SetActive(true);
                
        }
       
    }
}
