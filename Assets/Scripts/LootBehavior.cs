using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{

    public int healthBonus = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 90 * Time.deltaTime);

        if(transform.position.y < Random.Range(1.0f, 3.0f))
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            var playerHealth = other.GetComponent<Health>();

            playerHealth.TakeHealth(healthBonus);
            Debug.Log("Added Health");

            Destroy(gameObject);
        }
    }
}
