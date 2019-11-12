using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNavigator : MonoBehaviour
{
    private LoadMaze mazeLoader;
    private int t = 0; //current floor
    private MazeCell[][,] loadedMaze;
    private GameObject lastTriggered; //to check if the player tries to reenter the same cell
    private GameObject mazeEntranceTrigger;

    private MazeCell mazeExitCell;
    private MazeCellGraph mazeGraph;

    public GameObject playerController;
    public GameObject pathIndicator;
    public GameObject exitIndicator;
    private GameObject instanceOfPathIndicator;
    private GameObject instanceOfExitIndicator;

    private bool insideMaze = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializeMazeNavigator();

        //TODO: make a path
        mazeGraph = new MazeCellGraph();
        mazeGraph.BFS(loadedMaze, loadedMaze[0][mazeLoader.mazeEntrance.x, mazeLoader.mazeEntrance.y]);

        ChooseRandomExit();
        mazeExitCell = loadedMaze[mazeLoader.getMazeHeight() - 1][mazeLoader.getMazeExit().x, mazeLoader.getMazeExit().y];
        //instantiate exit beam
        instanceOfExitIndicator = Instantiate(exitIndicator, new Vector3(mazeExitCell.coordinates.x * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2, 0, mazeExitCell.coordinates.z * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2), Quaternion.identity);
        RedrawPath();
    }


    //TODO: choose random exit does not work properly.
    private void ChooseRandomExit()
    {
        for (int i = 0; i < mazeLoader.getMazeSize(); i++)
        {
            if (loadedMaze[15][i, mazeLoader.getMazeSize() - 1].predecessor != new Vector2Int(0, 0))
            {
                if( i > mazeLoader.getMazeSize() / 2)
                {
                    mazeLoader.setMazeExit(new Vector2Int(i, mazeLoader.getMazeSize() - 1));
                    break;
                }
            }
        }
    }

    private void Update()
    {
        //TODO: only teleport when inside the maze
        if (Input.GetKeyDown(KeyCode.Escape) && insideMaze)
        {
            ReloadMaze();
            GameObject.Destroy(instanceOfPathIndicator);
            RedrawPath();
            insideMaze = false;
        }
    }


    //todo: choose a valide mazeexit
    private void InitializeMazeNavigator()
    {
        lastTriggered = mazeEntranceTrigger; // make so when ever we enter lastTriggered, we dont make a step in the maze.
        mazeLoader = GetComponent<LoadMaze>(); //get maze structure
        loadedMaze = mazeLoader.getMaze();
        mazeLoader.drawFloor(loadedMaze[t]); //draw the first floor
        
    }


    //setter for the entrance cell
    public GameObject setMazeEntranceTrigger
    {
        set { mazeEntranceTrigger = value; }
    }

    public GameObject getLastTriggered
    {
        get { return lastTriggered; }
    }

    public void makeStepInMaze(GameObject collidedTrigger)
    {
        if (collidedTrigger != lastTriggered)
        {
            lastTriggered = collidedTrigger; //set the new cell as last visited cell
            insideMaze = true;

            if (t < mazeLoader.getMazeHeight())
            {
                mazeLoader.destroyFloor(loadedMaze[t]); //destroy current floor
                t += 1;
                mazeLoader.drawFloor(loadedMaze[t]); //draw next floor

                if (t == mazeLoader.getMazeHeight() - 1) //win condition
                {
                    insideMaze = false;
                    GameObject.Destroy(mazeExitCell.topWall);
                    GameObject.Destroy(instanceOfPathIndicator);
                    Vector3 path = new Vector3(mazeExitCell.coordinates.x * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2, 0, (mazeExitCell.coordinates.z + 1) * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2);
                    Instantiate(pathIndicator, path, Quaternion.identity);
                    Destroy(instanceOfExitIndicator);
                }   
                else
                {
                    GameObject.Destroy(instanceOfPathIndicator);
                    RedrawPath();
                }
            }
            else
            {
                ReloadMaze();
                GameObject.Destroy(instanceOfPathIndicator);
                RedrawPath();
                insideMaze = false;
            }
        }
    }

    private void RedrawPath()
    {
        MazeCell nextCell = findNextCell();
        Vector3 path = new Vector3(nextCell.coordinates.x * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2, 0, nextCell.coordinates.z * mazeLoader.getCellSize() + mazeLoader.getCellSize() / 2);
        instanceOfPathIndicator = Instantiate(pathIndicator, path, Quaternion.identity) as GameObject;
    }

    private MazeCell findNextCell()
    {
        MazeCell nextCell = mazeExitCell;

        while (nextCell.coordinates.y != t + 1)
        {
            nextCell = mazeGraph.getCellInDirection(nextCell.predecessor, nextCell, -1);
        }

        return nextCell;
    }

    private void ReloadMaze()
    {
        mazeLoader.destroyFloor(loadedMaze[t]);
        t = 0; //set to first floor
        mazeLoader.drawFloor(loadedMaze[t]);

        lastTriggered = mazeEntranceTrigger; //set the entrance to be the beginning again

        playerController.GetComponent<CharacterController>().enabled = false; //a bypass for setting position of standard library char controller
        playerController.transform.position = mazeEntranceTrigger.transform.position; //set player's position to the beginning.
        playerController.GetComponent<CharacterController>().enabled = true;
    }
}
