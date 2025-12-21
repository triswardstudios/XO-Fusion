using UnityEngine;
using UnityEngine.UI;

public class swipe_menu : MonoBehaviour
{
    public Scrollbar scrollbar;
    public HorizontalLayoutGroup layoutGroup;

    [Header("Spacing")]
    public float normalSpacing = 40f;
    public float selectedExtraSpacing = 30f;

    [Header("Card Size")]
    public Vector2 selectedSize = new Vector2(260, 360);
    public Vector2 normalSize = new Vector2(220, 320);
    [Header("Card Color")]
    public Color selectedColor;
    public Color normalColor;
    public float alpha = 0.5f;

    private Image uiImage;

    float scroll_pos = 0;
    float[] pos;

    int selectedIndex = 0;
    RectTransform[] cards;

    void Awake()
    {
        cards = new RectTransform[transform.childCount];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        layoutGroup.spacing = normalSpacing;
        selectedColor = cards[0].GetComponent<Image>().color;
        Color tempColor = selectedColor;
        tempColor.a = alpha;
        normalColor = tempColor;
    }

    void Update()
    {
        // --- Position calculation ---
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
            pos[i] = distance * i;

        // --- Always find nearest index (IMPORTANT FIX) ---
        selectedIndex = GetNearestIndex();

        // --- Input ---
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.value;
        }
        else
        {
            scrollbar.value = Mathf.Lerp(
                scrollbar.value,
                pos[selectedIndex],
                Time.deltaTime * 10f
            );
        }

        // --- Size logic (NOW WORKS) ---
        for (int i = 0; i < cards.Length; i++)
        {
            Vector2 targetSize = (i == selectedIndex)
                ? selectedSize
                : normalSize;
            Color targetColor = (i == selectedIndex)
                ? selectedColor
                : normalColor;

            cards[i].sizeDelta = Vector2.Lerp(
                cards[i].sizeDelta,
                targetSize,
                Time.deltaTime * 8f
            );
            cards[i].GetComponent<Image>().color = Color.Lerp(
                cards[i].GetComponent<Image>().color,
                targetColor,
                Time.deltaTime * 8f
            );
        }

        // --- Spacing ---
        float targetSpacing = normalSpacing + selectedExtraSpacing;
        layoutGroup.spacing = Mathf.Lerp(
            layoutGroup.spacing,
            targetSpacing,
            Time.deltaTime * 5f
        );

    }

    // 🔹 Nearest card finder
    int GetNearestIndex()
    {
        float minDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < pos.Length; i++)
        {
            float d = Mathf.Abs(scrollbar.value - pos[i]);
            if (d < minDistance)
            {
                minDistance = d;
                index = i;
            }
        }
        return index;
    }
}
