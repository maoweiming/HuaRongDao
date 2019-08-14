using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_GameManager : MonoBehaviour {
    //https://blog.csdn.net/q764424567/article/details/81324810
    #region 单例 
    public static m_GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static m_GameManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyImmediate(this);//立即销毁 
        }
    }
    #endregion

    #region 打算是有两个数组，一个数组是1-16排好序的，另一个是1-16乱序的数组，
    //只要把乱序数组调整回有序的数组，我们就赢了。也就是说那个有序数组是为了方便比较才创建的。
    private int[] Array;

    private List<int> list;
    private List<int> yao;
    void Start()
    {
        Array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
      
        yao = new List<int>();
        for (int i = 1; i < 17; i++)
        {
            yao.Add(i);
        }
        list = new List<int>();

    }
    #endregion



    #region 接下来是要打乱数组里的数字顺序，我采取的策略是随机两两互换。
   
    //我想了一会，也就想到这个比较好，还挺有意思。不知道大家有没有更好的方法。
    /// <summary> 
    /// 数组里的数字随机两两交换 
    /// </summary> 
    /// <param name="a">数组</param> 
    /// <param name="temptimes">交换次数</param> 
    void A_random(int[] a, int temptimes)
    {
        Hashtable hashtable = new Hashtable();
        System.Random rm = new System.Random();
        for (int i = 0; hashtable.Count < temptimes; i++)
        {
            int nValue = rm.Next(1,17);
            if (!hashtable.ContainsValue(nValue))
            {
                hashtable.Add(nValue,nValue);
                Debug.Log(nValue);
                list.Add(nValue);
              
            }
        }
        foreach (int num in a)
        {
          //Debug.Log(num);
        }
    }

    #endregion

    #region 上面那个函数只是动数据，没有动画面。接下来就是写函数改变图块位置，使画面与数据一致。
    //这里面有一个对应关系可以利用，在unity面板里，table子物体的索引值=乱序数组array的索引值。
    /// <summary> 
    /// 图块按照数组重新排序 
    /// </summary> 
    /// <param name="gameObject">table</param> 
    /// <param name="a">乱序数组</param> 
    void P_sequence(GameObject gameObject, List<int> a)
    {
        foreach (int num in a)
        {
            string name = "piece" + num;
           
            gameObject.transform.Find(name).SetSiblingIndex(-1);
        }
    }
    //那接下来，我们的开始游戏button对应的函数，可以这样写。别忘了在unity界面，把开始游戏button和这个函数绑定上。
    //我们还要获取table这个玩意的gameobject，在unity界面也要记得绑定上，即给代码里的“table”变量赋值。
    public GameObject table;
    public void GameStart()
    {
        A_random(Array, 16);
        P_sequence(table, list);
       
    }

    //接下来就是交换了，逻辑很简单：点击一个图块，如果图块周围有白板，那被点击的图块和白板交换位置。
    //关键点就是怎么检测图块周围有白板。这是这个游戏核心功能实现的最关键的地方。我本来是想【找被点击的x周围有没有白板】
    //，后来想了想，觉得【找白板周围的数字是不是x】会简单一点。
    /// <summary> 
    /// 判断【数字16（白板）】上下左右的项的索引值，如果是被点击的x，则交换两个索引值里的数据，然后调用函数刷新面板里图块的顺序 
    /// </summary> 
    /// <param name="x">点击图块的索引值</param> 
    void A_exchange(int x)
    {
        int index = table.transform.Find("piece16").GetSiblingIndex() + 1;
      //  Debug.Log("白板：---"+index);
        if (((index == x - 1) && !(index % 4 == 0)) || ((index == x + 1) && !(index % 4 == 1)) || (index + 4 == x) || (index - 4 == x))
        {
           // Debug.Log("-----进来----");
            //int temp = Array[x - 1];
            //Array[x - 1] = Array[index - 1];
            //Array[index - 1] = temp;
            //P_sequence(table, Array);
            int temp = list[x - 1];

            list[x - 1] = list[index - 1];
            list[index - 1] = temp;
            P_sequence(table, list);
        }

        for (int i = 0; i < list.Count; i++)
        {
            int a = list[i];
            int b = yao[i];
            if (a == b)
            {
                c++;
                if (c==16)
                {
                    c = 0;
                    Debug.Log("-------------------------成功------------------");
                }
               
            }
          
        }
        c = 0;
    }

    int c = 0;
    //下来是给每个图块添加一个button组件，在onclick里绑定如下函数，传递的参数对应为图块本身的数字。
    //这样在乱序的时候，点击一个图块时，我们先获取这个图块对应的数组索引值/table子物体索引值，
    //然后调用并传递给A_exchange函数，让其从索引值判断上下左右的关系，从而判断是否可以交换位置。
    /// <summary> 
    /// 获取数字num对应Array的索引值和table子物体的索引值 
    /// </summary> 
    /// <param name="num"></param> 
    public void Getsiblingindex(int num)
    {
      //  Debug.Log(num + "----num--");
        int s = table.transform.Find("piece" + num).GetSiblingIndex();
        A_exchange(s + 1);
    }


    #endregion
}


