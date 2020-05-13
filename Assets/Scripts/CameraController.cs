using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; //Le joueur

    private Vector3 offset; //Le vector pour déplacer la caméra

    void Start()
    {
        offset = transform.position - player.transform.position; //différence camera baballe
    }

    void LateUpdate() //lateUpdate au lieu de updatecar on est sur que tout les calculs sur la baballe ont été effectués
    {
        transform.position = new Vector3(offset.x, offset.y, offset.z + player.transform.position.z); //dire que la caméra se trouve baballe + vector
    }
}