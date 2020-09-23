using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("目標位子")]
    public Transform targrt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            targrt.GetComponent<CapsuleCollider>().enabled = false;
            other.transform.position = targrt.position;
            Invoke("OpenCollider",3f);
        }
    }
    private void OpenCollider()
    {
        targrt.GetComponent<CapsuleCollider>().enabled = true;
    }

}
