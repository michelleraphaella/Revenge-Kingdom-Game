using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private int MAX_HEALTH = 100;
    [SerializeField] Healthbar healthBar;
    [SerializeField] GameObject damageIndicatorPrefab;
    [SerializeField] GameObject healIndicatorPrefab;
    [SerializeField] List<GameObject> hidePotions; // List of game objects to hide

    private List<GameObject> activeIndicators = new List<GameObject>(); // List to keep track of active indicators
    private int healCount = 0; // Track the number of times the player has healed
    private int maxHeals; // Maximum number of heals allowed in the current scene

    public delegate void OnPlayerDeath();
    public event OnPlayerDeath PlayerDeath;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Healthbar>();
    }

    private void Start()
    {
        health = MAX_HEALTH;
        healthBar.UpdateHealthBar(health, MAX_HEALTH);
        healthBar.offset = new Vector3(0, 2, 0);

        // Determine the maximum number of heals based on the current scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 6)
        {
            maxHeals = 1;
        }
        else if (sceneIndex == 12)
        {
            maxHeals = 3;
        }
    }

    private void Update()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 6)
            {
                SceneManager.LoadScene(7, LoadSceneMode.Single);
            }
            else if (sceneIndex == 12)
            {
                SceneManager.LoadScene(14, LoadSceneMode.Single);
            }
        }

        // Detect "H" key press
        if (Input.GetKeyDown(KeyCode.H) && healCount < maxHeals)
        {
            Heal(10);
            healCount++; // Increment the heal count

            // Hide the specific sprite one by one
            if (hidePotions.Count > 0)
            {
                hidePotions[0].SetActive(false);
                hidePotions.RemoveAt(0); // Remove the hidden potion from the list
            }
        }
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

    private IEnumerator ShowHealIndicator()
    {
        GameObject indicator = Instantiate(healIndicatorPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
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
        healthBar.UpdateHealthBar(health, maxHealth);
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
        healthBar.UpdateHealthBar(health, MAX_HEALTH);

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
        StartCoroutine(ShowHealIndicator()); // Show heal indicator

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }

        healthBar.UpdateHealthBar(health, MAX_HEALTH);
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

        if (PlayerDeath != null)
        {
            PlayerDeath.Invoke();
        }

        Destroy(gameObject);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 6)
        {
            SceneManager.LoadScene(10, LoadSceneMode.Single);
        }
        else if (sceneIndex == 12)
        {
            SceneManager.LoadScene(13, LoadSceneMode.Single);
        }
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
    }
}
