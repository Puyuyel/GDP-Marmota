using UnityEngine;
using TMPro; // Asegúrate de usar esto si usas TextMeshPro

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    public int lootCount = 0;
    public TextMeshProUGUI lootText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateLootText();
    }

    public void AddLoot(int amount)
    {
        lootCount += amount;
        UpdateLootText();
    }

    void UpdateLootText()
    {
        lootText.text = $"Loot: {lootCount}";
    }
}
