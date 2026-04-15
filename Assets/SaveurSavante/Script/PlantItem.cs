using UnityEngine;

/// <summary>
/// Données d'une plante manipulable dans la serre.
/// </summary>
public class PlantItem : MonoBehaviour
{
    [Header("Identité")]
    [SerializeField] private string plantId = "menthe";
    [SerializeField] private string displayName = "Menthe";

    [Header("État de jeu")]
    [SerializeField] private bool isCollected;

    public string PlantId => plantId;
    public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? plantId : displayName;
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
