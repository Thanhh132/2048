using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameManager gameManager; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameManager.Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameManager.Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameManager.Move(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameManager.Move(Vector2Int.down);
        }
    }
}
