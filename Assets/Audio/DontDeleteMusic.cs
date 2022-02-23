using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDeleteMusic : MonoBehaviour
{
    static DontDeleteMusic instance;
    private AudioSource audioSource;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;





    }
    // Start is called before the first frame update
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        audioSource = GetComponent<AudioSource>();
        


        // // Create a temporary reference to the current scene.
        // Scene currentScene = SceneManager.GetActiveScene();

        // // Retrieve the name of this scene.
        // string sceneName = currentScene.name;

        if (scene.name == "Arena")
        {
            audioSource.Pause();
        }

    }
}
