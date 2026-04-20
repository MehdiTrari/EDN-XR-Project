using UnityEngine;

/// <summary>
/// Permet de corriger la taille du joueur (XR Origin / PlayerRoot) depuis l'inspecteur.
/// </summary>
public class PlayerSizeAdjuster : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    [Min(0.1f)] [SerializeField] private float playerScale = 1.2f;
    [SerializeField] private bool applyOnStart = true;

    private void Start()
    {
        if (applyOnStart)
        {
            ApplyScale();
        }
    }

    [ContextMenu("Apply Scale")]
    public void ApplyScale()
    {
        if (playerRoot == null)
        {
            Debug.LogWarning("[PlayerSizeAdjuster] playerRoot non assigné.");
            return;
        }

        playerRoot.localScale = Vector3.one * playerScale;
        Debug.Log($"[PlayerSizeAdjuster] Nouvelle taille appliquée: x{playerScale:0.00}");
    }
}
