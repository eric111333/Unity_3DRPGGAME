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
    [Header("第一段對話完要顯示的物件")]
    public GameObject objectShow;
    public GameObject spawnShow;
    [Header("任務資訊")]
    public RectTransform rectMission;

    private AudioSource aud;
    private Player player;
    private Animator ani;

    //private string 

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        ani = GetComponent<Animator>();
        data.state = StateNPC.NOMission;
    }


    private IEnumerator Type()
    {
        PlayAni();
        player.stop = true;
        textContent.text = "";

        string dialog = data.dialogs[(int)data.state];

        for (int i = 0; i < dialog.Length; i++)
        {
            textContent.text += dialog[i];
            aud.PlayOneShot(data.soundType, 0.5f);
            yield return new WaitForSeconds(data.speed);
        }
        player.stop = false;

        NoMission();
    }

    private void PlayAni()
    {
        if(data.state != StateNPC.Finish)
        {
            ani.SetBool("speak", true);
        }
        else
        {
            ani.SetTrigger("完成");
        }
    }

    private void NoMission()
    {
        if (data.state != StateNPC.NOMission) return;
        data.state = StateNPC.Missioning;
        objectShow.SetActive(true);
        spawnShow.SetActive(true);
        StartCoroutine(ShowMission());
    }

    private IEnumerator ShowMission()
    {
        while (rectMission.anchoredPosition.x > -220)
        {
            rectMission.anchoredPosition -= new Vector2(1000 * Time.deltaTime, 0);
                yield return null;
        }
    }

    private void Missioning()
    {

    }

    public void Finish()
    {
        data.state = StateNPC.Finish;
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
        ani.SetBool("speak", false);
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
