using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public GameObject cellPrefab;
    public int row = 4;
    public int col = 4;
    private int size;

    void Start()
    {
        size = row * col;
        for (int i = 0; i < size ; i++)
        {
            GameObject cell = Instantiate(cellPrefab);
            cell.transform.SetParent(gridLayout.transform, false);
        }
    }
}
