using UnityEngine;
using System.Collections;
public class Coroutine : MonoBehaviour
{
    //一般方法:無傳回
    private void MethodA()
    {

    }
    //有傳回方法
    private int MethodB()
    {
        return 10;
    }
    //協程方法
    //1.傳回類型: IEnumerator 
    //2.return yield 時間 new WaitForSeconds(秒數), null 一個影格的時間
    private IEnumerator Test()
    {
        print("我是協程的第一行");
        yield return new WaitForSeconds(2);
        print("我是兩秒後的程式");
    }
    private void Start()
    {
        //呼叫協程
        StartCoroutine(Test());
        StartCoroutine(Big());
    }

    public Transform cube;

    private IEnumerator Big()
    {
        for (int i = 0; i < 10; i++)
        {
            cube.localScale += new Vector3(0.5f,0.5f,0.5f);//尺寸+=三維向量=0.5f
            yield return new WaitForSeconds(0.3f);//等待0.3秒
        }
    }

}
