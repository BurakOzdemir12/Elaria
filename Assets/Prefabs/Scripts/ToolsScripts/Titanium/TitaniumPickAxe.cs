using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitaniumPickAxe : MonoBehaviour
{
    [SerializeField] private int minTitaniumPickAxeDamage = 49;
    [SerializeField] private int maxTitaniumPickAxeDamage = 60;

    [SerializeField] private AnimationClip attackAnimation; // Sald�r� animasyonu
    starterAssets input;

    public bool isAttackTriggered = false;
    public float attackCooldownTimer = 0f;
    public float attackCooldown;
    bool isAttackCooldown = false;
    private string titaniumPickaxeTag;

    private void Awake()
    {
        input = new starterAssets();

        input.Player.Attack.started += onAttack;
        input.Player.Attack.canceled += onAttack;
        input.Player.Attack.performed += onAttack;
    }
    void onAttack(InputAction.CallbackContext ctx)
    {
        isAttackTriggered = ctx.ReadValueAsButton();
    }
    private void Start()
    {
        titaniumPickaxeTag = gameObject.tag;
    }
    public void Update()
    {
        // Mouse sol tu�a bas�l�rsa veya bas�l� tutuluyorsa

        // E�er sald�r� durumunda de�ilsek, sald�r�y� ba�lat
        if (isAttackTriggered && !isAttackCooldown)
        {

            AttackCoroutine();
            StartAttackCooldown();
        }

        if (isAttackCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0)
            {
                isAttackCooldown = false;
                attackCooldownTimer = 0;
            }
        }
    }

    public void AttackCoroutine()
    {
        SoundManager.instance.PlayAttackSound();

        isAttackCooldown = true;
        attackCooldownTimer = attackAnimation.length;




        // Fare pozisyonuna bir ray g�nder
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Ray'in bir nesneye �arpmas� durumunda
        if (Physics.Raycast(ray, out hit, 4.4f))
        {
            // �arp�lan nesne TreeHealth bile�enine sahipse
            StoneOreHealth stoneOreHealth = hit.collider.GetComponent<StoneOreHealth>();
            if (stoneOreHealth != null)
            {
                // Hasar� uygula
                stoneOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            SteelOreHealth steelOreHealth = hit.collider.GetComponent<SteelOreHealth>();
            if (steelOreHealth != null)
            {
                // Hasar� uygula
                steelOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            OrichalcumOreHealth orichalcumOreHealth = hit.collider.GetComponent<OrichalcumOreHealth>();
            if (orichalcumOreHealth != null)
            {
                // Hasar� uygula
                orichalcumOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            MythrilOreHealth mythrilOreHealth = hit.collider.GetComponent<MythrilOreHealth>();
            if (mythrilOreHealth != null)
            {
                // Hasar� uygula
                mythrilOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            TitaniumOreHealth titaniumOreHealth = hit.collider.GetComponent<TitaniumOreHealth>();
            if (titaniumOreHealth != null)
            {
                // Hasar� uygula
                titaniumOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            GoldOreHealth goldOreHealth = hit.collider.GetComponent<GoldOreHealth>();
            if (goldOreHealth != null)
            {
                // Hasar� uygula
                goldOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }
            CoalOreHealth coalOreHealth = hit.collider.GetComponent<CoalOreHealth>();
            if (coalOreHealth != null)
            {
                // Hasar� uygula
                coalOreHealth.takeDamage(Random.Range(minTitaniumPickAxeDamage, maxTitaniumPickAxeDamage), transform.root.gameObject, titaniumPickaxeTag);
            }


        }
        // Sald�r�y� bitir

    }
    public void StartAttackCooldown()
    {
        // No need for a separate function for starting cooldown, it's handled inside Attack() function
    }
    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }
}
