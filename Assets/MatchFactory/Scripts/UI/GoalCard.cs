using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject checkmark;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject backFace;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        backFace.SetActive(Vector3.Dot(Vector3.forward, transform.forward) < 0);
    }

    public void Configure(int initialAmount, Sprite icon)
    {
        amountText.text = initialAmount.ToString();
        iconImage.sprite = icon;

    }

    public void UpdateAmount(int newAmount)
    {
        amountText.text = newAmount.ToString();

        BumpAnimation();
    }

    private void BumpAnimation()
    {
        // Cancel any ongoing animations
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.25f).setLoopPingPong(1);
    }

    public void Complete()
    {
        // gameObject.SetActive(false);

        animator.enabled = true;

        checkmark.SetActive(true);
        amountText.gameObject.SetActive(false);

        animator.Play("Complete");
    }
}