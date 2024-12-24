using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class Slider : MonoBehaviour
{

    [Header("Dimensions")]
    public int width;
    public int height;

    [Header("Values Settings")]
    public float maxValue = 100f;
    public float currentValue = 100f;

    [Header("UI Elements")]
    public Image background;
    public Image fill;
    public TextMeshProUGUI healthTextField;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        if (fill != null)
        {
            fill.fillAmount = currentValue / maxValue;
        }

        if (healthTextField != null)
        {
            healthTextField.text = $"{Mathf.FloorToInt(currentValue)} / {maxValue}";
        }

        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
