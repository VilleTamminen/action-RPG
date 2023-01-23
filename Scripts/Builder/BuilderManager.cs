using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.UI.Image;

/* Buttons that call SelectObject() must have layerMask object behind them because objects are lareay instantiated, 
 * even if buildable layerMask is not behind them, which would allow building in not suitable places.
 * */
public class BuilderManager : MonoBehaviour
{   
    public GameObject[] objects;
    private GameObject pendingObject;

    private Vector3 pos;
    private RaycastHit hit;

    [SerializeField] private LayerMask layerMask; //layerMask where object can be moved (Ground and BuildPoint)
    [SerializeField] private LayerMask layerMaskBuild; //layerMask where object can be build (BuildPoint)

    public GameObject player;
    public GameObject thirdPersonCamera;
    public GameObject buildCamera;
    public GameObject mainCamera;
    public GameObject dummyObject;
    public int currentIndex;

    void Start()
    {
        //Sets Player and camera objects inactive in scene
        player.SetActive(false);
        thirdPersonCamera.SetActive(false);
        buildCamera.SetActive(true);
        mainCamera.SetActive(false);
    }
    void Update()
    {
        if(pendingObject != null)
        {
            Ray rayPlace = Camera.main.ScreenPointToRay(Input.mousePosition);
            pendingObject.transform.position = pos;

            if (Input.GetMouseButtonDown(0) && Physics.Raycast(rayPlace, out hit, 1000, layerMaskBuild))
            {
                PlaceObject();
            }        
        }
    }

    public void PlaceObject()
    {
        //delete dummy instance. not very practical for memory?
        Destroy(pendingObject);
        //create real instance
        pendingObject = Instantiate(objects[currentIndex], pos, transform.rotation);
        pendingObject = null;
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //layerMask = Ground or BuildPoint
        if(Physics.Raycast(ray, out hit, 1000, layerMask)) 
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        currentIndex= index;
        //  pendingObject = Instantiate(objects[index], pos, transform.rotation);
        //objects with movement need dummy because their movement breaks them free of mouse pos
        pendingObject = Instantiate(dummyObject, pos, transform.rotation);
    }

    public void Play()
    {
        //Sets Player and camera objects active in scene
        if (player != null)
        {
            player.SetActive(true);
        }
        else
        {
            Debug.LogError("Player in BuildManager is null");
        }
        thirdPersonCamera.SetActive(true);
        mainCamera.SetActive(true);
        buildCamera.SetActive(false);
    }
}
