using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public Maze maze;

    [HideInInspector]
    public Maze mazeInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartMaze();
        }
    }

    public void StartMaze() 
    {
        if (mazeInstance != null) 
        {
            Destroy(mazeInstance.gameObject);
        }       
        mazeInstance = Instantiate(maze);
        //mazeInstance.Generate();
        StartCoroutine(mazeInstance.GenerateInSteps());
    }
}
