using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This class wil help us to display the score and gameOver
public class GameOver : MonoBehaviour
{
    public GameObject ground;
    public GameObject scrolling;
    private Vector3 playerPosition;
    private GameObject gameOverGround;
    private int score;
    public Text scoreText;
    public Text gameOverText;
    public GameRestart gamerestart;

    // Use this for initialization
    void Start()
    {
        scrolling.gameObject.SetActive(true);
        SetScoreText();
        gameOverText.text = "";
    }

    // Update is called once per frame
    //If the player has fall of the screen for a reason or hit the wall, it's game over
    void Update()
    {
        playerPosition = this.gameObject.transform.position;

        //We calcul the score
        score = (int)playerPosition.z;

        //We put in on the screen
        SetScoreText();

        //We check if it's gameOver
        if (playerPosition.y < 0)
        {
            gamerestart.instance.SetGameOver();
            EndGame();
        }

        
    }

    private void EndGame()
    {
        //We create a ground were we will put the player
        gameOverGround = Instantiate(ground);
        Vector3 newPosition = Vector3.zero;
        gameOverGround.transform.position = newPosition;

        //We put the player and desactivate him
        newPosition.y = 0.5f;
        this.gameObject.transform.position = newPosition;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.SetActive(false);

        //We desactivate the scrolling
        scrolling.gameObject.SetActive(false);

        //We display the score on the screen
        gameOverText.text = "Game Over ! \n Your Score is :" + score.ToString() + "\nPress anywere to restart";
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Scrolling")
        {
            EndGame();
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
