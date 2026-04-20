using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Orchestration du gameplay "L'Herboriste du Grand Siècle".
/// 1) Le joueur explore la serre et collecte les bonnes plantes.
/// 2) Il valide les plantes demandées.
/// 3) Il les mélange dans le chaudron pour créer la soupe du Roi.
/// </summary>
public class HerbariumGameManager : MonoBehaviour
{
    [Serializable]
    public class RecipeStep
    {
        public string plantId;
        public int requiredCount = 1;
    }

    [Serializable]
    public class BookEntry
    {
        public string plantId;
        public string plantName;
        public string description;
        public Sprite image;
        public int requiredCount;
    }

    [Header("Recette principale")]
    [SerializeField] private SoupRecipeDefinition soupRecipe;

    [Header("Catalogue de plantes (pour retrouver image/description par id)")]
    [SerializeField] private List<PlantDefinition> plantCatalog = new();

    [Header("Fallback si aucune recette")]
    [SerializeField] private List<string> requestedPlants = new() { "menthe", "lavande", "romarin" };
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

    public bool ShelfObjectiveCompleted => requestedPlants.Count > 0 && requestedPlants.All(_validatedShelfPlants.Contains);
    public bool RecipeCompleted => recipe.Count > 0 && recipe.All(step => GetRecipeCount(step.plantId) >= Mathf.Max(1, step.requiredCount));

    public string RecipeName => soupRecipe != null ? soupRecipe.RecipeName : "Soupe Royale";
    public string RoyalRequest => soupRecipe != null ? soupRecipe.RoyalRequest : "Prépare la soupe du Roi avec les plantes demandées.";

    private void Start()
    {
        BuildRecipeFromDefinitionIfNeeded();
        RefreshUi();
        SetFeedback("Mission du Roi: explore la serre puis rassemble les bonnes plantes.");
    }

    private void BuildRecipeFromDefinitionIfNeeded()
    {
        if (soupRecipe == null || soupRecipe.Ingredients.Count == 0)
        {
            if (recipe.Count == 0)
            {
                recipe = requestedPlants.Select(id => new RecipeStep { plantId = id, requiredCount = 1 }).ToList();
            }

            return;
        }

        recipe = soupRecipe.Ingredients
            .Where(ingredient => ingredient.plant != null && !string.IsNullOrWhiteSpace(ingredient.plant.PlantId))
            .Select(ingredient => new RecipeStep
            {
                plantId = ingredient.plant.PlantId,
                requiredCount = Mathf.Max(1, ingredient.requiredCount)
            })
            .ToList();

        requestedPlants = recipe.Select(step => step.plantId).Distinct().ToList();

        foreach (var ingredient in soupRecipe.Ingredients)
        {
            if (ingredient.plant != null && !plantCatalog.Contains(ingredient.plant))
            {
                plantCatalog.Add(ingredient.plant);
            }
        }
    }

    public List<BookEntry> GetBookEntries()
    {
        return recipe.Select(step =>
        {
            var def = FindPlantDefinition(step.plantId);
            return new BookEntry
            {
                plantId = step.plantId,
                plantName = def != null ? def.DisplayName : step.plantId,
                description = def != null ? def.Description : "Description non renseignée dans le grimoire.",
                image = def != null ? def.Illustration : null,
                requiredCount = Mathf.Max(1, step.requiredCount)
            };
        }).ToList();
    }

    public bool IsPlantValidated(string plantId) => _validatedShelfPlants.Contains(plantId);

    public int GetRecipeCount(string plantId)
    {
        return _recipeProgress.TryGetValue(plantId, out var count) ? count : 0;
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
        SetFeedback($"{plant.DisplayName} ajoutée ({_validatedShelfPlants.Count}/{requestedPlants.Count}) sur l'étagère.");

        if (ShelfObjectiveCompleted)
        {
            onShelfObjectiveComplete?.Invoke();
            SetFeedback("Étape 1 terminée : toutes les plantes demandées sont rassemblées. Passe au chaudron.");
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
            SetFeedback("Commence par valider les plantes sur l'étagère.");
            return false;
        }

        var step = recipe.FirstOrDefault(item => item.plantId == plant.PlantId);
        if (step == null)
        {
            SetFeedback($"{plant.DisplayName} n'appartient pas à la recette de la soupe.");
            return false;
        }

        var target = Mathf.Max(1, step.requiredCount);
        var current = GetRecipeCount(plant.PlantId);
        if (current >= target)
        {
            SetFeedback($"{plant.DisplayName} est déjà en quantité suffisante dans le chaudron.");
            return false;
        }

        _recipeProgress[plant.PlantId] = current + 1;
        SetFeedback($"{plant.DisplayName} ajoutée au chaudron ({current + 1}/{target}).");

        if (RecipeCompleted)
        {
            onRecipeComplete?.Invoke();
            SetFeedback("Soupe royale terminée ! Le Roi peut être impressionné.");
        }

        RefreshUi();
        return true;
    }

    private PlantDefinition FindPlantDefinition(string plantId)
    {
        return plantCatalog.FirstOrDefault(item => item != null && item.PlantId == plantId);
    }

    private void RefreshUi()
    {
        if (questText != null)
        {
            var questLines = GetBookEntries().Select(entry =>
            {
                var state = IsPlantValidated(entry.plantId) ? "✓" : "✗";
                return $"- {entry.plantName} {state}";
            });

            questText.text = $"Demande royale : {RoyalRequest}\n\nPlantes à trouver:\n" + string.Join("\n", questLines);
        }

        if (recipeText != null)
        {
            var recipeLines = GetBookEntries().Select(entry =>
            {
                var current = GetRecipeCount(entry.plantId);
                return $"- {entry.plantName}: {current}/{entry.requiredCount}";
            });

            recipeText.text = $"Recette - {RecipeName}:\n" + string.Join("\n", recipeLines);
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
