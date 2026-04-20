using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoupRecipeDefinition", menuName = "SaveurSavante/Soup Recipe")]
public class SoupRecipeDefinition : ScriptableObject
{
    [Serializable]
    public class IngredientRequirement
    {
        public PlantDefinition plant;
        [Min(1)] public int requiredCount = 1;
    }

    [SerializeField] private string recipeName = "Soupe Royale";

    [TextArea(2, 5)]
    [SerializeField] private string royalRequest = "Le Roi exige une soupe tonifiante pour impressionner la cour.";

    [SerializeField] private List<IngredientRequirement> ingredients = new();

    public string RecipeName => recipeName;
    public string RoyalRequest => royalRequest;
    public IReadOnlyList<IngredientRequirement> Ingredients => ingredients;
}
