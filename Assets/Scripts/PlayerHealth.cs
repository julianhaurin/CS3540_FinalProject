using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;

    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDies();
        }

        healthSlider.value = currentHealth;
        print("updated health" + currentHealth);
    }

    public void TakeHealth(int healthAmount)
    {
        if(currentHealth < 100)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, 100);
        }
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead...");
        
        transform.Rotate(-90, 0, 0, Space.Self);
    }
}
