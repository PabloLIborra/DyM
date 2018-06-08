using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScript : StatsScript
{

    public float stamina = 5;
    public float staminaMax = 5;
    public Scrollbar staminaBar;

    float useStamina = 0;

    private void Start()
    {
        Init();
        stamina = staminaMax;
        staminaBar.size = stamina / staminaMax;
    }

    private void Update()
    {
        update();
        if (useStamina > 0 && (useStamina -(staminaMax * 0.01f)) > 0)
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
        /*
        if (stamina == 0)
        {
            ResetStamina();
        }*/
    }

    public void UseStamina(float stm)
    {
        useStamina += stm;
    }

    public void ResetStamina()
    {
        stamina = staminaMax;
        staminaBar.size = stamina / staminaMax;
    }
}
