using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    private const string KEY_BOARD = "Save_Board";
    private const string KEY_SCORE = "Save_Score";

    public void SaveGame(List<GameObject> cells, int score)
    {
        string boardData = "";
        
        foreach (GameObject cell in cells)
        {
            int value = 0;
            if (cell.transform.childCount > 0)
            {
                Blocks block = cell.transform.GetChild(0).GetComponent<Blocks>();
                if (block != null) value = block.value;
            }
            boardData += value + ",";
        }

        PlayerPrefs.SetString(KEY_BOARD, boardData);
        PlayerPrefs.SetInt(KEY_SCORE, score);
        PlayerPrefs.Save();
    }

    public bool HasSaveData()
    {
        return PlayerPrefs.HasKey(KEY_BOARD);
    }

    public int LoadScore()
    {
        return PlayerPrefs.GetInt(KEY_SCORE, 0);
    }

    public int[] LoadBoardData()
    {
        string boardData = PlayerPrefs.GetString(KEY_BOARD);
        string[] stringData = boardData.Split(',');
        
        int[] intData = new int[stringData.Length];
        for (int i = 0; i < stringData.Length; i++)
        {
            if (!string.IsNullOrEmpty(stringData[i]))
            {
                int.TryParse(stringData[i], out intData[i]);
            }
        }
        return intData;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(KEY_BOARD);
        PlayerPrefs.DeleteKey(KEY_SCORE);
    }
}