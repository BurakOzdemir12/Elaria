using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class AxeAnimation : MonoBehaviour
{
    Animator animator;
    private GameObject playerObj = null;

    public static AxeAnimation Instance { get; set; }

    public bool isAttackTriggered = false;
    public float attackCooldown = 1.5f; // Cooldown duration for the attack
    float attackCooldownTimer = 0f;
    bool isAttackCooldown = false;
    starterAssets input;


    void Awake()
    {
        /*if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }*/

        input = new starterAssets();

        input.Player.Attack.started += onAttack;
        input.Player.Attack.canceled += onAttack;
        input.Player.Attack.performed += onAttack;

        animator = GetComponent<Animator>();
    }

    void onAttack(InputAction.CallbackContext ctx)
    {
        isAttackTriggered = ctx.ReadValueAsButton();
    }

    void Update()
    {
        if (isAttackTriggered && !isAttackCooldown)
        {
            Attack();
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

    public void Attack()
    {
        if (isAttackTriggered)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Tool Attack Layer"), 1);
            animator.SetTrigger("isAttack");
            isAttackCooldown = true;
            attackCooldownTimer = attackCooldown;
        }
        else
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Tool Attack Layer"), 0);
        }
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
