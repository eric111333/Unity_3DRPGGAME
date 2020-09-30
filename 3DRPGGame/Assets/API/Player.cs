using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public AudioClip hitclip;
    public AudioClip attackclip;
    public AudioClip skillclip;
    [Header("任務數量")]
    public Text eatText;
    [Header("Bar條")]
    public Image barHp;
    public Image barMp;
    public Image barExp;
    public Text textLV;
    [Header("Skill流星雨")]
    public GameObject rock;
    public Transform pointRock;
    public float costRock = 20;
    public float damageRock = 150;
    private float cdSkill;


    public float exp;
    private int lv = 1;
    private int count;
    private float maxHp;
    private float maxMp;
    private float maxExp;
    private float[] exps;

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

        maxHp = hp;
        maxMp = mp;

        exps = new float[99];

        for (int i = 0; i < exps.Length; i++)
        {
            exps[i] = 100 * (i + 1);
        }
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

    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        rig.AddForce(direction.forward * 150 + direction.up * 80);
        barHp.fillAmount = hp / maxHp;
        aud.PlayOneShot(hitclip);
        ani.SetTrigger("hit");

        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        enabled = false;
        ani.SetBool("die", true);

        StartCoroutine(ShowFinal());
    }

    [Header("END")]
    public CanvasGroup final;
    public Text textFinalTitle;

    private IEnumerator ShowFinal()
    {
        yield return new WaitForSeconds(0.5f);
        textFinalTitle.text = "任務失敗";
        Cursor.visible = true;
        final.interactable = true;
        final.blocksRaycasts = true;
        while (final.alpha < 1)
        {
            final.alpha += 0.5f * Time.deltaTime;
            yield return null;
        }

    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("attack");
            aud.PlayOneShot(attackclip);
        }
    }

    public void Exp(float expGet)
    {
        exp += expGet;
        barExp.fillAmount = exp / maxExp;

        while (exp >= maxExp) LevelUP();
    }

    public void LevelUP()
    {
        lv++;
        textLV.text = "LV" + lv;

        maxHp += 20;
        maxMp += 5;
        attack += 10;




        hp = maxHp;
        mp = maxMp;
        barHp.fillAmount = 1;
        barMp.fillAmount = 1;
        exp -= maxExp;
        maxExp = exps[lv - 1];
        barExp.fillAmount = exp / maxExp;



    }

    private void SkillRock()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && mp >= costRock && cdSkill >= 1)
        {
            ani.SetTrigger("skill");
            aud.PlayOneShot(skillclip);
            Instantiate(rock, pointRock.position, pointRock.rotation);
            mp -= costRock;
            barMp.fillAmount = mp / maxMp;
            cdSkill = 0;
        }
    }

    #endregion

    /// <summary>
    /// 固定更新：固定 50 FPS
    /// 有物理運動在這裡呼叫 Rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        if (stop) return;
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack") || ani.GetCurrentAnimatorStateInfo(0).IsName("Skill")) return;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hit(attack, transform);
        }
    }
    private void Update()
    {
        Attack();
        SkillRock();
        Restore(ref mp, restoreMp, maxMp, barMp);
        Restore(ref hp, restoreHp, maxHp, barHp);
        cdSkill += Time.deltaTime;
    }


    [Header("回魔量/每秒")]
    public float restoreMp = 2;
    [Header("回血量/每秒")]
    public float restoreHp = 5;
    private void Restore(ref float value, float restore, float max, Image bar)
    {
        value += restore * Time.deltaTime;
        value = Mathf.Clamp(value, 0, max);
        bar.fillAmount = value / max;
    }

}
