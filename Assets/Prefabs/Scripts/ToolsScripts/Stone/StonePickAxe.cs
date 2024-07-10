
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class StonePickAxe : MonoBehaviour
{
    [SerializeField] private int minstonePickAxeDamage = 5;
    [SerializeField] private int maxstonePickAxeDamage = 20;

    [SerializeField] private AnimationClip attackAnimation; // Saldýrý animasyonu
    starterAssets input;

    public bool isAttackTriggered = false;
    public float attackCooldownTimer = 0f;
    public float attackCooldown;
    bool isAttackCooldown = false;
    private string stonePickaxeTag;

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
        stonePickaxeTag = gameObject.tag;
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
                stoneOreHealth.takeDamage(Random.Range(minstonePickAxeDamage, maxstonePickAxeDamage), transform.root.gameObject, stonePickaxeTag);
            } 
            SteelOreHealth steelOreHealth = hit.collider.GetComponent<SteelOreHealth>();
            if (steelOreHealth != null)
            {
                // Hasarý uygula
                steelOreHealth.takeDamage(Random.Range(minstonePickAxeDamage, maxstonePickAxeDamage), transform.root.gameObject, stonePickaxeTag);
            }
            CoalOreHealth coalOreHealth = hit.collider.GetComponent<CoalOreHealth>();
            if (coalOreHealth != null)
            {
                // Hasarý uygula
                coalOreHealth.takeDamage(Random.Range(minstonePickAxeDamage, maxstonePickAxeDamage), transform.root.gameObject, stonePickaxeTag);
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
