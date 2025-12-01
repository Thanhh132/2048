using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameManager gameManager;

    // Vị trí chạm đầu và cuối
    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    
    // Khoảng cách tối thiểu để tính là 1 cú vuốt (tránh chạm nhầm)
    public bool detectSwipeOnlyAfterRelease = false; // Nên để false cho game 2048 để nhạy hơn
    public float minDistanceForSwipe = 20f; 

    void Update()
    {
        // --- Code Test bằng Chuột (Giả lập vuốt trên Editor) ---
        if (Input.GetMouseButtonDown(0))
        {
            fingerDownPos = Input.mousePosition;
            fingerUpPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            fingerUpPos = Input.mousePosition;
            CheckSwipe();
        }

        // 1. Xử lý cho Máy tính 
        if (Input.GetKeyDown(KeyCode.LeftArrow)) gameManager.Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) gameManager.Move(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) gameManager.Move(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) gameManager.Move(Vector2Int.down);

        // 2. Xử lý cho Điện thoại
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }

            // Có thể xử lý ngay khi ngón tay di chuyển  hoặc chờ nhấc tay 
            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPos = touch.position;
                CheckSwipe();
            }
        }
    }

    void CheckSwipe()
    {
        // Tính khoảng cách vuốt
        if (Vector2.Distance(fingerDownPos, fingerUpPos) > minDistanceForSwipe)
        {
            // Tính vector hướng
            Vector2 direction = fingerUpPos - fingerDownPos;
            
            // So sánh độ lớn của X và Y để biết vuốt Ngang hay Dọc
            // Mathf.Abs là lấy giá trị tuyệt đối (bỏ dấu âm)
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Vuốt theo phương NGANG (Horizontal)
                if (direction.x > 0)
                {
                    gameManager.Move(Vector2Int.right); // Phải
                }
                else
                {
                    gameManager.Move(Vector2Int.left); // Trái
                }
            }
            else
            {
                // Vuốt theo phương dọc
                if (direction.y > 0)
                {
                    gameManager.Move(Vector2Int.up); // Lên
                }
                else
                {
                    gameManager.Move(Vector2Int.down); // Xuống
                }
            }
        }
    }
}