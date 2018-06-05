using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScript : StatsScript
{

    float stamina = 50;
    public float staminaMax = 50;
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
        if (useStamina > 0)
        {
            if (stamina - 1 >= 0)
            {
                stamina -= 1;
            }
            else
            {
                stamina = 0;
            }

            staminaBar.size = stamina / staminaMax;
            useStamina--;
        }
        if (stamina == 0)
            ResetStamina();
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
