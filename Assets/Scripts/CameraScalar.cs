using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset; // Store the value on how far back the camera should be (z axis)
    public float aspectRatio = 0.5625f; // 9:16
    public float padding = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindFirstObjectByType<Board>();
        if (board != null )
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x/2, y/2, cameraOffset);
        transform.position = tempPosition;
        if (board.width >= board.height) {
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        } else {
            Camera.main.orthographicSize = board.height / 2 + padding;
        }

    }
}
