using UnityEngine;

/// <summary>
/// Zone trigger à placer dans le chaudron pour valider les ingrédients.
/// </summary>
[RequireComponent(typeof(Collider))]
public class CauldronRecipeZone : MonoBehaviour
{
    [SerializeField] private HerbariumGameManager gameManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject splashEffect;

    private void OnTriggerEnter(Collider other)
    {
        var plant = other.GetComponentInParent<PlantItem>();
        if (plant == null || gameManager == null)
        {
            return;
        }

        var accepted = gameManager.TryRegisterCauldronIngredient(plant);
        if (!accepted)
        {
            return;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (splashEffect != null)
        {
            Instantiate(splashEffect, other.transform.position, Quaternion.identity);
        }

        plant.gameObject.SetActive(false);
    }
}
