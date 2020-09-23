using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"),Range(0,100)]
    public float speed = 2f;
    [Header("攻擊"), Range(0, 100)]
    public float attack = 20f;
    [Header("血量"), Range(0, 1000)]
    public float hp = 350f;
    [Header("經驗值"), Range(0, 100)]
    public float exp = 30f;
    [Header("掉寶率"), Range(0f, 1f)]
    public float prop = 0.3f;
    [Header("掉落道具")]
    public Transform skull;
    [Header("停止距離:攻擊距離"), Range(0, 10)]
    public float rangeAttack = 1.5f;
    [Header("攻擊的冷卻時間"), Range(0, 10)]
    public float cd = 3f;


    private NavMeshAgent nma;
    private Animator ani;
    private Player player;
    private Rigidbody rig;
    private float timer;
    #endregion

    #region 方法
    private void Move()
    {
        nma.SetDestination(player.transform.position);
        ani.SetFloat("move", nma.velocity.magnitude);
        timer += Time.deltaTime;
        if (nma.remainingDistance <= rangeAttack) Attack();
    }
    private void Attack()
    {
        Quaternion look = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * speed);

        if(timer >=cd)
        {
            timer = 0;
            ani.SetTrigger("attack");
        }
        
    }
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        rig.AddForce(direction.forward * 180 + direction.up * 80);
        ani.SetTrigger("hit");

        if (hp <= 0) Dead();
    }
    private void Dead()
    {
        enabled = false;
        ani.SetBool("die", true);
        DropProp();
        Destroy(gameObject,1);
    }
    private void DropProp()
    {
        float r = Random.Range(0f, 1f);
        if(r<=prop)
        {
            Instantiate(skull, transform.position+transform.up*1.5f, transform.rotation);
        }
    }
    #endregion

    #region 事件
    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        rig = GetComponent<Rigidbody>();
        nma.speed = speed;
        nma.stoppingDistance = rangeAttack;
        nma.SetDestination(player.transform.position);
    }
    private void Update()
    {
        Move();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.8f, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "player")
        {
            other.GetComponent<Player>().Hit(attack, transform);
        }
    }
    #endregion

}
