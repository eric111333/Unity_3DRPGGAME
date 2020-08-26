using UnityEngine;

public class Loop : MonoBehaviour
{
    //迴圈 While
    //重複處理相同程式

    private void Start()
    {
        //當布林植為true時執行一次
        if (true)
        {
            print("我是判斷式!");
        }
        //當布林植為true時持續執行
        int a = 1;

        while (true&&a<=10)
        {
            print("我是迴圈 while!!!! 迴圈次數："+a);
            a++;
        }
        //迴圈for
        //(初始值;條件;迭代器 - 初始值增減)
        for (int i = 1; i <=10; i++)
        {
            print("我是迴圈 For!!!!!!! 迴圈次數：" + i);
        }



    }
}
