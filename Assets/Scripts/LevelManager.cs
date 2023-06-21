using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;

    public static bool isGameOver = false;
    public static float timer = 0;
    public bool isPaused = false;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    public string nextLevel;
    public string selectedSchool;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        timer +=Time.deltaTime;
    }

    public void LevelLost() 
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 1;
        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 2);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);
        
        Camera.main.GetComponent<AudioSource>().pitch = 2;
        AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    public void Pause()
    {
        print("paused");
        isPaused = true;
    }

    public void UnPause()
    {
        isPaused = false;
    }

    public void LoadFirstLevel()
    {
        // Logic for now
        SceneManager.LoadScene("EarthArena");

        // Logic for when we have all arenas
        /*
        if (selectedSchool == "WaterSchool")
        {
            SceneManager.LoadScene("WaterLevel1");
        }
        else if (selectedSchool == "FireSchool")
        {
            SceneManager.LoadScene("FireLevel1");
        }
        else if (selectedSchool == "AirSchool")
        {
            SceneManager.LoadScene("AirLevel1");
        }
        else if (selectedSchool == "EarthSchool")
        {
            SceneManager.LoadScene("EarthLevel1");
        }
        */
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
