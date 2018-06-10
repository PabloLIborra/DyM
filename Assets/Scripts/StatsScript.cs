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

    public float stamina = 5;
    public float staminaMax = 5;
    float multiplierStamina = 0.02f;
    public Scrollbar staminaBar;

    float useStamina = 0;
    float damage = 0;

    private void Start()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
        stamina = staminaMax;
        staminaBar.size = stamina / staminaMax;
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
        if (useStamina > 0 && (useStamina - (staminaMax * multiplierStamina)) > 0 && gameObject.tag == "player")
        {
            if (stamina - (staminaMax * multiplierStamina) >= 0)
            {
                stamina -= (staminaMax * multiplierStamina);
            }
            else
            {
                stamina = 0;
            }

            staminaBar.size = stamina / staminaMax;
            useStamina -= (staminaMax * multiplierStamina);
        }
        else
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
            stamina = (float)Math.Round(stamina, 0);
        }
    }

    public void Damage(float dmg)
    {
        damage += dmg;
    }

    public void UseStamina(float stm)
    {
        useStamina += stm;
    }

    public void Revive()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
        stamina = staminaMax;
        staminaBar.size = stamina / staminaMax;
    }

    public void ResetStamina()
    {
        stamina = staminaMax;
        staminaBar.size = stamina / staminaMax;
    }

}
