using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    public GameObject explosionVFX;
    public AudioClip shootSFX;
    public AudioClip destroySFX;

    // Start is called before the first frame update
    void Start()
    { 
        AudioSource.PlayClipAtPoint(shootSFX, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Triggered by: " + collider.gameObject.name);
        AudioSource.PlayClipAtPoint(destroySFX, transform.position);
        Destroy(gameObject);
        Destroy(Instantiate(explosionVFX, transform.position, transform.rotation), 1);
    }
}
 