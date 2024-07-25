using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        if (attackArea != null)
        {
            attackArea.SetActive(false);
            Debug.Log("Attack area initially set to inactive");
        }
        else
        {
            Debug.LogError("Attack area not found as the first child of the player object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !attacking)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false);
            }

        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(true);
    }
}