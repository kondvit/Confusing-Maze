using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsMazeGenerator 
{
    /*************************************
     * 1) pick a cell from frontier list
     * 2) need to remove a wall
     * 3) add new frontier cells
     * 
     *************************************/


    //TODO: block the entrance when entering and block the open the exit when exiting
    public static void run(MazeCell[,] mazeCells, int mazeSize)
    {
        //randomCell is at first initialized to the entrance of the maze
        List<MazeCell> frontier = new List<MazeCell>();

        MazeCell randomCell = mazeCells[Random.Range(0, mazeSize), Random.Range(0, mazeSize)];
        bool firstIteration = true;

        do
        {
            //if we can we add frontier to the left to the right and to the top
            //problem if there is exception everything is busted
            if (!firstIteration)
            {
                int randomFrontierIndex = Random.Range(0, frontier.Count);
                randomCell = frontier[randomFrontierIndex];
            }

            randomCell.visited = true;

            List<MazeCell> adjacentVisited = new List<MazeCell>(); //stores all adjecent cells that have been visited

            findAdjacentCells(mazeCells, mazeSize, frontier, randomCell, adjacentVisited);

            pickWallToDestroy(randomCell, adjacentVisited); //randomly chooses what wall gets destroyed

            frontier.Remove(randomCell);
            firstIteration = false;

        } while (frontier.Count != 0);

    }

    private static void pickWallToDestroy(MazeCell randomCell, List<MazeCell> adjacentVisited)
    {
        if (adjacentVisited.Count != 0)
        {
            int randomIndex = Random.Range(0, adjacentVisited.Count);
            MazeCell randomAdjacent = adjacentVisited[randomIndex];

            if (randomAdjacent.coordinates.x > randomCell.coordinates.x) //on the right, we remove the wall of the randomCell
            {
                randomCell.drawRightWall = false;
            }
            else if (randomAdjacent.coordinates.x < randomCell.coordinates.x) //on the left, we remove the right wall of adjacent cell
            {
                randomAdjacent.drawRightWall = false;
            }

            if (randomAdjacent.coordinates.z > randomCell.coordinates.z) //on top, we remove the top wall of randomCell
            {
                randomCell.drawTopWall = false;
            }
            else if (randomAdjacent.coordinates.z < randomCell.coordinates.z)  //on the bottom, we remove the right wall of adjacent cell
            {
                randomAdjacent.drawTopWall = false;
            }
        }
    }

    private static void findAdjacentCells(MazeCell[,] mazeCells, int mazeSize, List<MazeCell> frontier, MazeCell randomCell, List<MazeCell> adjacentVisited)
    {
        if (randomCell.coordinates.x > 0)
        {
            MazeCell leftCell = mazeCells[randomCell.coordinates.x - 1, randomCell.coordinates.z];
            if (leftCell.visited) { adjacentVisited.Add(leftCell); }
            else if (!frontier.Contains(leftCell)) { frontier.Add(leftCell); }
        }

        if (randomCell.coordinates.z < mazeSize - 1)
        {
            MazeCell topCell = mazeCells[randomCell.coordinates.x, randomCell.coordinates.z + 1];
            if (topCell.visited) { adjacentVisited.Add(topCell); }
            else if (!frontier.Contains(topCell)) { frontier.Add(topCell); }
        }

        if (randomCell.coordinates.x < mazeSize - 1)
        {
            MazeCell rightCell = mazeCells[randomCell.coordinates.x + 1, randomCell.coordinates.z];
            if (rightCell.visited) { adjacentVisited.Add(rightCell); }
            else if (!frontier.Contains(rightCell)) { frontier.Add(rightCell); }
        }

        if (randomCell.coordinates.z > 0)
        {
            MazeCell bottomCell = mazeCells[randomCell.coordinates.x, randomCell.coordinates.z - 1];
            if (bottomCell.visited) { adjacentVisited.Add(bottomCell); }
            else if (!frontier.Contains(bottomCell)) { frontier.Add(bottomCell); }
        }
    }
}
