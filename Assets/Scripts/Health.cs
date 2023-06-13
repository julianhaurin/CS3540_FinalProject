using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int startingHealth = 100;
    public Slider healthSlider;
    public AudioClip deadSFX;
    public bool player;
    public GameObject enemyDeath;

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
        if (currentHealth <= 0)
        {
            if (player) 
            {
                FindObjectOfType<LevelManager>().LevelLost();
            }
            else 
            {
                FindObjectOfType<LevelManager>().LevelBeat();
            }
        }
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
            Dies();
        }

        healthSlider.value = currentHealth;
        print("updated health" + currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shield") && gameObject.CompareTag("Enemy"))
        {
            Dies();
        }
    }

    public void TakeHealth(int healthAmount)
    {
        if(currentHealth < 100)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, 100);
        }
    }

    void Dies()
    {
        transform.Rotate(-90, 0, 0, Space.Self);

        if (!player)
        {
            Instantiate(enemyDeath, transform.position, transform.rotation);
            Destroy(gameObject, 0.5f);
        }
    }
}
