using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationLevel : MonoBehaviour {
    //Global var
    //PUBLIC
    private GameObject player; //Position of player is required for instantiate new ground
    public GameObject ground; //Ground that we're going to duplicate
    //pattern of obstacles
    public GameObject MotifObstacle1; //size = 1
    public GameObject MotifObstacle2; //size = 1 
    public GameObject MotifObstacle3; //size = 1 
    public GameObject MotifObstacle4; //size = 1 
    public GameObject MotifObstacle5; //size = 1 
    public GameObject MotifObstacle6; //size = 2 
    public GameObject MotifObstacle7; //size = 2 
    public GameObject MotifObstacle8; //size = 1 
    public GameObject MotifObstacle9; //size = 2 
    public GameObject MotifObstacle10; //size = 2  
    public GameObject MotifObstacle11; //size = 1 
    public GameObject MotifObstacle12; //size = 1 
    public GameObject MotifObstacle13; //size = 2 
    public GameObject MotifObstacle14; //size = 2
    

    //PRIVATE
    private int nbGroundSeen = 0; //Number of ground where the player has been (0 at the beginning, we don't count the first one for technical reason)
    private int nbGroundCreate = 0; //Total number of generate ground
    private int nbGroundActual = 0; //Number of ground actually create
    private List<GroundLevel> listGround; //List that stock the grounds
    private Vector3 actualGroundScale; //Value of the actual ground scale (we're going to need the z value)
    private bool checkPlayerPositionResult;  //A bool that will say it a new ground is required by the player's position
    private Vector3 playerPosition;  //Player's position
    private bool autorisationDestruction = false; //A bool for indicate if the oldest ground can be destroyed or not
    private GroundLevel ground1;
    private List<GameObject> listObstaclesSize1;
    private List<GameObject> listObstaclesSize2;
    private int[] size1Values; //Value of obstacle of size1
    private int[] size2Values;  //Value of obstacle of size2




    // Use this for initialization
    void Start () {
        player = this.gameObject;
        playerPosition = player.transform.position;
        actualGroundScale = ground.transform.localScale*10; //we're  going to need to *10 cause of the different proportion between scale and position
        listGround = new List<GroundLevel>();
        listObstaclesSize1 = new List<GameObject>();
        listObstaclesSize2 = new List<GameObject>();
        size1Values = new int[] { 1, 2, 3, 4, 5, 8, 11, 12 };
        size2Values = new int[] { 6, 7, 9, 10, 13, 14 };

        //We add the obstacles to the list
        listObstaclesSize1.Add(MotifObstacle1);
        listObstaclesSize1.Add(MotifObstacle2);
        listObstaclesSize1.Add(MotifObstacle3);
        listObstaclesSize1.Add(MotifObstacle4);
        listObstaclesSize1.Add(MotifObstacle5);
        listObstaclesSize2.Add(MotifObstacle6);
        listObstaclesSize2.Add(MotifObstacle7);
        listObstaclesSize1.Add(MotifObstacle8);
        listObstaclesSize2.Add(MotifObstacle9);
        listObstaclesSize2.Add(MotifObstacle10);
        listObstaclesSize1.Add(MotifObstacle11);
        listObstaclesSize1.Add(MotifObstacle12);
        listObstaclesSize2.Add(MotifObstacle13);
        listObstaclesSize2.Add(MotifObstacle14);
    }
	
	// Update is called once per frame
	void Update () {
        //----------------------------------------------------------------------
        //Updating player's position
        playerPosition = player.transform.position;
        //----------------------------------------------------------------------
        //Chekin player's position
        checkPlayerPositionResult = CheckPlayerPosition();

        //----------------------------------------------------------------------
        //Generations of the ground
        //nbGroundActual <3 : is required for generates the ground at the beggining
        if (nbGroundActual < 3 || checkPlayerPositionResult)
        {
            GroundGeneration(); //erreur potentielle
        }

        //Debug info
        //Debug.Log("nbGroundSeen : " + nbGroundSeen + "  - nbGroundCreate : " + nbGroundCreate + "  - nbGroundActual : " + nbGroundActual);
    }

    //this function will create and destroy the ground
    public void GroundGeneration()
    {
        //Initialisation
        if (nbGroundCreate == 0)
        {
            //First ground
            //GameObject newGround1 = Instantiate(ground);
            GroundLevel ground1 = new GroundLevel(ground);
            nbGroundActual++;
            nbGroundCreate++;
            listGround.Add(ground1);  //We add it to our list
            Debug.Log("First ground : " + ground1.mainGroundPosition());

            //second ground
           // GameObject newGround2 = Instantiate(ground);
            Vector3 ground2Position = new Vector3(0,0,ground.transform.localScale.z*10);
            GroundLevel ground2 = new GroundLevel(ground,ground2Position,listObstaclesSize1,listObstaclesSize2,nbGroundSeen,nbGroundCreate,nbGroundActual); //erreur potentielle 
            nbGroundActual++;
            nbGroundCreate++;
            listGround.Add(ground2);  //We add it to our list
            Debug.Log("Second ground : " + ground2.mainGroundPosition());

            //third ground
            //GameObject newGround3 = Instantiate(ground);
            Vector3 ground3Position = new Vector3(0, 0, 2 * ground.transform.localScale.z * 10);
            GroundLevel ground3 = new GroundLevel(ground, ground3Position, listObstaclesSize1, listObstaclesSize2, nbGroundSeen, nbGroundCreate, nbGroundActual);
            nbGroundActual++;
            nbGroundCreate++;
            listGround.Add(ground3);  //We add it to our list
            Debug.Log("Third ground : " + ground3.mainGroundPosition());
        }

        //The player's position is enough to destroy an old ground and generate a new one
        if (checkPlayerPositionResult && autorisationDestruction) {
            //We destroy the oldest ground
            listGround[0].DestoyGroundLevel();
            listGround.RemoveAt(0);
            nbGroundActual--;
            autorisationDestruction = false; //We need to turn it off, of all the ground will be destroy, we need for the player to reach the next ground to turn it on true again
        }
        while (nbGroundActual < 3)
        {
            //generate one
            Vector3 newPosition = new Vector3(0, 0, nbGroundCreate * actualGroundScale.z ); //setting is new position 
            GroundLevel newGroundLevel = new GroundLevel(ground,newPosition,listObstaclesSize1,listObstaclesSize2,nbGroundSeen,nbGroundCreate,nbGroundActual); //The generation of a new ground will be call here //erreur potentielle
            nbGroundActual++;       //Update of the numbers
            nbGroundCreate++;
            listGround.Add(newGroundLevel);  //We add it to our list
        }
    }
    //Conseil de pochies mdr
    //Cree: instantiateobject
  


    //this funciton will check player's position, and return true if a new ground is needed
    //it will also update the nbGroundSeen
    public bool CheckPlayerPosition()
    {
        //Debug tool
       
        //The origin of a ground is in the middle, not the bottom of it. So we've already "cross" half the distance when the player spawn
        //update of number ground seen and indicate that we can delete the previous ground
        if(playerPosition.z>= actualGroundScale.z/2f + actualGroundScale.z * nbGroundSeen)
        {
            Debug.Log("nbGroundSeen++");
            nbGroundSeen++;
            autorisationDestruction = true;
        }
        //We need to know if the player as cross the first ground and is in the middle of the second one
        if (nbGroundSeen != 0)
        {
            if (playerPosition.z >= actualGroundScale.z * nbGroundSeen)
            {
                return true;
            }
        }
        return false;
    }
}
