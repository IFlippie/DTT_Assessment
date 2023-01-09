using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    public Maze maze;
    public Text x, y;

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
            //StartMaze();
        }
    }

    //instantiate the maze DFS style
    public void StartDFS() 
    {
        if (mazeInstance != null) 
        {
            Destroy(mazeInstance.gameObject);
        }

        //checks the text but doesn't prevent against non number characters
        if (x.text != null && y.text != null) 
        {
            mazeInstance = Instantiate(maze);
            mazeInstance.size = new IntVector2(int.Parse(x.text), int.Parse(y.text));
            StartCoroutine(mazeInstance.Generate());
        }      
    }

    //instantiate the maze automata style
    public void StartAutomata()
    {
        if (mazeInstance != null)
        {
            Destroy(mazeInstance.gameObject);
        }

        if (x.text != null && y.text != null)
        {
            mazeInstance = Instantiate(maze);
            mazeInstance.size = new IntVector2(int.Parse(x.text), int.Parse(y.text));
            StartCoroutine(mazeInstance.GenerateAutomata());
        }
    }
}
