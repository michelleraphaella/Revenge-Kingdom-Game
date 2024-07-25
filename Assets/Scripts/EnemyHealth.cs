using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private int MAX_HEALTH = 100;
    [SerializeField] GameObject damageIndicatorPrefab;

    private List<GameObject> activeIndicators = new List<GameObject>(); // List to keep track of active indicators

    void Start()
    {
        health = MAX_HEALTH;
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator ShowDamageIndicator()
    {
        GameObject indicator = Instantiate(damageIndicatorPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        activeIndicators.Add(indicator); // Add the indicator to the list

        SpriteRenderer indicatorRenderer = indicator.GetComponent<SpriteRenderer>();

        // Optional: Add a fade-out effect
        Color originalColor = indicatorRenderer.color;
        float duration = 1.0f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            indicatorRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f - (t / duration));
            indicator.transform.position += new Vector3(0, Time.deltaTime, 0); // Move up over time
            yield return null;
        }

        activeIndicators.Remove(indicator); // Remove the indicator from the list
        Destroy(indicator);
    }

    public void SetHealth(int maxHealth, int health)
    {
        this.MAX_HEALTH = maxHealth;
        this.health = health;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }

        this.health -= amount;
        StartCoroutine(VisualIndicator(Color.red));
        StartCoroutine(ShowDamageIndicator());

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;
        StartCoroutine(VisualIndicator(Color.green));

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        Debug.Log("I am Dead!");

        // Destroy all active indicators
        foreach (GameObject indicator in activeIndicators)
        {
            Destroy(indicator);
        }
        activeIndicators.Clear();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("Player"))
            {
                Damage(10);
            }
        }
        // Check collision with heal
        else if (collision.gameObject.CompareTag("Heal"))
        {
            if (gameObject.CompareTag("Player"))
            {
                Heal(10);
                Destroy(collision.gameObject);
            }
        }
    }

}
