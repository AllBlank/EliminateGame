using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 方向
/// </summary>
public enum Dir
{
    Up,
    Down,
    Right,
    Left,
    No  //无方向
}

/// <summary>
/// 要生成的特殊Item
/// </summary>
public struct BuildItem
{
    public int row;
    public int col;
    public ItemType type;
    public Sprite sprite;
}

public class ItemsManager : MonoBehaviour
{
    //单例
    public static ItemsManager instance;

    public AudioClip buttonClick;
    public AudioClip clearClick;
    public AudioClip UnClearClick;
    public AudioClip specialClear;

    private AudioSource aud;

    /// <summary>
    /// 是否可以执行交换
    /// </summary>
    public bool canChange = false;

    /// <summary>
    /// 总行数
    /// </summary>
    public int row = 8;
    /// <summary>
    /// 总列数
    /// </summary>
    public int col = 7;

    /// <summary>
    /// Item坐标二维数组
    /// </summary>
    public Vector3[,] itemsPosition;

    /// <summary>
    /// Item脚本二维数组
    /// </summary>
    public SimpleItem[,] itemsScript;

    /// <summary>
    /// Item交换灵敏度
    /// </summary>
    public float changeSensitivity = 30f;

    /// <summary>
    /// 图片数组
    /// </summary>
    public Sprite[] sprites;

    //移动动画时间
    public float duration = 0.2f;

    /// <summary>
    /// 能消除的Item列表
    /// </summary>
    public List<SimpleItem> canClearList;

    public List<Tweener> canClearListTweener;

    /// <summary>
    /// 要交换的Item
    /// </summary>
    private SimpleItem item1;
    /// <summary>
    /// 被交换的Item
    /// </summary>
    private SimpleItem item2;
    /// <summary>
    /// 要交换的Item的相同Item列表
    /// </summary>
    private List<SimpleItem> item1SameList;
    /// <summary>
    /// 被交换的Item的相同Item列表
    /// </summary>
    private List<SimpleItem> item2SameList;
    /// <summary>
    /// 消除Item列表
    /// </summary>
    private List<SimpleItem> boomList;

    /// <summary>
    /// 要生成的特殊Item列表
    /// </summary>
    private List<BuildItem> buildItems;

    /// <summary>
    /// 连击数
    /// </summary>
    private int combo = 0;

    private void Awake()
    {   
        aud = GameObject.Find("Canvas").GetComponent<AudioSource>();
        //初始化
        DOTween.Init(true, true, LogBehaviour.Default);
        instance = this;
        buildItems = new List<BuildItem>();
        item1SameList = new List<SimpleItem>();
        item2SameList = new List<SimpleItem>();
        canClearList = new List<SimpleItem>();
        boomList = new List<SimpleItem>();
        canClearListTweener = new List<Tweener>();
        itemsPosition = new Vector3[row, col];
        itemsScript = new SimpleItem[row, col];

    }

