using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{

    public float health = 50;
    public float healthMax = 50;
    float multiplierHealth = 0.02f;
    public Scrollbar healthBar;

    public int stamina = 5;
    public int staminaMax = 5;
    public Image staminaBar;
    Text staminaText;

    int useStamina = 0;
    float damage = 0;

    public int attackDmg = 1;

    private void Start()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
        stamina = staminaMax;
        staminaText = staminaBar.gameObject.transform.GetChild(0).GetComponent<Text>();
        staminaText.text = stamina.ToString();
    }

    private void Update()
    {
        //DAMAGE
        if (damage > 0 && (damage - (healthMax * multiplierHealth)) >= 0)
        {
            if (health - (healthMax * multiplierHealth) >= 0)
            {
                health -= (healthMax * multiplierHealth);
            }
            else
            {
                health = 0;
            }

            healthBar.size = health / healthMax;
            damage -= (healthMax * multiplierHealth);
        }
        else
        {
            if(health - damage >= 0)
            {
                health -= damage;
            }
            else
            {
                health = 0;
            }
            damage = 0;
            health = (float)Math.Round(health, 0);
        }
        //STAMINA        
        if (useStamina > 0 && stamina - useStamina >= 0)
        {
            stamina--;
            useStamina--;
            staminaText.text = stamina.ToString();
        }
        else if(stamina - useStamina < 0)
        {
            if (stamina - useStamina >= 0)
            {
                stamina -= useStamina;
            }
            else
            {
                stamina = 0;
            }
            useStamina = 0;
            staminaText.text = stamina.ToString();
        }
    }

    public void Damage(float dmg)
    {
        damage += dmg;
    }

    public void UseStamina(int stm)
    {
        useStamina += stm;
    }

    public void Revive()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
        stamina = staminaMax;
        staminaText.text = stamina.ToString();
    }

    public void ResetStamina()
    {
        stamina = staminaMax;
        staminaText.text = stamina.ToString();
    }

    public float getDamage()
    {
        return damage;
    }

    public void updateVisibleBar()
    {
        if(gameObject.tag == "Enemy")
        {
            healthBar.size = health / healthMax;
            staminaText.text = stamina.ToString();
        }
    }
}
