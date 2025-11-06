using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public GameObject blockPrefab;
    
    private List<GameObject> emptyCells = new List<GameObject>();

    private List<GameObject> cells = new List<GameObject>();

    void Start()
    {
        foreach (Transform cell in boardManager.gridLayout.transform)
        {
            cells.Add(cell.gameObject);
        }

        SpawnRandomBlock(2);
    }

    void SpawnRandomBlock(int count)
    {
       
        foreach (GameObject cell in cells)
        {
            if (cell.transform.childCount == 0)
                emptyCells.Add(cell);
        }

        count = Mathf.Min(count, emptyCells.Count);
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            GameObject randomCell = emptyCells[randomIndex];
            Instantiate(blockPrefab, randomCell.transform, false);
            emptyCells.RemoveAt(randomIndex);
        }
    }

}
