using UnityEngine;

public class MazeCell
{
    public bool visited = false;
    public Vector2Int predecessor;
    public Vector3Int coordinates;

    public bool drawTopWall = true;
    public bool drawRightWall = true;

    public GameObject topWall, rightWall;

    public void drawCell(GameObject wallPrefab, int cellSize)
    {
        int i = coordinates.x; // horizontal
        int j = coordinates.z; // vertical

        if (drawTopWall)
        {
            topWall = GameObject.Instantiate(wallPrefab, new Vector3(i * cellSize + cellSize / 2, 0, j * cellSize + cellSize), Quaternion.identity) as GameObject;
            topWall.transform.Rotate(Vector3.up, 90f);
        }

        if (drawRightWall)
        {
            rightWall = GameObject.Instantiate(wallPrefab, new Vector3(i * cellSize + cellSize, 0, j * cellSize + cellSize / 2), Quaternion.identity) as GameObject;
        }
    }

    public void destroyCell()
    {
        if (topWall != null)
        {
            GameObject.Destroy(topWall);
        }

        if(rightWall != null)
        {
            GameObject.Destroy(rightWall);
        }
    }
}