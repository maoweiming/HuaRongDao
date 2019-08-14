using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    //https://blog.csdn.net/kill566666/article/details/77171299
    //单例模式
    #region single instance

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

#endregion

    public Transform emptyTransform;//记录一开始的空位置的坐标
    
    [HideInInspector]//在面板上隐藏掉这个公共变量
    public Vector3 empty;//方块移动时存储的空位置的坐标

    public GameObject[] pieces;//存储各个方块
    private Vector3[] piecePositions;//存储各个方块的位置,用作判断是否完成游戏
    private bool isSwaped = false;//记录是否调用SwapPiece()方法,用作判断是否完成游戏
	void Start () {
        //初始化empty,piecePositions的值
        empty = emptyTransform.position;
        piecePositions = new Vector3[pieces.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            piecePositions[i] = pieces[i].transform.position;
        }
        SwapPiece();
	}
   public void SwapPiece()//打乱方块的方法
    {
        int[] step = { -1, 1, -3, 3 };
        int emptyIndex = pieces.Length - 1;//空白方块的索引
        int i = 0;
        while( i < 1000)//随机点击各个方块,每点击一次就交换了一次方块
        {
            var index = emptyIndex + step[Random.Range(0, 4)];
            if (index < 8 && index >=0)
            {
                pieces[index].GetComponent<Block>().OnMouseDown();
                i++;
            }
            emptyIndex = index;
        }
        isSwaped = true;
    }
     public void SwapEmpty(Vector3 targer)//主要是判断游戏结果
    {
        empty = targer;
        if (emptyTransform.position == empty)
        {
            bool isWin = true;
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].transform.position != piecePositions[i])
                {
                    isWin = false;
                    break;
                }
            }
            if (isWin && isSwaped)
            {
                print("Win");
                isSwaped = false;
            }
        }
    }
}
