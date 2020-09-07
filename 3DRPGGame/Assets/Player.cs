using UnityEngine;

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

    public float exp;
    public int lv = 1;

    private Animator ani;
    private Rigidbody rig;
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
    }
    #endregion

    #region 方法：功能
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Vector3 pos = new Vector3(h, 0, v); 世界座標
        Vector3 pos = transform.forward * v + transform.right * h;
        rig.MovePosition(transform.position + pos*speed*Time.fixedDeltaTime);
        ani.SetFloat("move", Mathf.Abs(v) + Mathf.Abs(h));
    }
    #endregion

    /// <summary>
    /// 固定更新：固定 50 FPS
    /// 有物理運動在這裡呼叫 Rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }
}
