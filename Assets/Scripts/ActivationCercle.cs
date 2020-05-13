using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationCercle : MonoBehaviour
{
    //Variables globales
    public GameObject cercle; //On va référencer le cercle afin de pouvoir le désactiver
    public GameObject viseur; //Sphère qui sert de viseur
    public GameObject indicateurViseur; //repère visuel entre le viseur et le le centre du cercle
    private Camera mainCam; //nous servira pour avoir la valeur de la souris dans les coordonnées du jeu
    static Vector3 pointSouris; //Coord de la souris dans la dimension du jeu
    private float tailleIndicateurViseur; //Nous serviras pour déplacer le joueur
    private float rotationIndicateurViseur; //Nous serivras pour déplacer le joueur
    private Rigidbody rbPlayer; //Player's rigifbody (for addforce)
    private Vector3 lastPosition = Vector3.zero;// It will be usefull for controll the speed of the ball
    private float actualSpeed;
    private float maxSpeed = 0.7f;
    
    // Use this for initialization
    void Start()
    {
        //Cam
        mainCam = Camera.main;

        //Player
        rbPlayer = this.GetComponent<Rigidbody>();
    }



    //Nous servira pour avoir la valeur de la souris dans les coordonnées du jeu
    void OnGUI()
    {
        //Mouse
        pointSouris = new Vector3();

        //ScreenToWorldPoint receives a Vector3 argument where x and y are the screen coordinates, 
        //and z is the distance from the camera. Since Input.mousePosition.z is always 0, what you're getting is the camera position.
        // The mouse position in the 2D screen corresponds to a line in the 3D world passing through the camera center and the mouse pointer, 
        //thus you must somehow select which point in this line you're interested in - that's why you must pass the distance from the camera in z.
        var mousePos = Input.mousePosition;
        mousePos.z = mainCam.gameObject.transform.position.y; // select distance = 10 units from the camera
        pointSouris = mainCam.ScreenToWorldPoint(mousePos);

        //Affichage pour débug dans la fenetre de jeu
        //GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        //GUILayout.Label("Screen pixels: " + mainCam.pixelWidth + ":" + mainCam.pixelHeight);
        //GUILayout.Label("Mouse position: " + mousePos);
        //GUILayout.Label("World position: " + pointSouris.ToString("F3"));
        //GUILayout.EndArea();
    }



    //Fonction permettant de définir les coordonnées du viseur
    private Vector3 setViseurCoord()
    {
        Vector3 coord = new Vector3(); //les coordonnées que l'on va renvoyer
        Vector3 positionCercle = cercle.gameObject.transform.position;
        Vector3 tailleCercle = cercle.transform.localScale;
        float rayon = (tailleCercle.x) / 2f;
        float distanceSourisCentreCercle = Mathf.Sqrt(Mathf.Pow(pointSouris.x - positionCercle.x, 2f) + Mathf.Pow(pointSouris.z - positionCercle.z, 2f));
        if (distanceSourisCentreCercle > rayon) // On va regarder si la souris est en dehors du cercle
        {
            Vector3 distanceSourisCerle = pointSouris - positionCercle; //distance séparant la souris du cercle
            distanceSourisCerle = Vector3.ClampMagnitude(distanceSourisCerle, rayon);
            coord = this.gameObject.transform.position + distanceSourisCerle;
        }
        else
        {
            coord = pointSouris;
        }
        coord.y = this.gameObject.transform.position.y; //on met y a la bonne hauteur
        return coord;
    }



    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------
        //a chaque update on déplace le cercle et l'indicateur viseur suivant les coord du joueur
        cercle.gameObject.transform.position = this.gameObject.transform.position;
        indicateurViseur.gameObject.transform.position = this.gameObject.transform.position;

        //----------------------------------------------------------------------
        //On va regarder si le clique gauche est enclenché (activation du viseur dans ce cas)
        if (Input.GetMouseButton(0))
        {
            //On ralentit le temps
            Time.timeScale = 0.5f;
            //Activation des éléments
            cercle.gameObject.SetActive(true);
            viseur.gameObject.SetActive(true);
            indicateurViseur.gameObject.SetActive(true);

            //---------------------------------------------
            //Calcul coordonnées de la souris
            Vector3 positionSourisEcran = Input.mousePosition;
            Vector3 positionSourisJeu = mainCam.ScreenToWorldPoint(positionSourisEcran); //mainCamera a associer à ta caméra principale

            //---------------------------------------------
            //Calcul coordonnées du viseur
            viseur.gameObject.transform.position = setViseurCoord(); //on affecte sa position au viseur
            Vector3 positionViseur = viseur.gameObject.transform.position;

            //---------------------------------------------
            //Calcul coordonnées de l'indicateur du viseur (on calcule sa taille, puis on le fait tourner)
            //Taille
            tailleIndicateurViseur = Mathf.Sqrt(Mathf.Pow(positionViseur.x - cercle.gameObject.transform.position.x, 2f) + Mathf.Pow(positionViseur.z - cercle.gameObject.transform.position.z, 2f));
            indicateurViseur.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, tailleIndicateurViseur);

            //Rotation on fait en sorte que l'indicateur pointe sur le viseur puis on le sauvegarde
            indicateurViseur.gameObject.transform.LookAt(viseur.gameObject.transform); //Le viseur pointe à l'opposé, on le retourne
            var rotationVector = indicateurViseur.gameObject.transform.rotation.eulerAngles;
            rotationVector.y = indicateurViseur.gameObject.transform.rotation.eulerAngles.y + 180;
            indicateurViseur.gameObject.transform.rotation = Quaternion.Euler(rotationVector);

            //---------------------------------------------
            //Debug.log


        }
        else
        { //Si la souris n'est pas enclenché, on désactive les objets
            cercle.gameObject.SetActive(false);
            viseur.gameObject.SetActive(false);
            indicateurViseur.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }


        //----------------------------------------------------------------------
        if (Input.GetMouseButtonUp(0)) //On regarde si le bouton viens d'être relaché
        {
            MovementPlayer();
        }
    }

    //We will check the speed of the ball here
    void FixedUpdate()
    {
        actualSpeed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    //We check if the speed is too high or not for adding it to the player
    private void MovementPlayer()
    {
        if (actualSpeed > maxSpeed){
            Debug.Log("max speed");
            return;
        }
        else
        {
            Debug.Log("speed add");
            Vector3 distanceViseurJoueur = this.gameObject.transform.position - viseur.gameObject.transform.position;
            rbPlayer.AddForce(distanceViseurJoueur * 100);

        }
        Debug.Log("actual speed = " + actualSpeed);
        
    }

}
