using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OpenWaterArena()
    {
        Debug.Log("opening water arena...");
        SceneManager.LoadScene("WaterArena");
    }

    public void OpenEarthArena() {
        Debug.Log("opening earth arena...");
        SceneManager.LoadScene("EarthArena");
    }
}
