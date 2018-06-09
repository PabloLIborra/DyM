using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{

    public float health = 50;
    public float healthMax = 50;
    public Scrollbar healthBar;

    public float stamina = 5;
    public float staminaMax = 5;
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
        if(damage > 0)
        {
            if (health - (healthMax * 0.01f) >= 0)
            {
                health -= (healthMax * 0.01f);
            }
            else
            {
                health = 0;
            }

            healthBar.size = health / healthMax;
            damage -= (healthMax * 0.01f);
        }
        //STAMINA
        if (useStamina > 0 && (useStamina - (staminaMax * 0.01f)) > 0)
        {
            if (stamina - (staminaMax * 0.01f) >= 0)
            {
                stamina -= (staminaMax * 0.01f);
            }
            else
            {
                stamina = 0;
            }

            staminaBar.size = stamina / staminaMax;
            useStamina -= (staminaMax * 0.01f);
        }
        else
        {
            useStamina = 0;
            stamina = (float)Math.Round(stamina, 0);
        }
        /*if (health == 0)
            Revive();*/
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

}
