using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI canText;

    // Can değerlerini güncelleyen fonksiyon
    public void SetHealth(float currentHp, float maxHp)
    {
        if (canText == null)
        {
            Debug.LogWarning("UiManager: canText atanmadı!");
            return;
        }

        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        canText.text = $"{currentHp}/{maxHp}";
    }
}
