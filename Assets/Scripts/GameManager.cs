using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public BoardManager boardManager;
    public GameObject blockPrefab;

    public SaveSystem saveSystem;    
    public AudioManager audioManager; 

    
    public TextMeshProUGUI scoreText;
    public GameObject restartConfirmationPanel;

    private List<GameObject> cells = new List<GameObject>();
    private const int size = 4;
    private int score = 0;

   
    private bool hasMergedInThisTurn = false;

    void Start()
    {
        if (restartConfirmationPanel != null) restartConfirmationPanel.SetActive(false);
        CheckAllCells();

        if (saveSystem.HasSaveData()) LoadSavedGame();
        else StartNewGame();
    }

    public void Move(Vector2Int direction)
    {
        bool horizontal = Mathf.Abs(direction.x) > 0;
        bool moveMade = false;

        hasMergedInThisTurn = false;

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
            saveSystem.SaveGame(cells, score);

            if (audioManager != null)
            {

                audioManager.PlaySwipe();

                if (hasMergedInThisTurn)
                {
                    audioManager.PlayMerge();
                }
            }
        }
    }

    private bool MoveLine(List<Transform> line)
    {
        bool hasChanged = false;
        int emptyIndex = 0;

        foreach (var cell in line)
            if (cell.childCount > 0) cell.GetChild(0).GetComponent<Blocks>().hasMerged = false;

        for (int i = 0; i < size; i++)
        {
            Transform cell = line[i];

            if (cell.childCount > 0)
            {
                GameObject currentObj = cell.GetChild(0).gameObject;
                Blocks currentBlock = currentObj.GetComponent<Blocks>();

                if (emptyIndex > 0)
                {
                    Transform prevCell = line[emptyIndex - 1];
                    if (prevCell.childCount > 0)
                    {
                        Blocks prevBlock = prevCell.GetChild(0).GetComponent<Blocks>();

                        if (currentBlock.value == prevBlock.value && !prevBlock.hasMerged)
                        {
                            int newValue = prevBlock.value * 2;
                            score += newValue;
                            UpdateScoreText();

                            prevBlock.SetValue(newValue);
                            prevBlock.hasMerged = true;
                            prevBlock.PlayMergeAnim();

                            Destroy(currentObj);
                            hasChanged = true;

                            hasMergedInThisTurn = true;

                            continue;
                        }
                    }
                }

                if (emptyIndex != i)
                {
                    currentObj.transform.SetParent(line[emptyIndex], true);
                    currentObj.transform.localPosition = Vector3.zero;
                    currentObj.transform.localScale = Vector3.one;
                    hasChanged = true;
                }
                emptyIndex++;
            }
        }
        return hasChanged;
    }


    void LoadSavedGame()
    {
        score = saveSystem.LoadScore();
        UpdateScoreText();
        int[] boardData = saveSystem.LoadBoardData();
        foreach (GameObject cell in cells) if (cell.transform.childCount > 0) Destroy(cell.transform.GetChild(0).gameObject);
        for (int i = 0; i < cells.Count && i < boardData.Length; i++)
        {
            int val = boardData[i];
            if (val > 0)
            {
                GameObject newBlock = Instantiate(blockPrefab, cells[i].transform, false);
                newBlock.GetComponent<Blocks>().SetValue(val);
            }
        }
    }

    void StartNewGame()
    {
        saveSystem.DeleteSave();
        score = 0;
        UpdateScoreText();
        foreach (GameObject cell in cells) if (cell.transform.childCount > 0) foreach (Transform child in cell.transform) Destroy(child.gameObject);
        SpawnRandomBlock(2);
        saveSystem.SaveGame(cells, score);
    }

    void UpdateScoreText() { if (scoreText != null) scoreText.text = score.ToString(); }
    void SpawnRandomBlock(int count)
    {
        List<GameObject> emptyCells = new List<GameObject>();
        foreach (GameObject cell in cells) if (cell.transform.childCount == 0) emptyCells.Add(cell);
        count = Mathf.Min(count, emptyCells.Count);
        for (int i = 0; i < count; i++)
        {
            int r = Random.Range(0, emptyCells.Count);
            GameObject b = Instantiate(blockPrefab, emptyCells[r].transform, false);
            b.GetComponent<Blocks>().PlaySpawnAnim();
            emptyCells.RemoveAt(r);
        }
    }
    private void CheckAllCells()
    {
        cells.Clear();
        foreach (Transform cell in boardManager.gridLayout.transform) cells.Add(cell.gameObject);
    }
    public void OnRestartBtnClick() 
    { 
        if (restartConfirmationPanel != null) restartConfirmationPanel.SetActive(true); 
    }
    public void ConfirmRestart() 
    { 
        if (restartConfirmationPanel != null) restartConfirmationPanel.SetActive(false); 
        StartNewGame(); 
    }
    public void CancelRestart() 
    { 
        if (restartConfirmationPanel != null) restartConfirmationPanel.SetActive(false); 
    }
}