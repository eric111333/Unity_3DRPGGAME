using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("目標")]
    public Transform target;
    [Header("追蹤速度"), Range(0, 500)]
    public float speed = 3;
    [Header("旋轉速度"), Range(0, 500)]
    public float turn = 45;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(30, -20);

    private Quaternion rot;

    private void Awake()
    {
        Cursor.visible = false;
    }
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        Vector3 posA = transform.position;
        Vector3 posB = target.position;
        transform.position = Vector3.Lerp(posA, posB, Time.deltaTime * speed);

        //旋轉
        rot.x -= Input.GetAxis("Mouse Y") * turn * Time.deltaTime;
        rot.y -= Input.GetAxis("Mouse X") * turn * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, limit.y, limit.x);
        transform.rotation = Quaternion.Euler(rot.x,rot.y, rot.z);
    }
    private void LateUpdate()
    {
        Track();
    }

}
