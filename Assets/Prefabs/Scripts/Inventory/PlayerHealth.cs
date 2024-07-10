using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health = 100;
    [SerializeField] private int food = 0;
    [SerializeField] private int stamina = 0;
    public bool isDeath = false;
    public static PlayerHealth instance { get; set; }


    Animator animator;
    bool isGetHitCooldown = false;
    float getHitCooldown = .5f;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public void takeDamage(int decreaseHealth)
    {
        if (isGetHitCooldown) return;

        health -= decreaseHealth;
        animator.SetTrigger("isGetHit");


        Debug.Log(health);
        StartCoroutine(HitCooldown());
    }
   
    IEnumerator HitCooldown()
    {
        isGetHitCooldown = true;
        yield return new WaitForSeconds(getHitCooldown);
        isGetHitCooldown = false;
    }

    public void Update()
    {
        if (health <= 0 && !animator.GetBool("isDeath"))
        {
            animator.SetBool("isDeath", true);
            isDeath = true;
            health= 0;
        }
    }

    public void addHealth(int addedHealth)
    {
        health += addedHealth;
    }

    public void addFood(int addedFood)
    {
        food += addedFood;
    }

    public void addStamina(int addedStamina)
    {
        stamina += addedStamina;
    }
}
