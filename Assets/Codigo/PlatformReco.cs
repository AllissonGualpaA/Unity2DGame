using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReco : MonoBehaviour
{
    [SerializeField] private Transform Desde;
    [SerializeField] private Transform Hasta;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Desde.position, Hasta.position);
        Gizmos.DrawSphere(Desde.position, 0.1f);
        Gizmos.DrawSphere(Hasta.position, 0.1f);
    }
}
