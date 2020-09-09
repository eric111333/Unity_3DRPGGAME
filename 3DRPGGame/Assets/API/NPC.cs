using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話資訊")]
    public GameObject panel;
    [Header("對畫名稱")]
    public Text textName;

    private void Type()
    {

    }

    private void NoMission()
    {

    }

    private void Missioning()
    {

    }

    private void Finish()
    {

    }

    private void DialogStart()
    {
        panel.SetActive(true);
        textName.text = name;
    }

    private void DialogStop()
    {
        panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            DialogStart();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "player")
        {
            DialogStop();
        }
    }

}
