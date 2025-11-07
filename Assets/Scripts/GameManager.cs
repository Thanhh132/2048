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

        for (int outer = 0; outer < size; outer++)
        {
            List<Transform> line = new List<Transform>();

            for (int inner = 0; inner < size; inner++)
            {
                int index;
                if (horizontal)
                {
                    index = outer * size + inner;       // duyệt hàng
                }
                else
                {
                    index = inner * size + outer;       // duyệt cột
                }
                line.Add(cells[index].transform);
            }
            if (direction == Vector2Int.right || direction == Vector2Int.down)
                line.Reverse();

            MoveLine(line);
        }
    }

    private void MoveLine(List<Transform> line)
    {
        int emptyIndex = 0; 

        for (int i = 0; i < size; i++)
        {
            Transform cell = line[i];

            if (cell.childCount > 0)
            {
                if (i != emptyIndex)
                {
                    GameObject block = cell.GetChild(0).gameObject;
                    block.transform.SetParent(line[emptyIndex], false);
                }

                emptyIndex++;
            }
        }
    }


    void SpawnRandomBlock(int count)
    {
        List<GameObject> emptyCells = new List<GameObject>();

        foreach (GameObject cell in cells)
        {
            if (cell.transform.childCount == 0)
                emptyCells.Add(cell);
        }

        count = Mathf.Min(count, emptyCells.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            GameObject targetCell = emptyCells[randomIndex];
            Instantiate(blockPrefab, targetCell.transform, false);
            emptyCells.RemoveAt(randomIndex);
        }
    }
}
