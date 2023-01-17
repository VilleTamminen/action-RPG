using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //This script is placed on GameManager object
    //code from Brackeys enemy AI RPG ep 10
    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
        if(instance == null)
        {
            Debug.Log("Error: playerManager instance is null. ", this);
        }
    }
    void Start()
    {
        instance = this;
        if (instance == null)
        {
            Debug.Log("Error: playerManager instance is null. ", this);
        }
    }

    #endregion

    public GameObject player;
}
