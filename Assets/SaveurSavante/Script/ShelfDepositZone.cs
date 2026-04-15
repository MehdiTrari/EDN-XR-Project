using UnityEngine;

/// <summary>
/// Zone trigger à placer sur l'étagère de collecte.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ShelfDepositZone : MonoBehaviour
{
    [SerializeField] private HerbariumGameManager gameManager;
    [SerializeField] private bool disablePlantAfterDeposit = true;

    private void OnTriggerEnter(Collider other)
    {
        var plant = other.GetComponentInParent<PlantItem>();
        if (plant == null || gameManager == null)
        {
            return;
        }

        var accepted = gameManager.TryRegisterShelfPlant(plant);
        if (!accepted)
        {
            return;
        }

        if (disablePlantAfterDeposit)
        {
            plant.gameObject.SetActive(false);
        }
    }
}
