using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{

    public float health = 50;
    public float healthMax = 50;
    public Scrollbar healthBar;

    float damage = 0;

    protected void Init()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
    }

    protected void update()
    {
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
        if(health == 0)
            Revive();
    }

    public void Damage(float dmg)
    {
        damage += dmg;
    }

    public void Revive()
    {
        health = healthMax;
        healthBar.size = health / healthMax;
    }

}
