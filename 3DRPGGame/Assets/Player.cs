using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位：基本資料
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 5;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn = 5;
    [Header("攻擊力"), Range(0, 500)]
    public float attack = 20;
    [Header("血量"), Range(0, 500)]
    public float hp = 250;
    [Header("魔力"), Range(0, 500)]
    public float mp = 50;
    [Header("吃道具音效")]
    public AudioClip soundProp;
    [Header("任務數量")]
    public Text eatText;

    public float exp;
    public int lv = 1;
    private int count;

    private Animator ani;
    private Rigidbody rig;
    private Transform cam;
    [HideInInspector]
    public bool stop;
    private AudioSource aud;
    private NPC npc;
    #endregion


    #region 事件:入口
    /// <summary>
    /// 喚醒：會在start前執行一次
    /// </summary>
    private void Awake()
    {
        //取得文件()
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        cam = GameObject.Find("攝影機跟物件").transform;
        aud = GetComponent<AudioSource>();
        npc = FindObjectOfType<NPC>();

    }
    #endregion

    #region 方法：功能
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Vector3 pos = new Vector3(h, 0, v); 世界座標
        //Vector3 pos = transform.forward * v + transform.right * h;區域作標
        Vector3 pos = cam.forward * -v + cam.right * -h;//攝影機座標
        rig.MovePosition(transform.position + pos * speed * Time.fixedDeltaTime);
        ani.SetFloat("move", Mathf.Abs(v) + Mathf.Abs(h));

        if (h != 0 || v != 0)
        {
            pos.y = 0;
            Quaternion angle = Quaternion.LookRotation(pos);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn * Time.fixedDeltaTime);
        }

    }

    private void EatProp()
    {
        count++;
        eatText.text = "骷髏頭：" + count + "/" + npc.data.count;

        if (count == npc.data.count) npc.Finish();

    }
    #endregion

    /// <summary>
    /// 固定更新：固定 50 FPS
    /// 有物理運動在這裡呼叫 Rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        if (stop) return;
        Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "skull")
        {
            aud.PlayOneShot(soundProp);
            Destroy(collision.gameObject);
            EatProp();
        }
    }


}
