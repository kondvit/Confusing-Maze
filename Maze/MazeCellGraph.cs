using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGraph
{
    private MazeCell[][,] maze;

    public void BFS(MazeCell[][,] maze, MazeCell startCell)
    {
        this.maze = maze;
        MazeCell currentCell = startCell;
        
        Queue<MazeCell> queue = new Queue<MazeCell>();
        queue.Enqueue(currentCell); //queue up the first cell
        //TODO: run through all cells and set visited to false
        currentCell.visited = true; //mark the first cell as visited

        while (queue.Count != 0)
        {
            currentCell = queue.Dequeue(); // get next mazecell

            List<MazeCell> currentAdjacent = new List<MazeCell>(); //create a list of all adjacencies of current cell

            getAdjacentCells(currentCell, currentAdjacent); //populate the adjacencie list

            foreach(MazeCell cell in currentAdjacent)
            {
                if (!cell.visited)
                {
                    cell.visited = true;
                    queue.Enqueue(cell);
                }
            }
        }
    }

    //TODO: check for walls
    private void getAdjacentCells(MazeCell currentCell, List<MazeCell> currentAdjacent)
    {
        MazeCell left = getCellInDirection(Vector2Int.left, currentCell, 0);
        if (left != null && !left.drawRightWall)
        {
            //left.visited = true;
            left = getCellInDirection(Vector2Int.left, currentCell, 1);
            if (left != null)
            {
                left.predecessor = Vector2Int.right; // points in the oposite direction
                currentAdjacent.Add(left);
            }
        }

        MazeCell up = getCellInDirection(Vector2Int.up, currentCell, 0);
        if (up != null && !currentCell.drawTopWall)
        {
            //up.visited = true;
            up = getCellInDirection(Vector2Int.up, currentCell, 1);
            if (up != null) { 
                up.predecessor = Vector2Int.down; // points in the oposite direction
                currentAdjacent.Add(up);
            }
        }

        MazeCell right = getCellInDirection(Vector2Int.right, currentCell, 0);
        if (right != null && !currentCell.drawRightWall)
        {
            //right.visited = true;
            right = getCellInDirection(Vector2Int.right, currentCell, 1);
            if (right != null)
            {
                right.predecessor = Vector2Int.left; // points in the oposite direction
                currentAdjacent.Add(right);
            }
        }

        MazeCell down = getCellInDirection(Vector2Int.down, currentCell, 0);
        if (down != null && !down.drawTopWall)
        {
            //down.visited = true;
            down = getCellInDirection(Vector2Int.down, currentCell, 1);
            if (down != null)
            {
                down.predecessor = Vector2Int.up; // points in the oposite direction
                currentAdjacent.Add(down);
            }
        }
    }

    // int field -1 for going down the maze, + 1 to go up the maze
    public MazeCell getCellInDirection(Vector2Int direction, MazeCell currentCell, int yDir)
    {
        try // if cell exists return, else return null
        {
            return maze[currentCell.coordinates.y + yDir][currentCell.coordinates.x + direction.x, currentCell.coordinates.z + direction.y];
        }
        catch
        {
            return null;
        }
    }

}
