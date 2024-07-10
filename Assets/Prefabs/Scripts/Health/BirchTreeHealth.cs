using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirchTreeHealth : MonoBehaviour
{
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private List<ItemDrop> ItemDrops = new List<ItemDrop>();
    Animator animator;

    [Header("Tree Particles")]
    [SerializeField] private ParticleSystem treeDamageParticles;
    [SerializeField] private ParticleSystem treeBreaksParticles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void takeDamage(int damage, GameObject player, string attackerTag)
    {
        Debug.Log("Attacker tag: " + attackerTag);
        
            if ( attackerTag == "StoneAxe" ||
            attackerTag == "SteelAxe" ||
            attackerTag == "OrichalcumAxe" ||
            attackerTag == "MythrilAxe" ||
            attackerTag == "TitaniumAxe"

            ) {
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

            SoundManager.instance.PlayChoppingTree();
            animator.SetTrigger("isBTreeAttack");

            if (treeDamageParticles != null)
            {
                treeDamageParticles.Play();
            }

            if (currentHealth <= 0)
            {
                SoundManager.instance.PlayChoppingTree();
            }
        }

        if (currentHealth <= 0)
        {
            SoundManager.instance.PlayFallingTree();

            if (treeBreaksParticles != null)
            {
                Vector3 particlePosition = transform.position + new Vector3(0, 6, 0);
                Quaternion particleRotation = Quaternion.Euler(90f, 180f, 0f); // Example rotation (adjust as needed)
                Vector3 particleScale = new Vector3(5f, 6f, 1.5f); // Example scale (adjust as needed)

                ParticleSystem newParticles = Instantiate(treeBreaksParticles, particlePosition, particleRotation);
                newParticles.transform.localScale = particleScale;
                newParticles.Play();

                Destroy(newParticles.gameObject, newParticles.main.duration);
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
public class BItemDrop
{
    public GameObject BItemToDrop;
    public int minQuantityToDrop = 1;
    public int maxQuantityToDrop = 6;
}