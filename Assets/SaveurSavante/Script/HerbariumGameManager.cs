using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Orchestration du gameplay "L'Herboriste du Grand Siècle".
/// 1) Le joueur dépose les bonnes plantes sur l'étagère.
/// 2) Puis il les jette dans le chaudron selon la recette.
/// </summary>
public class HerbariumGameManager : MonoBehaviour
{
    [Serializable]
    public class RecipeStep
    {
        public string plantId;
        public int requiredCount = 1;
    }

    [Header("Objectif 1 - Étagère")]
    [Tooltip("Plantes demandées par le roi.")]
    [SerializeField] private List<string> requestedPlants = new() { "menthe", "lavande", "romarin" };

    [Header("Objectif 2 - Recette")]
    [SerializeField] private List<RecipeStep> recipe = new();

    [Header("UI (facultatif)")]
    [SerializeField] private TMP_Text questText;
    [SerializeField] private TMP_Text recipeText;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Événements")]
    [SerializeField] private UnityEvent onShelfObjectiveComplete;
    [SerializeField] private UnityEvent onRecipeComplete;

    private readonly HashSet<string> _validatedShelfPlants = new();
    private readonly Dictionary<string, int> _recipeProgress = new();

    public bool ShelfObjectiveCompleted => requestedPlants.All(_validatedShelfPlants.Contains);
    public bool RecipeCompleted => recipe.Count > 0 && recipe.All(step => GetRecipeCount(step.plantId) >= Mathf.Max(1, step.requiredCount));

    private void Start()
    {
        if (recipe.Count == 0)
        {
            recipe = requestedPlants.Select(id => new RecipeStep { plantId = id, requiredCount = 1 }).ToList();
        }

        RefreshUi();
        SetFeedback("Mission du Roi: rassemble les bonnes plantes sur l'étagère.");
    }

    public bool TryRegisterShelfPlant(PlantItem plant)
    {
        if (plant == null)
        {
            return false;
        }

        if (!requestedPlants.Contains(plant.PlantId))
        {
            SetFeedback($"{plant.DisplayName} n'est pas demandée dans le grimoire.");
            return false;
        }

        if (_validatedShelfPlants.Contains(plant.PlantId))
        {
            SetFeedback($"{plant.DisplayName} est déjà validée sur l'étagère.");
            return false;
        }

        _validatedShelfPlants.Add(plant.PlantId);
        plant.MarkCollected();
        SetFeedback($"{plant.DisplayName} ajoutée à l'étagère ({_validatedShelfPlants.Count}/{requestedPlants.Count}).");

        if (ShelfObjectiveCompleted)
        {
            onShelfObjectiveComplete?.Invoke();
            SetFeedback("Parfait ! Les plantes demandées sont réunies. Passe à la recette.");
        }

        RefreshUi();
        return true;
    }

    public bool TryRegisterCauldronIngredient(PlantItem plant)
    {
        if (plant == null)
        {
            return false;
        }

        if (!ShelfObjectiveCompleted)
        {
            SetFeedback("Commence par remplir l'étagère avec les plantes demandées.");
            return false;
        }

        if (!recipe.Any(step => step.plantId == plant.PlantId))
        {
            SetFeedback($"{plant.DisplayName} n'appartient pas à la recette.");
            return false;
        }

        _recipeProgress.TryGetValue(plant.PlantId, out var count);
        _recipeProgress[plant.PlantId] = count + 1;
        SetFeedback($"Ingrédient ajouté au chaudron : {plant.DisplayName}.");

        if (RecipeCompleted)
        {
            onRecipeComplete?.Invoke();
            SetFeedback("Potion terminée ! Tu peux la présenter au Roi.");
        }

        RefreshUi();
        return true;
    }

    private int GetRecipeCount(string plantId)
    {
        return _recipeProgress.TryGetValue(plantId, out var count) ? count : 0;
    }

    private void RefreshUi()
    {
        if (questText != null)
        {
            var lines = requestedPlants
                .Select(id => $"- {id} {( _validatedShelfPlants.Contains(id) ? "✓" : "✗")}");
            questText.text = "Plantes demandées:\n" + string.Join("\n", lines);
        }

        if (recipeText != null)
        {
            var lines = recipe.Select(step =>
            {
                var target = Mathf.Max(1, step.requiredCount);
                var current = GetRecipeCount(step.plantId);
                return $"- {step.plantId}: {current}/{target}";
            });

            recipeText.text = "Recette:\n" + string.Join("\n", lines);
        }
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }

        Debug.Log($"[HerbariumGameManager] {message}");
    }
}
