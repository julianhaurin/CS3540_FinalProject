using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    public GameObject explosionVFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("Triggered by: " + collider.gameObject.name);
        Destroy(gameObject);
        Destroy(Instantiate(explosionVFX, transform.position, transform.rotation), 1);
    }
}
 