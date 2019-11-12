using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTriggerNotifier : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<MazeNavigator>().makeStepInMaze(gameObject);
        }
    }
}
