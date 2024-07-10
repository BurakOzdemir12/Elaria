using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRaycast : MonoBehaviour
{
    public int damage = 10;
    public LayerMask targetLayer;
    private bool hasDamagedPlayer = false; // Kontrol deðiþkeni

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetLayer))
        {
            if (hit.collider.CompareTag("Player") && !hasDamagedPlayer)
            {
                // Hedefe isabet edildiðinde
                hit.collider.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
                hasDamagedPlayer = true; 

                Destroy(gameObject);

            }

        }
    }
}
