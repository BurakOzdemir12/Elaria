using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : NetworkBehaviour
{
    Animator animator;
    private GameObject playerObj = null;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentSprintMovement;

    bool isWalkBackwardPressed;
    bool isWalkingBackwardFast;
    bool isMovementPressed;
    bool isSprintPressed;
    bool isRunningJumpPressed;
    public bool isAttackTriggered;

    float rotationFactorPerFrame = 1.0f;
    float attackCooldown = 1.5f;
    bool isAttackCooldown = false;
    starterAssets input;

    public static CharacterMovement Instance { get; set; }

    Inventory inventory;

    public void Awake()
    {
        input = new starterAssets();
        input.Player.Move.started += onMovementInput;
        input.Player.Move.canceled += onMovementInput;
        input.Player.Move.performed += onMovementInput;

        input.Player.Sprint.started += onSprint;
        input.Player.Sprint.canceled += onSprint;
        input.Player.Sprint.performed += onSprint;

        input.Player.Jump.started += onRunningJump;
        input.Player.Jump.canceled += onRunningJump;
        input.Player.Jump.performed += onRunningJump;

        input.Player.Attack.started += onAttack;
        input.Player.Attack.canceled += onAttack;
        input.Player.Attack.performed += onAttack;

        animator = GetComponent<Animator>();
    }

    void onAttack(InputAction.CallbackContext ctx)
    {
        isAttackTriggered = ctx.ReadValueAsButton();
    }

    void onRunningJump(InputAction.CallbackContext ctx)
    {
        isRunningJumpPressed = ctx.ReadValueAsButton();
    }

    void onSprint(InputAction.CallbackContext ctx)
    {
        isSprintPressed = ctx.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext ctx)
    {
        currentMovementInput = ctx.ReadValue<Vector2>();

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        currentSprintMovement.x = currentMovement.x;
        currentSprintMovement.z = currentMovement.z;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    public void Start()
    {
        if (playerObj == null)
            playerObj = GameObject.Find("BasicMan");
        playerObj = GameObject.Find("HumanMaleCharacter");
    }

    public void Update()
    {
        if (!IsOwner) return;


        isWalkingBackwardFast = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S);
        isWalkBackwardPressed = Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftShift);

        

        if (!isAttackCooldown && isAttackTriggered && PlayerHealth.instance.isDeath == false)
        {
            StartCoroutine(Attack());
            StartCoroutine(StartAttackCooldown());
        }

        handleAnimation();
    }

    void handleAnimation()
    {
        bool isWalkBack = animator.GetBool("isWalkBack");
        bool isWalkBackFast = animator.GetBool("isWalkBackFast");
        bool isRunning = animator.GetBool("isRunning");
        bool isRunningFast = animator.GetBool("isRunningFast");
        bool isRunningJump = animator.GetBool("isRunningJump");

        // Normal Running
        if (isMovementPressed && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else if (!isMovementPressed && isRunning)
        {
            animator.SetBool("isRunning", false);
        }

        // Walking Backward
        if (isMovementPressed && isWalkBackwardPressed)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningFast", false);
            animator.SetBool("isWalkBack", true);
        }
        else if (!isWalkBackwardPressed)
        {
            animator.SetBool("isWalkBack", false);
        }

        // Walking Backward Fast
        if (isMovementPressed && isWalkingBackwardFast)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningFast", false);
            animator.SetBool("isWalkBack", true);
            animator.SetBool("isWalkBackFast", true);
            animator.speed = 1.5f; // Increase animation speed
        }
        else if (!isWalkingBackwardFast)
        {
            animator.SetBool("isWalkBackFast", false);
            animator.speed = 1.0f; // Reset animation speed
        }

        // Sprinting
        if ((isMovementPressed && isSprintPressed) && !isRunningFast && !Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isRunningFast", true);
        }
        if ((!isMovementPressed || !isSprintPressed) && isRunningFast)
        {
            animator.SetBool("isRunningFast", false);
        }

        // Running Jump
        if ((isSprintPressed && isRunningJumpPressed))
        {
            animator.SetBool("isRunningJump", true);
        }
        if (!isRunningJumpPressed && isRunningJump)
        {
            animator.SetBool("isRunningJump", false);
        }
    }

    public IEnumerator Attack()
    {
        if (isAttackTriggered)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
            animator.SetTrigger("isAttack");
        }
        else
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        }

        yield return null;
    }
   
    public IEnumerator StartAttackCooldown()
    {
        isAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackCooldown = false;
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
