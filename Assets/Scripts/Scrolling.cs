using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used for moving the scrolling
public class Scrolling : MonoBehaviour
{
    public GameObject player;
    private float speed; //speed of the scrolling
    private float playerPositionZ; //Players position (wee will be used for upgrading the speed)
    private int count; // Will be used as a speed multiplicator

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
        count = 1;
        speed = 5;
        getPlayerPosition();
    }

    

    void Update()
    {
        getPlayerPosition();
        if(playerPositionZ >= 40f * (float)count)
        {
            count++;
            speed += 1f;
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void getPlayerPosition()
    {
        playerPositionZ = player.transform.position.z;
    }
}
