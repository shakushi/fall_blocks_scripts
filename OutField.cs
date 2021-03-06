using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OutField");
            other.GetComponent<PlayerCtlr>().IPRespone();
        }
    }
}
