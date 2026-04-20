using UnityEngine;

[CreateAssetMenu(fileName = "PlantDefinition", menuName = "SaveurSavante/Plant Definition")]
public class PlantDefinition : ScriptableObject
{
    [Header("Identité")]
    [SerializeField] private string plantId = "menthe";
    [SerializeField] private string displayName = "Menthe";

    [Header("Lore grimoire")]
    [TextArea(2, 6)]
    [SerializeField] private string description = "Feuilles aromatiques idéales pour les décoctions royales.";
    [SerializeField] private Sprite illustration;

    public string PlantId => plantId;
    public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? plantId : displayName;
    public string Description => description;
    public Sprite Illustration => illustration;
}
