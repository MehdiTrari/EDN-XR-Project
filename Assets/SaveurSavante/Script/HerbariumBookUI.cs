using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface du grimoire: affiche image + description des plantes nécessaires.
/// </summary>
public class HerbariumBookUI : MonoBehaviour
{
    [SerializeField] private HerbariumGameManager gameManager;
    [SerializeField] private TMP_Text recipeTitleText;
    [SerializeField] private TMP_Text pageIndicatorText;
    [SerializeField] private TMP_Text plantNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image plantImage;

    private readonly List<HerbariumGameManager.BookEntry> _pages = new();
    private int _currentIndex;

    private void Start()
    {
        RefreshPages();
        ShowPage(0);
    }

    public void RefreshPages()
    {
        _pages.Clear();

        if (gameManager == null)
        {
            return;
        }

        _pages.AddRange(gameManager.GetBookEntries());
    }

    public void NextPage() => ShowPage(_currentIndex + 1);

    public void PreviousPage() => ShowPage(_currentIndex - 1);

    public void ShowPage(int index)
    {
        if (_pages.Count == 0)
        {
            WriteEmptyBook();
            return;
        }

        _currentIndex = Mathf.Clamp(index, 0, _pages.Count - 1);
        var page = _pages[_currentIndex];

        if (recipeTitleText != null)
        {
            recipeTitleText.text = gameManager != null
                ? $"Grimoire royal - {gameManager.RecipeName}"
                : "Grimoire royal";
        }

        if (pageIndicatorText != null)
        {
            pageIndicatorText.text = $"Page {_currentIndex + 1}/{_pages.Count}";
        }

        if (plantNameText != null)
        {
            var validated = gameManager != null && gameManager.IsPlantValidated(page.plantId) ? "✓" : "✗";
            plantNameText.text = $"{page.plantName} [{validated}]";
        }

        if (descriptionText != null)
        {
            descriptionText.text = page.description;
        }

        if (quantityText != null)
        {
            var current = gameManager != null ? gameManager.GetRecipeCount(page.plantId) : 0;
            quantityText.text = $"Quantité: {current}/{page.requiredCount}";
        }

        if (plantImage != null)
        {
            plantImage.enabled = page.image != null;
            plantImage.sprite = page.image;
        }
    }

    private void WriteEmptyBook()
    {
        if (recipeTitleText != null) recipeTitleText.text = "Grimoire royal";
        if (pageIndicatorText != null) pageIndicatorText.text = "Page 0/0";
        if (plantNameText != null) plantNameText.text = "Aucune plante configurée";
        if (descriptionText != null) descriptionText.text = "Ajoute une recette au HerbariumGameManager.";
        if (quantityText != null) quantityText.text = string.Empty;

        if (plantImage != null)
        {
            plantImage.sprite = null;
            plantImage.enabled = false;
        }
    }
}
