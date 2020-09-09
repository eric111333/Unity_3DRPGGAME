using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話資訊")]
    public GameObject panel;
    [Header("對畫名稱")]
    public Text textName;
    [Header("對話內容")]
    public Text textContent;

    private AudioSource aud;
    //private string 

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }


    private IEnumerator Type()
    {
        textContent.text = "";

        string dialog = data.dialogs[0];

        for (int i = 0; i < dialog.Length; i++)
        {
            textContent.text += dialog[i];
            aud.PlayOneShot(data.soundType, 0.5f);
            yield return new WaitForSeconds(data.speed);
        }
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
        StartCoroutine(Type());
    }

    private void DialogStop()
    {
        panel.SetActive(false);
    }

    //面向玩家
    private void LookAtPlayer(Collider other)
    {
        Vector3 pos = other.transform.position;
        pos.y = transform.position.y;
        Quaternion angle = Quaternion.LookRotation(other.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * 5);
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
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "player")
        {
            LookAtPlayer(other);
        }
    }

}
