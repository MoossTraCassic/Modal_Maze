using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierScanner : MonoBehaviour
{
    private NPCGrenadier nPCGrenadier;

    private void Start()
    {
        nPCGrenadier = GetComponentInParent<NPCGrenadier>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nPCGrenadier.SetTarget(other.transform);
            nPCGrenadier.follow = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nPCGrenadier.follow = true;
        }
    }
}
