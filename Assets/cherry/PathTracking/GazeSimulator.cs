using UnityEngine;

public class GazeSimulator : MonoBehaviour
{
    public Vector2 GetGazeScreenPosition()
    {
        return Input.mousePosition;
    }
}