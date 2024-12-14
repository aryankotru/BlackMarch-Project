using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Grid/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public bool[,] _gridData = new bool[10, 10];
}
