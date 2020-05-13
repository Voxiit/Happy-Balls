using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameRestart : MonoBehaviour
{
    public GameRestart instance;

    private void Awake()
    {
        instance = this;
    }

    private bool gameOver = false;

    public void SetGameOver()
    {
        gameOver = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameOver && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown("r")))
        {
            Restart();
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("game"); //Load scene called Game
    }
}