    private void Start()
    {
        int count = transform.childCount;
        Transform[] temps = new Transform[count];
        //获取所有Item的Transform
        for (int i = 0; i < count; i++)
        {
            temps[i] = transform.GetChild(i);
        }

        //temps数组索引
        int index = 0;

        //给二维数组赋值
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //获取Item显示用的图片组件
                Image img = temps[index].GetChild(1).GetComponent<Image>();
                //随机获取图片
                img.sprite = sprites[Random.Range(0, sprites.Length)];
                //将图片设为可见
                img.color = new Color(1, 1, 1, 1);
                //获取Item脚本
                SimpleItem item = temps[index].GetComponent<SimpleItem>();
                //设置行列号
                item.col = j;
                item.row = i;
                //坐标数组赋值
                itemsPosition[i, j] = temps[index].position;
                //Item脚本数组赋值
                itemsScript[i, j] = temps[index].GetComponent<SimpleItem>();
                //索引自增
                index++;
            }
        }
        //自动消除
        AutoClear();
    }

    /// <summary>
    /// 获取交换Item的方向
    /// </summary>
    /// <param name="x">鼠标横轴移动值</param>
    /// <param name="y">鼠标纵轴移动的值</param>
    /// <returns></returns>
    public Dir ChangeItemDir(float x, float y)
    {
        //如果横轴移动大于纵轴
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            //如果横轴移动距离小于灵敏度设置
            if (Mathf.Abs(x) < changeSensitivity) return Dir.No;
            //如果横轴移动大于0
            else if (x > 0)
            {
                return Dir.Right;
            }
            //如果横轴移动小于0
            else
            {
                return Dir.Left;
            }
        }
        //如果纵轴移动大于横轴
        else
        {
            //如果纵轴移动距离小于灵敏度设置
            if (Mathf.Abs(y) < changeSensitivity) return Dir.No;
            //如果纵轴移动大于0
            else if (y > 0)
            {
                return Dir.Up;
            }
            //如果纵轴移动小于0
            else
            {
                return Dir.Down;
            }
        }
    }

    /// <summary>
    /// 交换Item位置
    /// </summary>
    /// <param name="item">当前Item脚本</param>
    /// <param name="targetRow">要交换item的row</param>
    /// <param name="targetCol">要交换item的col</param>
    public void ChangeItem(SimpleItem item, int targetRow, int targetCol)
    {
        if (!canChange) return;
        //如果行列数不符合要求,直接Return
        if (targetRow < 0 || targetRow >= row || targetCol < 0 || targetCol >= col) return;
        //获取要交换的Item的脚本
        SimpleItem targetItem = itemsScript[targetRow, targetCol];
        if (targetItem == null) return;
        //如果要交换的Item非激活,直接Return
        if (!targetItem.gameObject.activeSelf) return;
        //将可以操作的标志位设为False
        canChange = false;
        Debug.Log("Set False");
        //将要交换和被交换的Item赋值
        item1 = item;
        item2 = targetItem;
        //交换Item脚本数组中的位置
        itemsScript[targetRow, targetCol] = item;
        itemsScript[item.row, item.col] = targetItem;
        //交换脚本行列坐标
        int tempRow = item.row;
        int tempCol = item.col;
        item.row = targetItem.row;
        item.col = targetItem.col;
        targetItem.row = tempRow;
        targetItem.col = tempCol;
        if (item1.type != ItemType.Normal && item2.type != ItemType.Normal)
        {
            boomList.Add(item1);
            boomList.Add(item2);
        }
        //交换位置
        Vector3 temp = item.transform.position;
        item.transform.DOMove(targetItem.transform.position, duration);

        targetItem.transform.DOMove(temp, duration).OnComplete(()=> { CheckBoom();});
        aud.clip = buttonClick;
        aud.Play();
        //交换完成后,进行消除检测
        Debug.Log("BeginMove");
    }

    /// <summary>
    /// 检测是否能够消除
    /// </summary>
    public void CheckBoom()
    {
        //将交换的Item加入相同列表
        item1SameList.Add(item1);
        item2SameList.Add(item2);
        //检测周围Item是否相同
        CheckAroundItem(item1, item1SameList);
        CheckAroundItem(item2, item2SameList);
        //检测相同列表里Item是否相连,如果是则加入消除列表
        CheckLink(item1SameList);
        CheckLink(item2SameList);

        //如果无法消除
        if (boomList.Count == 0)
        {
            aud.clip = UnClearClick;
            aud.Play();
            //交换回原来位置
            //交换Item脚本数组中的位置
            itemsScript[item1.row, item1.col] = item2;
            itemsScript[item2.row, item2.col] = item1;
            //交换脚本行列坐标
            int tempRow = item1.row;
            int tempCol = item1.col;
            item1.row = item2.row;
            item1.col = item2.col;
            item2.row = tempRow;
            item2.col = tempCol;
            //交换位置
            Vector3 temp = item1.transform.position;
            item1.transform.DOMove(item2.transform.position, duration);
            item2.transform.DOMove(temp, duration).OnComplete(() =>
            {
                canChange = true;
            });
        }
        //如果可以消除
        else
        {
            aud.clip = clearClick;
            aud.Play();
            if (PlayerManager.instance.gameType == GameType.Step)
            {
                PlayerManager.instance.playerStep++;
            }
            //清除消除列表的相同元素
            boomList = RemoveListSame(boomList);
            //消除
            BoomItem();
        }
        //清空列表
        boomList.Clear();
        item1SameList.Clear();
        item2SameList.Clear();
        
    }

    #region 相同Item检测

    /// <summary>
    /// 检测附近所有相同的Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    public void CheckAroundItem(SimpleItem item, List<SimpleItem> list)
    {
        CheckUpItem(item, list);
        CheckDownItem(item, list);
        CheckLeftItem(item, list);
        CheckRightItem(item, list);
    }

    /// <summary>
    /// 递归检测上方Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    public void CheckUpItem(SimpleItem item, List<SimpleItem> list)
    {
        if (item.row - 1 < 0) return;
        SimpleItem upItem = itemsScript[item.row - 1, item.col];
        if (upItem == null) return;
        if (!upItem.gameObject.activeSelf) return;
        if (upItem.transform.GetChild(1).GetComponent<Image>().sprite.name == item.transform.GetChild(1).GetComponent<Image>().sprite.name)
        {
            if (!list.Contains(upItem))
                list.Add(upItem);
            CheckUpItem(upItem, list);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 递归检测下方Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    public void CheckDownItem(SimpleItem item, List<SimpleItem> list)
    {
        if (item.row + 1 >= row) return;
        SimpleItem downItem = itemsScript[item.row + 1, item.col];
        if (downItem == null) return;
        if (!downItem.gameObject.activeSelf) return;
        if (downItem.transform.GetChild(1).GetComponent<Image>().sprite.name == item.transform.GetChild(1).GetComponent<Image>().sprite.name)
        {
            if (!list.Contains(downItem))
                list.Add(downItem);
            CheckDownItem(downItem, list);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 递归检测左方Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    public void CheckLeftItem(SimpleItem item, List<SimpleItem> list)
    {
        if (item.col - 1 < 0) return;
        SimpleItem leftItem = itemsScript[item.row, item.col - 1];
        if (leftItem == null) return;
        if (!leftItem.gameObject.activeSelf) return;
        if (leftItem.transform.GetChild(1).GetComponent<Image>().sprite.name == item.transform.GetChild(1).GetComponent<Image>().sprite.name)
        {
            if (!list.Contains(leftItem))
                list.Add(leftItem);
            CheckLeftItem(leftItem, list);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 递归检测右方Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="list"></param>
    public void CheckRightItem(SimpleItem item, List<SimpleItem> list)
    {
        if (item.col + 1 >= col) return;
        SimpleItem rightItem = itemsScript[item.row, item.col + 1];
        if (rightItem == null) return;
        if (!rightItem.gameObject.activeSelf) return;
        if (rightItem.transform.GetChild(1).GetComponent<Image>().sprite.name == item.transform.GetChild(1).GetComponent<Image>().sprite.name)
        {
            if (!list.Contains(rightItem))
                list.Add(rightItem);
            CheckRightItem(rightItem, list);
        }
        else
        {
            return;
        }
    }

    #endregion

    /// <summary>
    /// 检测列表Item相连长度,达到标准则加入消除列表
    /// </summary>
    /// <param name="sameList"></param>
    public void CheckLink(List<SimpleItem> sameList)
    {
        //同行Item
        List<SimpleItem> rowList = new List<SimpleItem>();
        //同列Item
        List<SimpleItem> colList = new List<SimpleItem>();
        //同列Item的列数
        int colNum = sameList[0].col;
        //同行Item的行数
        int rowNum = sameList[0].row;
        //遍历相同Item列表
        foreach (var item in sameList)
        {
            //如果行数相同
            if (item.row == rowNum)
            {
                //加入同行列表
                rowList.Add(item);
            }
            //如果列数相同
            if (item.col == colNum)
            {
                //加入同列列表
                colList.Add(item);
            }
        }
        if(colList.Count >= 3 && rowList.Count >= 3)
        {
            BuildItem buildItem = new BuildItem();
            buildItem.col = sameList[0].col;
            buildItem.row = sameList[0].row;
            buildItem.type = ItemType.ClearRange;
            buildItem.sprite = sameList[0].transform.GetChild(1).GetComponent<Image>().sprite;
            buildItems.Add(buildItem);
        }
        //如果同行列表
        if (colList.Count >= 3)
        {
            if(colList.Count == 4)
            {
                BuildItem buildItem = new BuildItem();
                buildItem.col = sameList[0].col;
                buildItem.row = sameList[0].row;
                buildItem.type = ItemType.ClearCol;
                buildItem.sprite = sameList[0].transform.GetChild(1).GetComponent<Image>().sprite;
                buildItems.Add(buildItem);
            }
            if (colList.Count >= 5)
            {
                BuildItem buildItem = new BuildItem();
                buildItem.col = sameList[0].col;
                buildItem.row = sameList[0].row;
                buildItem.type = ItemType.ClearSame;
                buildItem.sprite = sameList[0].transform.GetChild(1).GetComponent<Image>().sprite;
                buildItems.Add(buildItem);
            }
            boomList.AddRange(colList);
        }
        if (rowList.Count >= 3)
        {
            if(rowList.Count == 4)
            {
                BuildItem buildItem = new BuildItem();
                buildItem.col = sameList[0].col;
                buildItem.row = sameList[0].row;
                buildItem.type = ItemType.ClearRow;
                buildItem.sprite = sameList[0].transform.GetChild(1).GetComponent<Image>().sprite;
                buildItems.Add(buildItem);
            }
            if (rowList.Count >= 5)
            {
                BuildItem buildItem = new BuildItem();
                buildItem.col = sameList[0].col;
                buildItem.row = sameList[0].row;
                buildItem.type = ItemType.ClearSame;
                buildItem.sprite = sameList[0].transform.GetChild(1).GetComponent<Image>().sprite;
                buildItems.Add(buildItem);
            }
            boomList.AddRange(rowList);
        }
    }

    /// <summary>
    /// 消除列表里的相同元素
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    public List<SimpleItem> RemoveListSame(List<SimpleItem> list)
    {
        List<SimpleItem> newList = new List<SimpleItem>();
        foreach (var item in list)
        {
            if (!newList.Contains(item))
            {
                newList.Add(item);
            }
        }
        return newList;
    }


    /// <summary>
    /// 消除BoomList里的Item
    /// </summary>
    public void BoomItem()
    {
        if (boomList.Count != 0)
        {
            aud.clip = clearClick;
            aud.Play();
        }
        isdown = false;
        //遍历消除列表
        for (int i = 0; i < boomList.Count; i++)
        {
            //获取消除Item的脚本
            SimpleItem item = boomList[i];
            if (item != itemsScript[item.row, item.col]) continue;
            if(item.type == ItemType.Normal)
            {
                ClearItem(item);
            }
            else if(item.type == ItemType.ClearRow)
            {
                for (int j = 0; j < col; j++)
                {
                    SimpleItem clearItem = itemsScript[item.row, j];
                    if(clearItem != null && clearItem.gameObject.activeSelf)
                    {
                        ClearItem(clearItem);
                    }                   
                }
            }
            else if(item.type == ItemType.ClearCol)
            {
                for (int j = 0; j < row; j++)
                {
                    SimpleItem clearItem = itemsScript[j, item.col];
                    if (clearItem != null && clearItem.gameObject.activeSelf)
                    {
                        ClearItem(clearItem);
                    }
                }
            }
            else if(item.type == ItemType.ClearCross)
            {
                for (int j = 0; j < col; j++)
                {

                    SimpleItem clearItem = itemsScript[item.row, j];
                    if (clearItem != null && clearItem.gameObject.activeSelf)
                    {
                        ClearItem(clearItem);
                    }
                }
                for (int j = 0; j < row; j++)
                {
                    SimpleItem clearItem = itemsScript[j, item.col];
                    if (clearItem != null && clearItem.gameObject.activeSelf)
                    {
                        ClearItem(clearItem);
                    }
                }
            }
            else if(item.type == ItemType.ClearRange)
            {
                SimpleItem clearItem;
                for (int j = -1; j <= 1; j++)
                {                   
                    if (item.col + j >=0 && item.col + j< col)
                    {
                        for (int m = -1; m <= 1; m++)
                        {
                            if (item.row + m >= 0 && item.row + m < row)
                            {
                                clearItem = itemsScript[item.row + m, item.col + j];
                                if (clearItem != null && clearItem.gameObject.activeSelf)
                                {
                                    ClearItem(clearItem);
                                }
                            }
                                
                        }
                        
                    }
                }
            }
            else
            {
                for (int j = 0; j < row; j++)
                {
                    for (int m = 0; m < col; m++)
                    {
                        SimpleItem clearItem = itemsScript[j, m];
                        if(clearItem != null && clearItem.gameObject.activeSelf && item.transform.GetChild(1).GetComponent<Image>().sprite.name == clearItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                        {
                            ClearItem(clearItem);
                        }
                    }
                }
            }
            

        }
        canClearList.Clear();
        //aud.clip = buttonClick;
    }

    /// <summary>
    /// 生成BuildItemList里的特殊Item
    /// </summary>
    public void CreateBuildItem()
    {
        foreach (var item in buildItems)
        {
            if(itemsScript[item.row,item.col] == null)
            {
                GameObject newItem = ItemPool.instance.GetItem(transform);
                SimpleItem newItemScript = newItem.GetComponent<SimpleItem>();
                newItemScript.row = item.row;
                newItemScript.col = item.col;
                newItemScript.Type = item.type;
                itemsScript[item.row, item.col] = newItemScript;
                newItem.transform.position = itemsPosition[item.row, item.col];
                newItem.transform.localScale = Vector3.zero;
                newItem.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                newItem.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                newItem.SetActive(true);
                newItem.transform.DOScale(1, duration);
                newItem.GetComponent<Image>().DOFade(1, duration);
                newItem.transform.GetChild(1).GetComponent<Image>().DOFade(1, duration);
                newItem.transform.GetChild(1).GetComponent<Image>().sprite = item.sprite;
            }
            
        }
        buildItems.Clear();
    }


    bool isdown = false;

    /// <summary>
    /// 消除单个Item
    /// </summary>
    /// <param name="item"></param>
    public void ClearItem(SimpleItem item)
    {
        //清空二维数组中当前Item的脚本
        itemsScript[item.row, item.col] = null;
        if (item.type == ItemType.ClearRow)
        {
            aud.clip = specialClear;
            aud.Play();
            for (int j = 0; j < col; j++)
            {
                SimpleItem clearItem = itemsScript[item.row, j];
                if (clearItem != null && clearItem.gameObject.activeSelf)
                {
                    ClearItem(clearItem);
                }
            }
        }
        else if (item.type == ItemType.ClearCol)
        {
            aud.clip = specialClear;
            aud.Play();
            for (int j = 0; j < row; j++)
            {
                SimpleItem clearItem = itemsScript[j, item.col];
                if (clearItem != null && clearItem.gameObject.activeSelf)
                {
                    ClearItem(clearItem);
                }
            }
        }
        else if (item.type == ItemType.ClearCross)
        {
            aud.clip = specialClear;
            aud.Play();
            for (int j = 0; j < col; j++)
            {

                SimpleItem clearItem = itemsScript[item.row, j];
                if (clearItem != null && clearItem.gameObject.activeSelf)
                {
                    ClearItem(clearItem);
                }
            }
            for (int j = 0; j < row; j++)
            {
                SimpleItem clearItem = itemsScript[j, item.col];
                if (clearItem != null && clearItem.gameObject.activeSelf)
                {
                    ClearItem(clearItem);
                }
            }
        }
        else if (item.type == ItemType.ClearRange)
        {
            aud.clip = specialClear;
            aud.Play();

            SimpleItem clearItem;
            for (int j = -1; j <= 1; j++)
            {
                if (item.col + j >= 0 && item.col + j < col)
                {
                    for (int m = -1; m <= 1; m++)
                    {
                        if (item.row + m >= 0 && item.row + m < row)
                        {
                            clearItem = itemsScript[item.row + m, item.col + j];
                            if (clearItem != null && clearItem.gameObject.activeSelf)
                            {
                                ClearItem(clearItem);
                            }
                        }

                    }

                }
            }
        }
        else if(item.type == ItemType.ClearSame)
        {
            aud.clip = specialClear;
            aud.Play();
            for (int j = 0; j < row; j++)
            {
                for (int m = 0; m < col; m++)
                {
                    SimpleItem clearItem = itemsScript[j, m];
                    if (clearItem != null && clearItem.gameObject.activeSelf && item.transform.GetChild(1).GetComponent<Image>().sprite.name == clearItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                    {
                        ClearItem(clearItem);
                    }
                }
            }
        }
        //执行动画
        item.transform.DOShakeScale(0.3f).OnComplete(() =>
        {
            AddSources();
            item.transform.DOScale(0, 0.3f);
            item.GetComponent<Image>().DOFade(0, 0.3f);
            if (isdown)
            {
                item.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() =>
                {
                    //动画完成将Item放入对象池
                    ItemPool.instance.SetItem(item.gameObject);
                });
            }
            else
            {
                item.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() =>
                {
                    //动画完成将Item放入对象池
                    ItemPool.instance.SetItem(item.gameObject);
                    ItemDown();
                });
                isdown = true;
            }
        });
    }

    /// <summary>
    /// 增加积分
    /// </summary>
    public void AddSources()
    {
        PlayerManager.instance.playerScore += (int)(20 * (1 + combo / 10f));
        combo++;
    }

    /// <summary>
    /// 下落算法
    /// </summary>
    public void ItemDown()
    {
        CreateBuildItem();
        for (int i = 0; i < col; i++)
        {
            int lastRow = -1;
            for (int j = row - 1; j >= 0; j--)
            {
                if (itemsScript[j, i] == null)
                {
                    lastRow = j;
                    break;
                }
            }

            if (lastRow != -1)
            {
                for (int j = lastRow; j >= 0; j--)
                {
                    if (itemsScript[j, i] != null)
                    {
                        if (itemsScript[j, i].gameObject.activeSelf)
                        {
                            SimpleItem changeItem = itemsScript[j, i];
                            itemsScript[j, i] = null;
                            itemsScript[lastRow, i] = changeItem;
                            changeItem.row = lastRow;
                            changeItem.transform.DOMove(itemsPosition[lastRow, i], duration);
                            for (int m = lastRow; m >= 0; m--)
                            {
                                if (itemsScript[m, i] == null)
                                {
                                    lastRow = m;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int m = j; m >= 0; m--)
                            {
                                if (itemsScript[m, i] == null)
                                {
                                    j = m - 1;
                                    lastRow = m;
                                    break;
                                }
                                if (m == 0)
                                {
                                    j = -1;
                                }
                            }
                        }
                    }
                }
            }
        }
        StartCoroutine(CreateNewItem());

    }


    /// <summary>
    /// 生成新Item
    /// </summary>
    public IEnumerator CreateNewItem()
    {
        yield return new WaitForSeconds(0.3f);
        Tweener tweener = null;
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (itemsScript[j, i] == null)
                {
                    GameObject newItem = ItemPool.instance.GetItem(transform);
                    SimpleItem newItemScript = newItem.GetComponent<SimpleItem>();
                    newItemScript.row = j;
                    newItemScript.col = i;
                    newItemScript.Type = ItemType.Normal;
                    itemsScript[j, i] = newItemScript;
                    newItem.transform.position = itemsPosition[j, i];
                    newItem.transform.localScale = Vector3.zero;
                    newItem.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    newItem.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    newItem.SetActive(true);
                    newItem.transform.DOScale(1, duration);
                    newItem.GetComponent<Image>().DOFade(1, duration);
                    tweener = newItem.transform.GetChild(1).GetComponent<Image>().DOFade(1, duration);
                    newItem.transform.GetChild(1).GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];

                }
            }

        }
        if (tweener != null)
        {
            tweener.OnComplete(() =>
            {
                AutoClear();
            });
        }
        

    }

    /// <summary>
    /// 自动遍历消除
    /// </summary>
    public void AutoClear()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (itemsScript[i, j] != null)
                {
                    if (!itemsScript[i, j].gameObject.activeSelf) continue;
                }
                if (boomList.Contains(itemsScript[i, j])) continue;
                item1 = itemsScript[i, j];
                item1SameList.Add(item1);
                CheckAroundItem(item1, item1SameList);
                CheckLink(item1SameList);
                item1SameList.Clear();
            }
        }
        //清除消除列表的相同元素
        boomList = RemoveListSame(boomList);
        //消除
        BoomItem();
        if (boomList.Count == 0)
        {
            combo = 0;
            canChange = true;
            if (!CheckCanClear())
            {
                ResetPosition();
            }
        }
        boomList.Clear();
    }

    /// <summary>
    /// 检测可以消除的Item
    /// </summary>
    public bool CheckCanClear()
    {
        List<SimpleItem> dontCheck = new List<SimpleItem>();
        Dir dir;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                for (int m = 1; m <= 2; m++)
                {
                    if (!itemsScript[i, j].gameObject.activeSelf) continue;
                    if (i - m >= 0)
                    {
                        if (itemsScript[i - m, j].gameObject.activeSelf)
                        {
                            if (itemsScript[i, j].transform.GetChild(1).GetComponent<Image>().sprite.name == itemsScript[i - m, j].transform.GetChild(1).GetComponent<Image>().sprite.name)
                            {
                                dontCheck.Clear();
                                dontCheck.Add(itemsScript[i, j]);
                                dontCheck.Add(itemsScript[i - m, j]);
                                if (m == 1)
                                {
                                    if (i - 2 >= 0 && itemsScript[i - 2, j].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i - 2, j], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                else
                                {
                                    if (itemsScript[i - 1, j].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i - 1, j], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                if (dir != Dir.No)
                                {
                                    if (m == 1)
                                    {
                                        AddToCanClearList(itemsScript[i - 2, j], dir, dontCheck);
                                    }
                                    else
                                    {
                                        AddToCanClearList(itemsScript[i - 1, j], dir, dontCheck);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                    if (i + m < row)
                    {
                        if (itemsScript[i + m, j].gameObject.activeSelf)
                        {
                            if (itemsScript[i, j].transform.GetChild(1).GetComponent<Image>().sprite.name == itemsScript[i + m, j].transform.GetChild(1).GetComponent<Image>().sprite.name)
                            {
                                dontCheck.Clear();
                                dontCheck.Add(itemsScript[i, j]);
                                dontCheck.Add(itemsScript[i + m, j]);
                                if (m == 1)
                                {
                                    if (i + 2 < row && itemsScript[i + 2, j].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i + 2, j], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                else
                                {
                                    if (itemsScript[i + 1, j].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i + 1, j], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                if (dir != Dir.No)
                                {
                                    if (m == 1)
                                    {
                                        AddToCanClearList(itemsScript[i + 2, j], dir, dontCheck);
                                    }
                                    else
                                    {
                                        AddToCanClearList(itemsScript[i + 1, j], dir, dontCheck);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                    if (j - m >= 0)
                    {
                        if (itemsScript[i, j - m].gameObject.activeSelf)
                        {
                            if (itemsScript[i, j].transform.GetChild(1).GetComponent<Image>().sprite.name == itemsScript[i, j - m].transform.GetChild(1).GetComponent<Image>().sprite.name)
                            {
                                dontCheck.Clear();
                                dontCheck.Add(itemsScript[i, j]);
                                dontCheck.Add(itemsScript[i, j - m]);
                                if (m == 1)
                                {
                                    if (j - 2 >= 0 && itemsScript[i, j - 2].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i, j - 2], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                else
                                {
                                    if (itemsScript[i, j - 1].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i, j - 1], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                if (dir != Dir.No)
                                {
                                    if (m == 1)
                                    {
                                        AddToCanClearList(itemsScript[i, j - 2], dir, dontCheck);
                                    }
                                    else
                                    {
                                        AddToCanClearList(itemsScript[i, j - 1], dir, dontCheck);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                    if (j + m < col)
                    {
                        if (itemsScript[i, j + m].gameObject.activeSelf)
                        {
                            if (itemsScript[i, j].transform.GetChild(1).GetComponent<Image>().sprite.name == itemsScript[i, j + m].transform.GetChild(1).GetComponent<Image>().sprite.name)
                            {
                                dontCheck.Clear();
                                dontCheck.Add(itemsScript[i, j]);
                                dontCheck.Add(itemsScript[i, j + m]);
                                if (m == 1)
                                {
                                    if (j + 2 < col && itemsScript[i, j + 2].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i, j + 2], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                else
                                {
                                    if (itemsScript[i, j + 1].gameObject.activeSelf)
                                    {
                                        dir = CheckSameOnColAndRow(itemsScript[i, j + 1], itemsScript[i, j], dontCheck);
                                    }
                                    else
                                    {
                                        dir = Dir.No;
                                    }
                                }
                                if (dir != Dir.No)
                                {
                                    if (m == 1)
                                    {
                                        AddToCanClearList(itemsScript[i, j + 2], dir, dontCheck);
                                    }
                                    else
                                    {
                                        AddToCanClearList(itemsScript[i, j + 1], dir, dontCheck);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测item上下左右是否有和sameitem相同的item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="sameItem"></param>
    /// <param name="dontCheck"></param>
    /// <returns></returns>
    public Dir CheckSameOnColAndRow(SimpleItem item, SimpleItem sameItem, List<SimpleItem> dontCheck)
    {
        if (item.row - 1 >= 0)
        {
            if (itemsScript[item.row - 1, item.col].gameObject.activeSelf)
            {
                if (!dontCheck.Contains(itemsScript[item.row - 1, item.col]))
                {
                    if (itemsScript[item.row - 1, item.col].transform.GetChild(1).GetComponent<Image>().sprite.name == sameItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                    {
                        return Dir.Up;
                    }
                }
            }
        }
        if (item.row + 1 < row)
        {
            if (itemsScript[item.row + 1, item.col].gameObject.activeSelf)
            {
                if (!dontCheck.Contains(itemsScript[item.row + 1, item.col]))
                {
                    if (itemsScript[item.row + 1, item.col].transform.GetChild(1).GetComponent<Image>().sprite.name == sameItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                    {
                        return Dir.Down;
                    }
                }
            }
        }
        if (item.col - 1 >= 0)
        {
            if (itemsScript[item.row, item.col - 1].gameObject.activeSelf)
            {
                if (!dontCheck.Contains(itemsScript[item.row, item.col - 1]))
                {
                    if (itemsScript[item.row, item.col - 1].transform.GetChild(1).GetComponent<Image>().sprite.name == sameItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                    {
                        return Dir.Left;
                    }
                }
            }
        }
        if (item.col + 1 < col)
        {
            if (itemsScript[item.row, item.col + 1].gameObject.activeSelf)
            {
                if (!dontCheck.Contains(itemsScript[item.row, item.col + 1]))
                {
                    if (itemsScript[item.row, item.col + 1].transform.GetChild(1).GetComponent<Image>().sprite.name == sameItem.transform.GetChild(1).GetComponent<Image>().sprite.name)
                    {
                        return Dir.Right;
                    }
                }
            }
        }
        return Dir.No;
    }

    /// <summary>
    /// 添加进可清除列表
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dir"></param>
    /// <param name="addList"></param>
    public void AddToCanClearList(SimpleItem item, Dir dir, List<SimpleItem> addList)
    {
        SimpleItem targetItem = null;
        canClearList.Clear();
        switch (dir)
        {
            case Dir.Up:
                targetItem = itemsScript[item.row - 1, item.col];
                break;
            case Dir.Down:
                targetItem = itemsScript[item.row + 1, item.col];
                break;
            case Dir.Left:
                targetItem = itemsScript[item.row, item.col - 1];
                break;
            case Dir.Right:
                targetItem = itemsScript[item.row, item.col + 1];
                break;
        }
        canClearList.Add(targetItem);
        canClearList.AddRange(addList);
    }

    /// <summary>
    /// 显示可消除Item
    /// </summary>
    public void ShakeCanClear()
    {
        foreach (var item in canClearListTweener)
        {
            item.Kill();
        }
        canClearListTweener.Clear();
        foreach (var item in canClearList)
        {
            if (item == canClearList[canClearList.Count - 1])
            {
                canClearListTweener.Add(item.transform.GetChild(1).DOScale(1.2f, duration).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    ShakeCanClear();
                }));
            }
            else
            {
                canClearListTweener.Add(item.transform.GetChild(1).DOScale(1.2f, duration).SetLoops(2, LoopType.Yoyo));
            }
        }
    }

    /// <summary>
    /// 重新摆放位置
    /// </summary>
    public void ResetPosition()
    {
        List<SimpleItem> randomList = new List<SimpleItem>();
        SimpleItem[,] newItemsScript = new SimpleItem[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(itemsScript[i, j].gameObject.activeSelf)
                {
                    randomList.Add(itemsScript[i, j]);
                }
                else
                {
                    newItemsScript[i, j] = itemsScript[i, j];
                }
            }
        }
        int index;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(newItemsScript[i, j] == null)
                {
                    index = Random.Range(0, randomList.Count);
                    newItemsScript[i, j] = randomList[index];
                    newItemsScript[i, j].row = i;
                    newItemsScript[i, j].col = j;
                    randomList.RemoveAt(index);
                }               
            }
        }
        itemsScript = newItemsScript;
        if (CheckCanClear())
        {
            Tweener tweener = null;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    tweener = itemsScript[i, j].transform.DOMove(itemsPosition[i, j], 0.4f);
                }
            }
            tweener.OnComplete(() =>
            {
                AutoClear();
            });
        }
        else
        {
            ResetPosition();
        }
        
    }


    public void RandomChangeItemType(int num)
    {
        List<SimpleItem> list = new List<SimpleItem>();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(itemsScript[i, j].type == ItemType.Normal)              
                {
                    list.Add(itemsScript[i, j]);
                }
            }
        }
        for (int i = 0; i < num; i++)
        {
            if (list.Count == 0)
                break;
            int index = Random.Range(0, list.Count);
            SimpleItem tempItem = list[index];
            list.RemoveAt(index);
            tempItem.Type = (ItemType)Random.Range(1, System.Enum.GetValues(typeof(ItemType)).Length);
        }
    }

    /// <summary>
    /// 清除特殊Item
    /// </summary>
    public void BoomSpecial()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if(itemsScript[i,j].gameObject.activeSelf && itemsScript[i, j].type != ItemType.Normal)
                {
                    boomList.Add(itemsScript[i, j]);
                }
            }
        }
        BoomItem();
    }

    float timer = 0;
    bool isShowCanClear = false;

    private void Update()
    {
        if (canChange)
        {           
            if (timer > 5)
            {
                if (!isShowCanClear)
                {
                    ShakeCanClear();
                    isShowCanClear = true;
                }               
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            timer = 0;
            isShowCanClear = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetPosition();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RandomChangeItemType(4);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            BoomSpecial();
        }
    }
}
