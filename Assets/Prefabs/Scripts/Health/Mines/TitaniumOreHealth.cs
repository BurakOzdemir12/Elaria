using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitaniumOreHealth : MonoBehaviour
{


    [SerializeField] private int currentHealth = 60;
    [SerializeField] private List<ItemDrop> ItemDrops = new List<ItemDrop>();
    Animator animator;

    [Header("Tree Particles")]
    [SerializeField] private ParticleSystem titaniumDamageParticles;
    [SerializeField] private ParticleSystem titaniumBreaksParticles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void takeDamage(int damage, GameObject player, string attackerTag)
    {
        if (attackerTag == "TitaniumPickAxe" ||
            attackerTag == "MythrilPickAxe" 
            )
        {
            Debug.Log("Attacker tag: " + attackerTag);
            // Start a coroutine to delay the execution
            StartCoroutine(DamageWithDelay(damage));
        }
        else
        {
            /*StartCoroutine(DamageWithDelay(0));*/
        }

        /*if (AxeAnimation.Instance.isAttackTriggered && attackerTag == "StoneAxe")
        {
            SoundManager.instance.PlayChoppingTree();
            animator.SetTrigger("isTreeAttack");
            currentHealth -= damage; // burayý animasyon ve random damage için düzenle
        }*/
        IEnumerator DamageWithDelay(int damage)
        {
            currentHealth -= damage;
            yield return new WaitForSeconds(0.23f); // Delay for 0.2 seconds

            SoundManager.instance.PlayMetallicOreBreak();
            animator.SetTrigger("isStoneGetHit");

            if (titaniumDamageParticles != null)
            {
                titaniumDamageParticles.Play();
            }

            if (currentHealth <= 0)
            {
                SoundManager.instance.PlayMetallicOreBreak();
            }
        }

        if (currentHealth <= 0)
        {
            SoundManager.instance.PlayOreBreak();

            if (titaniumBreaksParticles != null)
            {
                ParticleSystem newParticles = Instantiate(titaniumBreaksParticles, transform.position + new Vector3(0, 3, 0), titaniumBreaksParticles.transform.rotation);
                newParticles.Play();

                /* Vector3 particlePosition = transform.position + new Vector3(0, 6, 0);
                 Quaternion particleRotation = Quaternion.Euler(90f, 180f, 0f); // Example rotation (adjust as needed)
                 Vector3 particleScale = new Vector3(5f, 6f, 1.5f); // Example scale (adjust as needed)

                 ParticleSystem newParticles = Instantiate(titaniumBreaksParticles, particlePosition, particleRotation);
                 newParticles.transform.localScale = particleScale;
                 newParticles.Play();
 */
                Destroy(newParticles.gameObject, newParticles.main.startLifetime.constant);
            }

            foreach (ItemDrop item in ItemDrops)
            {


                /*if (quantityToDrop == 0) { // it alows not dropping item
                    return;
                }*/

                /* Item droppedItem=Instantiate(item.ItemToDrop,
                     transform.position,
                     Quaternion.identity).GetComponent<Item>();
                 droppedItem.currentQuantity = quantityToDrop;
 */

                int quantityToDrop = Random.Range(item.minQuantityToDrop, item.maxQuantityToDrop);

                Vector3 newPosition = transform.position + new Vector3(0, 3, 0);

                Item droppedItem = Instantiate(item.ItemToDrop, newPosition, Quaternion.identity).GetComponent<Item>();
                droppedItem.currentQuantity = quantityToDrop;

                //player.GetComponent<Inventory>().addItemToInventory(droppedItem);
                player.GetComponent<Inventory>().gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
[System.Serializable]
public class TitaniumItemDrop
{
    public GameObject TitaniumItemToDrop;
    public int minQuantityToDrop = 1;
    public int maxQuantityToDrop = 6;
}

