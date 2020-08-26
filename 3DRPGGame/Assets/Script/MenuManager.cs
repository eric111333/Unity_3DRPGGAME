using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

///<summary>
///選單管理:開始遊戲.載入遊戲.離開遊戲
///</summary>
public class MenuManager : MonoBehaviour
{
    [Header("載入畫面")]
    public GameObject panel;
    [Header("載入進度")]
    public Text textLoading;
    [Header("載入霸條")]
    public Image imgLoading;
    [Header("提示文字")]
    public Text textTip;
    /// <summary>
    /// 離開
    /// </summary>
    public void Quit()
    {
        Application.Quit();//應用程式.離開
    }
    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        panel.SetActive(true);                                      //啟動設定(BOOL)  -  True 顯示. false 隱藏
        AsyncOperation ao = SceneManager.LoadSceneAsync("遊戲場景");//非同步載入資訊 = 場景管理器.非同步載入("場景名稱")
        ao.allowSceneActivation = false;                           //載入資訊.允許自動載入 = 否 不允許自動載入

        //當載入訊息.完成 為false -  尚未載入完程時執行
        while (!ao.isDone)                                          //(ao.isDone == false)
        {
            //progress 載入場景的進度為0-1.如果設定allow為false會卡在0.9
            //ToString("F小數點位數"):小數點2位F2 .小數點0位F0 
            textLoading.text = "載入進度" + (ao.progress / 0.9f * 100).ToString("F1") + "％";//載入文字 = "載入進度" + ao.進度*100 + "％"
            imgLoading.fillAmount = ao.progress / 0.9f;
            yield return null;

            if (ao.progress == 0.9f)         //如果 ao.進度=0.9                   
            {
                textTip.enabled = true;      //提是文字.啟動 = 是 - 顯示提是文字
                if (Input.anyKeyDown)
                    ao.allowSceneActivation = true; //如果按下任意鍵 允許進入

            }

        }

    }

}
