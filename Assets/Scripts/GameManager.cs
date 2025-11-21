using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public GameObject blockPrefab;

    private List<GameObject> cells = new List<GameObject>();
    private const int size = 4;

    void Start()
    {
        CheckAllCells();
        SpawnRandomBlock(2);
    }

    private void CheckAllCells()
    {
        foreach (Transform cell in boardManager.gridLayout.transform)
            cells.Add(cell.gameObject);
    }

    public void Move(Vector2Int direction)
    {
        bool horizontal = Mathf.Abs(direction.x) > 0;
        bool moveMade = false;

        for (int outer = 0; outer < size; outer++)
        {
            List<Transform> line = new List<Transform>();

            for (int inner = 0; inner < size; inner++)
            {
                int index = horizontal ? (outer * size + inner) : (inner * size + outer);
                line.Add(cells[index].transform);
            }

            if (direction == Vector2Int.right || direction == Vector2Int.down)
                line.Reverse();

            if (MoveLine(line))
            {
                moveMade = true;
            }
        }

        if (moveMade)
        {
            SpawnRandomBlock(1);
        }
    }

    private bool MoveLine(List<Transform> line)
    {
        bool hasChanged = false;
        int emptyIndex = 0;

        // Reset trạng thái gộp
        foreach (var cell in line)
        {
            if (cell.childCount > 0)
                cell.GetChild(0).GetComponent<Blocks>().hasMerged = false;
        }

        for (int i = 0; i < size; i++)
        {
            Transform cell = line[i];

            if (cell.childCount > 0)
            {
                GameObject currentObj = cell.GetChild(0).gameObject;
                Blocks currentBlock = currentObj.GetComponent<Blocks>();

                // --- LOGIC GỘP ---
                if (emptyIndex > 0)
                {
                    Transform prevCell = line[emptyIndex - 1];
                    if (prevCell.childCount > 0)
                    {
                        Blocks prevBlock = prevCell.GetChild(0).GetComponent<Blocks>();

                        if (currentBlock.value == prevBlock.value && !prevBlock.hasMerged)
                        {
                            prevBlock.SetValue(prevBlock.value * 2);
                            prevBlock.hasMerged = true;
                            prevBlock.PlayMergeAnim();
                            
                            Destroy(currentObj); 
                            hasChanged = true;
                            continue;
                        }
                    }
                }

                if (emptyIndex != i)
                {
                    // Giữ World Position để tạo hiệu ứng trượt
                    currentObj.transform.SetParent(line[emptyIndex], true);
                    
                    // --- SỬA LỖI CHUI XUỐNG DƯỚI TẠI ĐÂY ---
                    // Lấy vị trí hiện tại
                    Vector3 pos = currentObj.transform.localPosition;
                    // Ép Z về 0 ngay lập tức (để không bị vẽ sau tấm nền)
                    pos.z = 0; 
                    currentObj.transform.localPosition = pos;
                    
                    // Đảm bảo scale không bị méo
                    currentObj.transform.localScale = Vector3.one;

                    hasChanged = true;
                }

                emptyIndex++;
            }
        }
        return hasChanged;
    }

    void SpawnRandomBlock(int count)
    {
        List<GameObject> emptyCells = new List<GameObject>();
        foreach (GameObject cell in cells)
        {
            if (cell.transform.childCount == 0) emptyCells.Add(cell);
        }

        count = Mathf.Min(count, emptyCells.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            GameObject newBlock = Instantiate(blockPrefab, emptyCells[randomIndex].transform, false);
            
            // Gọi anim spawn
            newBlock.GetComponent<Blocks>().PlaySpawnAnim();
            
            emptyCells.RemoveAt(randomIndex);
        }
    }
}