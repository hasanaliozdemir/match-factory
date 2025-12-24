using TMPro;
using UnityEngine;

public enum EPowerUpType
{
    Vacuum = 0,
    Spring = 1,
    Fan = 2,
    FreezeGun = 3
}


public abstract class PowerUp : MonoBehaviour
{
    [Header("PowerUp Settings")]
    [SerializeField] private EPowerUpType powerUpType;
    public EPowerUpType PowerUpType => powerUpType;

    [Header("Elements")]
    [SerializeField] private TextMeshPro amountText;
    [SerializeField] private GameObject videoIcon;

    public void UpdateVisuals(int amount)
    {
        videoIcon.SetActive(amount <= 0);
        amountText.gameObject.SetActive(amount > 0);

        amountText.text = amount.ToString();
    }

}
