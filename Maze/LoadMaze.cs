using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMaze : MonoBehaviour
{

    public GameObject wallPrefab; 
    public GameObject colliderPrefab;

    public float colliderSize = 6.0f;
    public Vector2Int mazeEntrance = new Vector2Int(1, 0); //in local coordinates of the maze
    private Vector2Int mazeExit;

    private int mazeSize = 8;
    private int cellSize = 10;
    private int mazeHeight = 16;
    private MazeCell[][,] mazeCells; // array of 2d arrays

    void Awake()
    {
        InitMaze();
    }

    //creates 3d structure of a maze
    private void InitMaze()
    {
        transform.position = new Vector3(0, 0, 0); //set maze position to the origine

        mazeCells = new MazeCell[mazeHeight][,];

        // i follows x axis
        // j follows z axis
        for (int t = 0; t < mazeHeight; t++)
        {
            mazeCells[t] = new MazeCell[mazeSize, mazeSize]; //initialize a floor

            //instantiate each cell on a floor.
            for (int i = 0; i < mazeSize; i++)
            {
                for (int j = 0; j < mazeSize; j++)
                {
                    mazeCells[t][i, j] = new MazeCell();
                    mazeCells[t][i, j].coordinates = new Vector3Int(i, t, j);
                }
            }

            PrimsMazeGenerator.run(mazeCells[t], mazeSize); //create the maze.

            for (int i = 0; i < mazeSize; i++)
            {
                for (int j = 0; j < mazeSize; j++)
                {
                    mazeCells[t][i, j].visited = false;
                }
            } //reset all visited to false, for BFS
        }



        
        InitializeCollider();
    }

    //Places collision boxes around the maze
    private void InitializeCollider()
    {
        colliderPrefab.transform.localScale = new Vector3(colliderSize, 1, colliderSize); //sets the size of future colliders.

        for(int n = 0; n < mazeSize * mazeSize; n++) { Instantiate(colliderPrefab, transform); } //for each cell adds a box collider

        IEnumerator childEnumerator = transform.GetEnumerator();

        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                childEnumerator.MoveNext();

                Vector3 positionInLocalCoords = transform.InverseTransformPoint(i * cellSize + cellSize / 2, 0, j * cellSize + cellSize / 2);

                Transform current = childEnumerator.Current as Transform;

                current.position = positionInLocalCoords;

                //sets up the entrance tile, so we don't make a step in maze when we enter it.
                if (i == mazeEntrance.x && j == mazeEntrance.y)
                {
                    GetComponent<MazeNavigator>().setMazeEntranceTrigger = current.gameObject;
                }
            }
        }
    }

    public void drawFloor(MazeCell[,] mazeCells)
    {
        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                mazeCells[i, j].drawCell(wallPrefab, cellSize);
            }
        }
    }

    public void destroyFloor(MazeCell[,] mazeCells)
    {
        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                mazeCells[i, j].destroyCell();
            }
        }
    }

    public MazeCell[][,] getMaze()
    {
        return mazeCells;
    }

    public int getMazeHeight()
    {
        return mazeHeight;
    }

    public int getMazeSize()
    {
        return mazeSize;
    }

    public void setMazeExit(Vector2Int exit)
    {
        mazeExit = exit;
    }

    public Vector2Int getMazeExit()
    {
        return mazeExit;
    }

    public int getCellSize() { return cellSize; }
}
