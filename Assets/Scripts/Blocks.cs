using UnityEngine;
using TMPro;
using System.Collections;

public class Blocks : MonoBehaviour
{
    public int value = 2;
    [SerializeField] private TextMeshProUGUI valueText;
    [HideInInspector] public bool hasMerged = false;
    private float slideSpeed = 8000f; 

    private void Start()
    {
        // Đảm bảo khi sinh ra scale là 1 ngay lập tức (trừ khi chạy anim spawn)
        // Để tránh việc bị tàng hình
        if (transform.localScale == Vector3.zero) 
            transform.localScale = Vector3.one;
            
        UpdateVisual();
    }

    private void Update()
    {
        // Nếu vị trí chưa về (0,0,0)
        if (transform.localPosition != Vector3.zero)
        {
            // 1. Di chuyển cực nhanh về tâm
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, slideSpeed * Time.deltaTime);
            
            // 2.Sửa lỗi chui xuống dưới bàn cờ
            // Ép tọa độ Z luôn bằng 0 để nó nổi trên mặt tiền
            Vector3 fixedPos = transform.localPosition;
            fixedPos.z = 0;
            transform.localPosition = fixedPos;
        }
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (valueText != null) valueText.text = value.ToString();
    }

    public void PlaySpawnAnim()
    {
        StartCoroutine(AnimateScale(Vector3.zero, Vector3.one, 0.2f));
    }

    public void PlayMergeAnim()
    {
        StartCoroutine(AnimatePulse());
    }

    // Helper Animation
    IEnumerator AnimateScale(Vector3 start, Vector3 end, float time)
    {
        float t = 0;
        transform.localScale = start;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            transform.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
        transform.localScale = end;
    }

    IEnumerator AnimatePulse()
    {
        yield return AnimateScale(Vector3.one, Vector3.one * 1.2f, 0.1f);
        yield return AnimateScale(Vector3.one * 1.2f, Vector3.one, 0.1f);
    }
}