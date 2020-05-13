using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used for generate a ground based on the actual player's values
public class GroundLevel 
{
    //Global var
    //PUBLIC

    //PRIVATE
    //This need to be in the constructor
    private Vector3 actualGroundScale; //Value of the actual ground scale (we're going to need the z value)
    private GameObject mainGround;
    private List<GameObject> obstaclesList; //list of obstacles

    //Constructor - It will be used once, for the first ground because it as no obstacles
    public GroundLevel(GameObject ground)
    {
        //We set the values
        this.mainGround = UnityEngine.Object.Instantiate(ground);
        Vector3 position = new Vector3(0, 0, 0);
        actualGroundScale = ground.transform.localScale*10;
        this.mainGround.transform.position = position;
        obstaclesList = new List<GameObject>();
    }

    //Constructor with argument
    public GroundLevel(GameObject ground, Vector3 newGroundPosition, List<GameObject> listObstaclesSize1, List<GameObject> listObstaclesSize2, int nbGroundSeen, int nbGroundCreate, int nbGroundActual)
    {
        //we set the values 
        //this.mainGround = ground;
        this.mainGround = UnityEngine.Object.Instantiate(ground);
        this.mainGround.transform.position = newGroundPosition; //we set the position of the new ground
        this.obstaclesList = new List<GameObject>();
        actualGroundScale = ground.transform.localScale*10;
        

        //radom values for generation
        int nbZone;
        int nbObstacle;

        if (nbGroundSeen<15)
        {
            int add = (nbGroundSeen - (nbGroundSeen % 5)) / 5;
            nbZone = Random.Range(1 + add, 3 + add);
            nbObstacle = Random.Range(1 + add, 4 + add);
        }
        else
        {
            nbZone = 4;
            nbObstacle = Random.Range(4, 6);
        }

        //generation of the obstacles
        for (int i = 0; i < nbZone; i++)
        {
            //We're going to prepare the z values for the spawn of our object
            float zValueZone;
            float[] mult = new float[] { -0.5f, -0.25f, 0f, 0.25f };
            //zValueZone = newGroundPosition.z + ground.transform.localScale.z * mult[i];
            zValueZone = newGroundPosition.z + actualGroundScale.z * mult[i];

            //We're going to choose between obstacles of size 1 and size 2
            //If we can create an obstacle of size 2, ther's 50% of change of choosing it
            int j = 0;
            float newObstacleZPosition=zValueZone;  //Position of where we will place the new obstacle
            Vector3 obstaclePosition;
            int size1;
            int size2;

            //We will use this for check for conflict
            int previousObstacle = -1;
            int[]  size1Values = new int[] { 1, 2, 3, 4, 5, 8, 11, 12 };
            int[]  size2Values = new int[] { 6, 7, 9, 10, 13, 14 };
            while (j < nbObstacle)
            {
                //
                if (j + 2 <= 0)//We can create an obstacle of size 2
                {
                    int sizeOfObstacle = Random.Range(0, 2); //max is exclusive, we need to put max=2 if want to have at least two possible answer (0 and 1)
                    if (sizeOfObstacle == 0) //Obstacle of size2
                    {
                        //let's choos an obstacle of size 2 (6,7,9,10,13,14) that doesn't make conflict
                        size2 = Random.Range(0, listObstaclesSize2.Count );
                        //Check for conflict
                        while(!CheckConflict(previousObstacle, size2Values[size2]))
                        {
                            size2 = Random.Range(0, listObstaclesSize2.Count);

                        }
                        previousObstacle = size2Values[size2];

                        GameObject newObstacle = UnityEngine.Object.Instantiate(listObstaclesSize2[size2]); //We copy the new obstacle
                        obstaclePosition = new Vector3(0f, 0f, newObstacleZPosition);
                        newObstacle.transform.position = obstaclePosition; //Xe set the position of our obstacle
                        newObstacle.gameObject.SetActive(true); //  We activate the gameobject
                        this.obstaclesList.Add(newObstacle); //We add our new obstacle to the list
                        newObstacleZPosition += 4;  //We prepare the position for a new obstacle
                        j += 2;
                    }
                    else   //Obstacle of size 1
                    {
                        //let's choos an obstacle of size 1 (1,2,3,4,5,8,11,12) that doesn't make conflict
                        size1 = Random.Range(0, listObstaclesSize1.Count);
                        //Check for conflict
                        while (!CheckConflict(previousObstacle, size1Values[size1]))
                        {
                            size1 = Random.Range(0, listObstaclesSize1.Count);

                        }
                        previousObstacle = size1Values[size1];

                        GameObject newObstacle = UnityEngine.Object.Instantiate(listObstaclesSize1[size1]);
                        obstaclePosition = new Vector3(0f, 0f, newObstacleZPosition);
                        newObstacle.transform.position = obstaclePosition; //Xe set the position of our obstacle
                        newObstacle.gameObject.SetActive(true); //  We activate the gameobject
                        this.obstaclesList.Add(newObstacle); //We add our new obstacle to the list
                        newObstacleZPosition += 2;  //We prepare the position for a new obstacle
                        j++;
                    }
                }
                else  //We can't create an obstacle of size 2
                {
                    //let's choos an obstacle of size 1 (1,2,3,4,5,8,11,12) that doesn't make conflict
                    size1 = Random.Range(0, listObstaclesSize1.Count);
                    //Check for conflict
                    while (!CheckConflict(previousObstacle, size1Values[size1]))
                    {
                        size1 = Random.Range(0, listObstaclesSize1.Count);

                    }
                    previousObstacle = size1Values[size1];

                    GameObject newObstacle = UnityEngine.Object.Instantiate(listObstaclesSize1[size1]);
                    obstaclePosition = new Vector3(0f, 0f, newObstacleZPosition);
                    newObstacle.transform.position = obstaclePosition; //Xe set the position of our obstacle
                    newObstacle.gameObject.SetActive(true); //  We activate the gameobject
                    this.obstaclesList.Add(newObstacle); //We add our new obstacle to the list
                    newObstacleZPosition += 2;  //We prepare the position for a new obstacle
                    j++;
                }
            }
        }
    }

    //Destructor
    public void DestoyGroundLevel()
    {
        //I can't have monobehavior, so I can't destroy the object
        //I have to destroy them somewhere else
        //destruction of obstacles
        for(int i = 0; i < obstaclesList.Count; i++)
        {
            UnityEngine.Object.Destroy(obstaclesList[i]);
        }
        //destruction of the ground
        UnityEngine.Object.Destroy(mainGround);
    }

    //Main ground position
    public Vector3 mainGroundPosition()
    {
        return this.mainGround.transform.position;
    }

    public bool CheckConflict(int previousObstacle, int currentObstacle)
    {
        if(previousObstacle == -1)
        {
            return true;
        }
        if (previousObstacle == 3 && (currentObstacle == 12 || currentObstacle == 2))
        {
            return false;
        }
        if (previousObstacle == 2 && (currentObstacle == 11 || currentObstacle == 8 || currentObstacle == 1 || currentObstacle == 3))
        {
            return false;
        }
        if (previousObstacle == 11 && currentObstacle == 1)
        {
            return false;
        }
        if (previousObstacle == 8 && (currentObstacle == 4 || currentObstacle == 1))
        {
            return false;
        }
        return true;
    }
}
