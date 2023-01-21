using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bgSprite;//背景卡片
    private Vector2 BeginPos = new Vector2(-1.5f, 1.5f);//差不多在屏幕中间
    private float OffsetX = 1.1f;//xy 加个0.1 有个间隙
    private float OffsetY = 1.1f;

    public GameObject _card;//卡片对象
    private GameObject[,] cardList = new GameObject[4, 4];//卡片游戏对象对应的棋盘格子
    private int CardNum = 0;//棋盘格子的卡片计数，用于满格后重新开始游戏

    void Start()
    {
        CreateBG();
        CreateCard();
        CreateCard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))//上
        {
            MoveUp();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.S))//下
        {
            MoveDown();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.A))//左
        {
            MoveLeft();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.D))//右
        {
            MoveRight();
            CreateCard();
        }
    }

    void CreateBG()
    {
        GameObject BG = new GameObject("BG");//创建空游戏对象作为背景预制体
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 newPos = new Vector2(BeginPos.x + j * OffsetX, BeginPos.y - i * OffsetY);
                Instantiate(bgSprite, newPos, Quaternion.identity, BG.transform);
            }
        }
    }

    void CreateCard()
    {
        CardNum = 0;
        foreach (var item in cardList)
        {
            if (item)
            {
                CardNum++;
            }
        }
        if (CardNum >= 16)
        {
            ResetGame();
            return;
        }

        int X_index, Y_index = 0;
        do
        {
            X_index = Random.Range(0, 4);
            Y_index = Random.Range(0, 4);
        } while (cardList[X_index, Y_index]);
        Vector2 newPos = GetPosVector2(X_index, Y_index);

        cardList[X_index, Y_index] = Instantiate(_card, newPos, Quaternion.identity);
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            cardList[X_index, Y_index].GetComponent<Card>().Generate(1);
        }
        else
        {
            cardList[X_index, Y_index].GetComponent<Card>().Generate(2);
        }
        SucceedGame();
    }

    public Vector2 GetPosVector2(int x, int y)
    {
        return new Vector2(BeginPos.x + y * OffsetX, BeginPos.y - x * OffsetY);
    }


    void ResetGame()
    {
        foreach (var item in cardList)
        {
            if (item != null)
            {
                Destroy(item);
            }
            cardList = new GameObject[4, 4];
        }
    }

    void SucceedGame()
    {
        foreach (var item in cardList)
        {
            if (item.GetComponent<Card>()._currentIndex >= 11)
            {
                Debug.Log("游戏成功");
                break;
            }
        }
        ResetGame();
    }

    void MoveUp()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cardList[i, j] != null)//当找到其中的卡片后
                {
                    GameObject temp = cardList[i, j];//保留该卡片的引用
                    int x = i;
                    int y = j;
                    bool isFind = false;//设置查找标识
                    while (!isFind)
                    {
                        x--;//根据方向的不同  x--是向上 x++是向下
                        if (x < 0 || cardList[x, y])//达到边界或找到卡片后
                        {
                            isFind = true;
                            //判断值是否相同，相同的话合并操作
                            if (x >= 0 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//否则移动即可
                            {
                                cardList[i, j] = null;
                                cardList[x + 1, y] = temp;
                                cardList[x + 1, y].transform.position = GetPosVector2(x + 1, y);
                            }
                        }
                    }
                }
            }
        }
    }

    void MoveDown()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cardList[i, j] != null)//当找到其中的卡片后
                {
                    GameObject temp = cardList[i, j];//保留该卡片的引用
                    int x = i;
                    int y = j;
                    bool isFind = false;//设置查找标识
                    while (!isFind)
                    {
                        x++;//根据方向的不同  x--是向上 x++是向下
                        if (x > 3 || cardList[x, y])//达到边界或找到卡片后
                        {
                            isFind = true;
                            //判断值是否相同，相同的话合并操作
                            if (x <= 3 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//否则移动即可
                            {
                                cardList[i, j] = null;
                                cardList[x - 1, y] = temp;
                                cardList[x - 1, y].transform.position = GetPosVector2(x - 1, y);
                            }
                        }
                    }
                }
            }
        }
    }

    void MoveLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cardList[i, j] != null)//当找到其中的卡片后
                {
                    GameObject temp = cardList[i, j];//保留该卡片的引用
                    int x = i;
                    int y = j;
                    bool isFind = false;//设置查找标识
                    while (!isFind)
                    {
                        y--;//根据方向的不同  x--是向上 x++是向下
                        if (y < 0 || cardList[x, y])//达到边界或找到卡片后
                        {
                            isFind = true;
                            //判断值是否相同，相同的话合并操作
                            if (y >= 0 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//否则移动即可
                            {
                                cardList[i, j] = null;
                                cardList[x, y + 1] = temp;
                                cardList[x, y + 1].transform.position = GetPosVector2(x, y + 1);
                            }
                        }
                    }
                }
            }
        }
    }

    void MoveRight()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cardList[i, j] != null)//当找到其中的卡片后
                {
                    GameObject temp = cardList[i, j];//保留该卡片的引用
                    int x = i;
                    int y = j;
                    bool isFind = false;//设置查找标识
                    while (!isFind)
                    {
                        y++;//根据方向的不同  x--是向上 x++是向下
                        if (y > 3 || cardList[x, y])//达到边界或找到卡片后
                        {
                            isFind = true;
                            //判断值是否相同，相同的话合并操作
                            if (y <= 3 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//否则移动即可
                            {
                                cardList[i, j] = null;
                                cardList[x, y - 1] = temp;
                                cardList[x, y - 1].transform.position = GetPosVector2(x, y - 1);
                            }
                        }
                    }
                }
            }
        }
    }
}
