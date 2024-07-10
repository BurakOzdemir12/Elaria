
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SteelAxe : MonoBehaviour
{
    [SerializeField] private int minSteelAxeDamage = 5;
    [SerializeField] private int maxSteelAxeDamage = 20;

    [SerializeField] private AnimationClip attackAnimation; // Saldýrý animasyonu
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
            TreeHealth treeHealth = hit.collider.GetComponent<TreeHealth>();
            if (treeHealth != null)
            {
                // Hasarý uygula
                treeHealth.takeDamage(Random.Range(minSteelAxeDamage, maxSteelAxeDamage), transform.root.gameObject, axeTag);
            }
            BirchTreeHealth birchtreeHealth = hit.collider.GetComponent<BirchTreeHealth>();
            if (birchtreeHealth != null)
            {
                // Hasarý uygula
                birchtreeHealth.takeDamage(Random.Range(minSteelAxeDamage, maxSteelAxeDamage), transform.root.gameObject, axeTag);
            }
            NormalTreeHealth normalTreeHealth = hit.collider.GetComponent<NormalTreeHealth>();
            if (normalTreeHealth != null)
            {
                // Hasarý uygula
                normalTreeHealth.takeDamage(Random.Range(minSteelAxeDamage, maxSteelAxeDamage), transform.root.gameObject, axeTag);
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
