using UnityEngine;

[System.Serializable]
public struct ItemLevelData
{
    public Item itemPrefab;

    [NaughtyAttributes.ValidateInput("ValidateAmount", "Amount must be greater than 0 and multiple of 3")]
    [NaughtyAttributes.AllowNesting]
    [Range(0, 100)]
    public int amount;

    private bool ValidateAmount(int amount)
    {
        return amount > 0 && amount % 3 == 0;
    }
}
