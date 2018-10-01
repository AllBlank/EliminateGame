using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCtr : UIbase
{
    GameObject tmpUI;

    Text coins;

    List<int> info;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        info = new List<int>();
        tmpUI = GameObject.Find("UI");
        coins = transform.Find("Coins").GetComponent<Text>();
        AddButtonListen("StoreBack_n", OnStoreBack);
        List<ArrayList> tempList = SqlitePropDataManager.Instance.AllDataExcuteASC("PlayerPropTable");
        foreach (var item in tempList[0])
        {
            info.Add(System.Convert.ToInt32(item.ToString()));
        }
            
        coins.text = info[0].ToString();

        AddButtonListen("buy1_n", OnBuy1Click);
        AddButtonListen("buy2_n", OnBuy2Click);
        AddButtonListen("buy3_n", OnBuy3Click);
        AddButtonListen("buy4_n", OnBuy4Click);
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    /// <summary>
    /// Play场景加载
    /// </summary>
    public void OnPlayClick()
    {

    }
    /// <summary>
    /// 商店切换
    /// </summary>    
    public void OnStoreBack()
    {
        tmpUI.transform.Find("Store").gameObject.SetActive(false);
    }
    /// <summary>
    /// 购买技能事件回调
    /// </summary>
    public void OnBuy1Click()
    {
        if (info[0] >= 100)
        {
            info[0] -= 100;
            SqlitePropDataManager.Instance.UpdateCoinData("PlayerPropTable", info[0]);
            SqlitePropDataManager.Instance.UpdateBoomData("PlayerPropTable", ++info[1]);
            coins.text = info[0].ToString();
        }
        
    }
    public void OnBuy2Click()
    {
        if (info[0] >= 100)
        {
            info[0] -= 100;
            SqlitePropDataManager.Instance.UpdateCoinData("PlayerPropTable", info[0]);
            SqlitePropDataManager.Instance.UpdateSpecialData("PlayerPropTable", ++info[2]);
            coins.text = info[0].ToString();
        }
    }
    public void OnBuy3Click()
    {
        if (info[0] >= 100)
        {
            info[0] -= 100;
            SqlitePropDataManager.Instance.UpdateCoinData("PlayerPropTable", info[0]);
            SqlitePropDataManager.Instance.UpdateRefreshData("PlayerPropTable", ++info[3]);
            coins.text = info[0].ToString();
        }
    }
    public void OnBuy4Click()
    {
        if (info[0] >= 100)
        {
            info[0] -= 100;
            SqlitePropDataManager.Instance.UpdateCoinData("PlayerPropTable", info[0]);
            SqlitePropDataManager.Instance.UpdateTimeData("PlayerPropTable", ++info[4]);
            coins.text = info[0].ToString();
        }
    }
}
