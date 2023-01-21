using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bgSprite;//������Ƭ
    private Vector2 BeginPos = new Vector2(-1.5f, 1.5f);//�������Ļ�м�
    private float OffsetX = 1.1f;//xy �Ӹ�0.1 �и���϶
    private float OffsetY = 1.1f;

    public GameObject _card;//��Ƭ����
    private GameObject[,] cardList = new GameObject[4, 4];//��Ƭ��Ϸ�����Ӧ�����̸���
    private int CardNum = 0;//���̸��ӵĿ�Ƭ������������������¿�ʼ��Ϸ

    void Start()
    {
        CreateBG();
        CreateCard();
        CreateCard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))//��
        {
            MoveUp();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.S))//��
        {
            MoveDown();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.A))//��
        {
            MoveLeft();
            CreateCard();
        }
        if (Input.GetKeyDown(KeyCode.D))//��
        {
            MoveRight();
            CreateCard();
        }
    }

    void CreateBG()
    {
        GameObject BG = new GameObject("BG");//��������Ϸ������Ϊ����Ԥ����
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
                Debug.Log("��Ϸ�ɹ�");
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
                if (cardList[i, j] != null)//���ҵ����еĿ�Ƭ��
                {
                    GameObject temp = cardList[i, j];//�����ÿ�Ƭ������
                    int x = i;
                    int y = j;
                    bool isFind = false;//���ò��ұ�ʶ
                    while (!isFind)
                    {
                        x--;//���ݷ���Ĳ�ͬ  x--������ x++������
                        if (x < 0 || cardList[x, y])//�ﵽ�߽���ҵ���Ƭ��
                        {
                            isFind = true;
                            //�ж�ֵ�Ƿ���ͬ����ͬ�Ļ��ϲ�����
                            if (x >= 0 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//�����ƶ�����
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
                if (cardList[i, j] != null)//���ҵ����еĿ�Ƭ��
                {
                    GameObject temp = cardList[i, j];//�����ÿ�Ƭ������
                    int x = i;
                    int y = j;
                    bool isFind = false;//���ò��ұ�ʶ
                    while (!isFind)
                    {
                        x++;//���ݷ���Ĳ�ͬ  x--������ x++������
                        if (x > 3 || cardList[x, y])//�ﵽ�߽���ҵ���Ƭ��
                        {
                            isFind = true;
                            //�ж�ֵ�Ƿ���ͬ����ͬ�Ļ��ϲ�����
                            if (x <= 3 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//�����ƶ�����
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
                if (cardList[i, j] != null)//���ҵ����еĿ�Ƭ��
                {
                    GameObject temp = cardList[i, j];//�����ÿ�Ƭ������
                    int x = i;
                    int y = j;
                    bool isFind = false;//���ò��ұ�ʶ
                    while (!isFind)
                    {
                        y--;//���ݷ���Ĳ�ͬ  x--������ x++������
                        if (y < 0 || cardList[x, y])//�ﵽ�߽���ҵ���Ƭ��
                        {
                            isFind = true;
                            //�ж�ֵ�Ƿ���ͬ����ͬ�Ļ��ϲ�����
                            if (y >= 0 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//�����ƶ�����
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
                if (cardList[i, j] != null)//���ҵ����еĿ�Ƭ��
                {
                    GameObject temp = cardList[i, j];//�����ÿ�Ƭ������
                    int x = i;
                    int y = j;
                    bool isFind = false;//���ò��ұ�ʶ
                    while (!isFind)
                    {
                        y++;//���ݷ���Ĳ�ͬ  x--������ x++������
                        if (y > 3 || cardList[x, y])//�ﵽ�߽���ҵ���Ƭ��
                        {
                            isFind = true;
                            //�ж�ֵ�Ƿ���ͬ����ͬ�Ļ��ϲ�����
                            if (y <= 3 && cardList[x, y].GetComponent<Card>()._currentIndex == cardList[i, j].GetComponent<Card>()._currentIndex)
                            {
                                cardList[x, y].GetComponent<Card>().Merge();
                                Destroy(cardList[i, j]);
                                cardList[i, j] = null;
                            }
                            else//�����ƶ�����
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
