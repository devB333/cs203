using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GrappleManager : MonoBehaviour
{

    public static GrappleManager instance;// singleton to access one list

    public List<GrapplePoint> grapplePoints = new List<GrapplePoint>();// list to hold all grapple point objects

    //method that loads the singelton
    private void Awake()
    {
        if (instance == null)// if the singletion is null then set curr obj = to it
            instance = this;
        else
            Destroy(gameObject);// else destory curr obj if singleton exists
    }

    public void LoadPoints()// method to load all grapple points into the list
    {
        grapplePoints.Clear();// clear the list first to allow for constant checking
        GrapplePoint[] found = FindObjectsOfType<GrapplePoint>();// finds all grapple points in the scene
        foreach (var gp in found)
            grapplePoints.Add(gp);// add the grapplePoint to the grapplePoints List
        Debug.Log($"GrappleManager: Loaded {grapplePoints.Count} grapple points");

    }

    private void Start()
    {
        // initial population for the current scene
        LoadPoints();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // refresh when a scene is loaded
        LoadPoints();
    }

}
