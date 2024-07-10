using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask targetLayer;
    public int minDamage;
    public int maxDamage;
    private bool hasDamagedPlayer = false; // Kontrol de�i�keni
    public float knockbackForce = 120f; // Knockback kuvveti

    [Header("Projectile  Particles")]
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem vaweParticles;

    private void Awake()
    {
        // damage de�i�kenini Awake() i�inde belirleyin
        minDamage = 5;
        maxDamage = 15;
    }
    private void OnTriggerEnter(Collider other)
    {
        // �arp��ma alan�na bir nesne girdi�inde
        if (other.gameObject.GetComponent<CharacterController>() || 
            other.gameObject.GetComponent<CapsuleCollider>())
        {
            // �arpan nesnenin PlayerHealth bile�enini kontrol edin ve hasar� uygulay�n
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int damage = Random.Range(minDamage, maxDamage + 1);
                playerHealth.takeDamage(damage);
            }

            // Projektili yok edin
            SoundManager.instance.PlayProjectileRockExplotion();

            Destroy(gameObject);

            if (explosionParticles != null)
            {
                Vector3 particlePosition = transform.position + new Vector3(0, 2, 0);
                Quaternion particleRotation = Quaternion.Euler(90f, 180f, 0f); // Example rotation (adjust as needed)
                Vector3 particleScale = new Vector3(8f, 6f, 6f); // Example scale (adjust as needed)

                ParticleSystem newParticles = Instantiate(explosionParticles, particlePosition, particleRotation);
                newParticles.transform.localScale = particleScale;
                newParticles.Play();

                Destroy(newParticles.gameObject, newParticles.main.duration);
            }

            
            hasDamagedPlayer = true; // Tek seferlik hasar kontrol�


        }
    }
   
}
