using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionScript : ActionScript
{

    public bool walkButton = false;
    public bool attackButton = false;
    public bool distAttackButton = false;

    GameObject healthBar = null;
    GameObject staminaBar = null;


    private void Start()
    {
        Init();
        players.Add(gameObject);
    }


    // Update is called once per frame
    void Update ()
    {
        if (!checkHealth() && gameObject.activeSelf == true)
        {
            CheckMouse();
            //WALK
            if (walkButton == true && Time.timeScale > 0 && turn)
            {
                if (!moving)
                {
                    FindGoTile();
                    CheckMouse();
                }
                else
                {
                    Move();
                    GameObject.FindGameObjectWithTag("Dist Attack Button").GetComponent<Button>().interactable = false;
                }

                if (lastMove == true && moving == false)
                {
                    walkButton = false;
                    lastMove = false;
                }
            }

            //MELE ATTACK
            if (attackButton == true && Time.timeScale > 0 && turn && meleAttackDo == false)
            {
                if (!attacking)
                {
                    FindAttackTile();
                    CheckMouse();
                }
                else
                {
                    MeleAttack();
                    GameObject.FindGameObjectWithTag("Dist Attack Button").GetComponent<Button>().interactable = false;
                }

                if (lastMove == true && attacking == false)
                {
                    attackButton = false;
                    lastMove = false;
                }
            }
            else if(meleAttackDo == true)
            {
                GameObject.FindGameObjectWithTag("Attack Button").GetComponent<Button>().interactable = false;
            }

            //DISTANCE ATTACK
            if (distAttackButton == true && Time.timeScale > 0 && turn)
            {
                if (!attacking)
                {
                    FindDistAttackTile();
                    CheckMouse();
                }
                else
                {
                    DistAttack();
                    GameObject.FindGameObjectWithTag("Attack Button").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("Walk Button").GetComponent<Button>().interactable = false;
                }

                if (lastMove == true && attacking == false)
                {
                    distAttackButton = false;
                    lastMove = false;
                }
            }
        }
        else if(gameObject.activeSelf == false)
        {
            currentTile.Restart(gameObject, true);
        }
    }

    public void clickedWalk()
    {
        if (!moving && attackButton == false && distAttackButton == false && Time.timeScale > 0 && turn)
        {
            if (walkButton == false)
            {
                walkButton = true;
            }
            else
            {
                walkButton = false;
                RemoveSelectableTiles();
            }
        }
    }

    public void clickedAttack()
    {
        if (!attacking && walkButton == false && distAttackButton == false && Time.timeScale > 0 && turn)
        {
            if (attackButton == false)
            {
                attackButton = true;
            }
            else
            {
                attackButton = false;
                RemoveSelectableTiles();
            }
        }
    }

    public void clickedDistAttack()
    {
        if (!attacking && walkButton == false && Time.timeScale > 0 && turn)
        {
            if (distAttackButton == false)
            {
                distAttackButton = true;
            }
            else
            {
                distAttackButton = false;
                RemoveSelectableTiles();
            }
        }
    }

    public void clickedTurn()
    {
        if (!moving && !attacking && Time.timeScale > 0 && turn)
        {
            this.GetComponent<StatsScript>().ResetStamina();
            TurnManager.EndTurn();
            attackButton = false;
            walkButton = false;
            distAttackButton = false;
            GameObject.FindGameObjectWithTag("Attack Button").GetComponent<Button>().interactable = true;
            GameObject.FindGameObjectWithTag("Dist Attack Button").GetComponent<Button>().interactable = true;
            GameObject.FindGameObjectWithTag("Walk Button").GetComponent<Button>().interactable = true;
        }
    }

    void CheckMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Tile" || hit.collider.tag == "Enemy")
                {
                    Tile t;
                    if (hit.collider.tag == "Enemy")
                    {
                        t = hit.collider.GetComponent<ActionScript>().currentTile;
                        if(t == null)
                        {
                            t = hit.collider.GetComponent<ActionScript>().actualTargetTile;
                        }
                        setActiveBar(hit.collider.gameObject, true);
                    }
                    else
                    {
                        t = hit.collider.GetComponent<Tile>();
                        if(t.npc != null && t.npc.tag == "Enemy")
                        {
                            setActiveBar(t.npc, true);
                        }
                        else if(!t.go && !t.attack &&
                            GameObject.FindGameObjectWithTag("Canvas Pause").GetComponent<Canvas>().enabled == false)
                        {
                            setActiveBar(null, false);
                        }
                    }
                    

                    if (t != null && t.go && !moving && turn)
                    {
                        MoveToTile(t);
                    }
                    else if(t != null && t.attack && !attacking && turn)
                    {
                        if(attackButton == true && distAttackButton == false)
                        {
                            MeleAttackToTile(t);
                        }
                        else if(attackButton == false && distAttackButton == true)
                        {
                            DistAttackToTile(t);
                        }
                    }
                }
                else if(hit.collider.tag == "player")
                {
                    Tile t = hit.collider.GetComponent<ActionScript>().currentTile;
                    if (t == null)
                    {
                        t = hit.collider.GetComponent<ActionScript>().actualTargetTile;
                    }
                    t.failAttack = true;
                }
            }
        }
    }

    public void setActiveBar(GameObject npc, bool active)
    {
        if(npc != null && active == true)
        {
            healthBar = npc.GetComponent<StatsScript>().healthBar.gameObject;
            healthBar.SetActive(true);
            staminaBar = npc.GetComponent<StatsScript>().staminaBar.gameObject;
            staminaBar.SetActive(true);
            npc.GetComponent<StatsScript>().updateVisibleBar();
        }
        else
        {
            if (healthBar != null)
            {
                healthBar.SetActive(false);
                healthBar = null;
            }
            if (staminaBar != null)
            {
                staminaBar.SetActive(false);
                staminaBar = null;
            }
        }
    }

    public static void ButtonTrackingFound()
    {
        GameObject turnButton = GameObject.FindGameObjectWithTag("Turn Button");
        if (turnButton != null)
        {
            turnButton.GetComponent<Button>().interactable = true;
        }
        GameObject attackButton = GameObject.FindGameObjectWithTag("Attack Button");
        if (attackButton != null)
        {
            attackButton.GetComponent<Button>().interactable = true;
        }
        GameObject walkButton = GameObject.FindGameObjectWithTag("Walk Button");
        if (walkButton != null)
        {
            walkButton.GetComponent<Button>().interactable = true;
        }
        GameObject[] rotateButton = GameObject.FindGameObjectsWithTag("Rotate Button");
        foreach (var rotate in rotateButton)
        {
            if (rotate != null)
            {
                rotate.GetComponent<Button>().interactable = true;
            }
        }
        GameObject warningTrack = GameObject.FindGameObjectWithTag("Warning");
        if (warningTrack != null)
        {
            warningTrack.GetComponent<Image>().enabled = false;
        }
    }

    public static void ButtonTrackingLost()
    {
        GameObject[] turnButton = GameObject.FindGameObjectsWithTag("Turn Button");
        foreach (var turn in turnButton)
        {
            if (turn != null)
            {
                turn.GetComponent<Button>().interactable = false;
            }
        }
        GameObject[] attackButton = GameObject.FindGameObjectsWithTag("Attack Button");
        foreach (var attack in attackButton)
        {
            if (attack != null)
            {
                attack.GetComponent<Button>().interactable = false;
            }
        }
        GameObject[] walkButton = GameObject.FindGameObjectsWithTag("Walk Button");
        foreach (var walk in walkButton)
        {
            if (walk != null)
            {
                walk.GetComponent<Button>().interactable = false;
            }
        }
        GameObject[] rotateButton = GameObject.FindGameObjectsWithTag("Rotate Button");
        foreach (var rotate in rotateButton)
        {
            if (rotate != null)
            {
                rotate.GetComponent<Button>().interactable = false;
            }
        }
        GameObject[] warningTrack = GameObject.FindGameObjectsWithTag("Warning");
        foreach (var warning in warningTrack)
        {
            if (warning != null)
            {
                warning.GetComponent<Image>().enabled = true;
            }
        }
    }

}
