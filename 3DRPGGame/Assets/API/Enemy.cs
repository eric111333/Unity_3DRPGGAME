﻿using UnityEngine;
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
        

        if(timer >=cd)
        {
            timer = 0;
            ani.SetTrigger("attack");
        }
        
    }
    #endregion

    #region 事件
    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
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
