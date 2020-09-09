using UnityEngine;

//列舉(下拉式選單)
public enum StateNPC
{
    NOMission,Missioning,Finish
}

//ScriptableObject腳本化物件：將資料儲存於專案
//CreateAssetMenu建立菜單：檔案名稱與菜單名稱(fileName = "NPC 資料" , menuName = "TANG/NPC 資料")]
[CreateAssetMenu(fileName = "NPC 資料" , menuName = "TANG/NPC 資料")]
public class NPCData : ScriptableObject
{
    [Header("打字速度"), Range(0f, 3f)]
    public float speed = 0.5f;
    [Header("打字音效")]
    public AudioClip soundType;
    [Header("任務需求數量"), Range(5, 50)]
    public int count;
    [Header("對話"), TextArea(3,10)]
    public string[] dialogs = new string[3];
    [Header("NPC 狀態")]
    public StateNPC state;

}
