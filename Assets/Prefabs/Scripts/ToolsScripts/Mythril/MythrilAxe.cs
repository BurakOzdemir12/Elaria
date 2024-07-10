using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MythrilAxe : MonoBehaviour
{
    [SerializeField] private int minMythrilAxeDamage = 49;
    [SerializeField] private int maxMythrilAxeDamage = 60;

    [SerializeField] private AnimationClip attackAnimation; // Sald�r� animasyonu
    starterAssets input;

    public bool isAttackTriggered = false;
    public float attackCooldownTimer = 0f;
    public float attackCooldown;
    bool isAttackCooldown = false;
    private string axeTag;

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
        axeTag = gameObject.tag;
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
            TreeHealth treeHealth = hit.collider.GetComponent<TreeHealth>();
            if (treeHealth != null)
            {
                // Hasar� uygula
                treeHealth.takeDamage(Random.Range(minMythrilAxeDamage, maxMythrilAxeDamage), transform.root.gameObject, axeTag);
            }
            BirchTreeHealth birchtreeHealth = hit.collider.GetComponent<BirchTreeHealth>();
            if (birchtreeHealth != null)
            {
                // Hasar� uygula
                birchtreeHealth.takeDamage(Random.Range(minMythrilAxeDamage, maxMythrilAxeDamage), transform.root.gameObject, axeTag);
            }
            NormalTreeHealth normalTreeHealth = hit.collider.GetComponent<NormalTreeHealth>();
            if (normalTreeHealth != null)
            {
                // Hasar� uygula
                normalTreeHealth.takeDamage(Random.Range(minMythrilAxeDamage, maxMythrilAxeDamage), transform.root.gameObject, axeTag);
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
