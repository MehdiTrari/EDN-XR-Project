using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronLogic : MonoBehaviour
{
    [Header("Paramètres")]
    public string plantTag = "Plante"; // Pour identifier tes plantes
    public GameObject splashEffect;    // Optionnel : un petit effet de particules

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // On verifie si l'objet qui entre a le bon tag
        if (other.CompareTag(plantTag))
        {
            Debug.Log("Une plante est tombee dans le chaudron : " + other.gameObject.name);

            // 1. Jouer un son (reutilise ta logique PinAudio si tu veux)
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null) audio.Play();

            // 2. Creer un effet visuel (si tu en as un)
            if (splashEffect != null)
            {
                Instantiate(splashEffect, other.transform.position, Quaternion.identity);
            }

            // 3. Faire disparaitre la plante
            // On desactive l'objet plutot que de le detruire (mieux pour les perfs)
            other.gameObject.SetActive(false);

            // TODO: Ici tu pourras ajouter un point a ton "Livre de Recettes"
        }
    }
}
