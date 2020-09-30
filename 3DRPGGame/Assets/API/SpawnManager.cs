
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]
    public Transform enemy;
    [Header("生成間隔時間"), Range(0f, 5f)]
    public float interval = 4f;

    public GameObject[] points;
    private void Awake()
    {
        points = GameObject.FindGameObjectsWithTag("生成點");
        InvokeRepeating("Spawn", 0, interval);
    }

    private void Spawn()
    {
        int r = Random.Range(0, points.Length);
        Instantiate(enemy, points[r].transform.position, points[r].transform.rotation);
    }
}
