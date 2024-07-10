using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrichalcumPickAxe : MonoBehaviour
{
    [SerializeField] private int minOrichalcumPickAxeDamage = 49;
    [SerializeField] private int maxOrichalcumPickAxeDamage = 60;

    [SerializeField] private AnimationClip attackAnimation; // Saldýrý animasyonu
    starterAssets input;

    public bool isAttackTriggered = false;
    public float attackCooldownTimer = 0f;
    public float attackCooldown;
    bool isAttackCooldown = false;
    private string PickaxeTag;

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
        PickaxeTag = gameObject.tag;
    }
    public void Update()
    {
        // Mouse sol tuþa basýlýrsa veya basýlý tutuluyorsa

        // Eðer saldýrý durumunda deðilsek, saldýrýyý baþlat
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




        // Fare pozisyonuna bir ray gönder
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Ray'in bir nesneye çarpmasý durumunda
        if (Physics.Raycast(ray, out hit, 4.4f))
        {
            // Çarpýlan nesne TreeHealth bileþenine sahipse
            StoneOreHealth stoneOreHealth = hit.collider.GetComponent<StoneOreHealth>();
            if (stoneOreHealth != null)
            {
                // Hasarý uygula
                stoneOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
            SteelOreHealth steelOreHealth = hit.collider.GetComponent<SteelOreHealth>();
            if (steelOreHealth != null)
            {
                // Hasarý uygula
                steelOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
            OrichalcumOreHealth orichalcumOreHealth = hit.collider.GetComponent<OrichalcumOreHealth>();
            if (orichalcumOreHealth != null)
            {
                // Hasarý uygula
                orichalcumOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
            MythrilOreHealth mythrilOreHealth = hit.collider.GetComponent<MythrilOreHealth>();
            if (mythrilOreHealth != null)
            {
                // Hasarý uygula
                mythrilOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
            GoldOreHealth goldOreHealth = hit.collider.GetComponent<GoldOreHealth>();
            if (goldOreHealth != null)
            {
                // Hasarý uygula
                goldOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
            CoalOreHealth coalOreHealth = hit.collider.GetComponent<CoalOreHealth>();
            if (coalOreHealth != null)
            {
                // Hasarý uygula
                coalOreHealth.takeDamage(Random.Range(minOrichalcumPickAxeDamage, maxOrichalcumPickAxeDamage), transform.root.gameObject, PickaxeTag);
            }
        }
        // Saldýrýyý bitir

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
