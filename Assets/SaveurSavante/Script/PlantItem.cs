using UnityEngine;

/// <summary>
/// Données d'une plante manipulable dans la serre.
/// </summary>
public class PlantItem : MonoBehaviour
{
    [Header("Source de données (optionnel)")]
    [SerializeField] private PlantDefinition definition;

    [Header("Fallback si aucune définition")]
    [SerializeField] private string plantId = "menthe";
    [SerializeField] private string displayName = "Menthe";

    [Header("État de jeu")]
    [SerializeField] private bool isCollected;

    public string PlantId => definition != null ? definition.PlantId : plantId;
    public string DisplayName => definition != null ? definition.DisplayName : (string.IsNullOrWhiteSpace(displayName) ? plantId : displayName);
    public string Description => definition != null ? definition.Description : string.Empty;
    public bool IsCollected => isCollected;

    public void MarkCollected()
    {
        isCollected = true;
    }

    public void ResetState()
    {
        isCollected = false;
    }
}
