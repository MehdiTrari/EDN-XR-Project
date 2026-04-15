# Projet Unity XR pour casque PICO

Ce dépôt contient un template de projet Unity préconfiguré pour le développement d'applications de réalité étendue sur casque PICO.


## Récupération du projet

- **Créer un fork du projet** sur votre compte GitHub
 
- **Cloner votre fork** : Ouvrir un terminal et exécuter :
  - `git lfs install`
  - `git clone https://github.com/<votre-nom-utilisateur>/EDN-XR-Project.git`

🏺"L'Herboriste du Grand Siècle" (Histoire & Botanique)
Un retour dans le passé, dans un cabinet de curiosités ou une serre royale.
Le concept : Tu es un savant botaniste chargé de répertorier et d'extraire les essences de plantes exotiques pour le Roi.
Le côté Savant : Le jeu se base sur la botanique réelle. Tu dois consulter un grimoire (UI en français) pour identifier les plantes selon leurs feuilles ou leurs racines avant de les transformer.
Interactions VR : Utiliser un mortier et un pilon pour broyer des herbes, manipuler une loupe pour observer des détails sur les plantes, et classer des bocaux sur des étagères.
Pourquoi ça marche : L'ambiance "vieux cabinet de travail" est très facile à rendre cohérente en VR avec peu d'assets. C'est très calme et éducatif.

lettre manuscrite : demande du roi d'être ...

une étagère vide
des plantes éparpillées
plusieurs types de plantes
on possède un grimoire avec les différentes plantes et manières de les identifier

on doit ramener sur l'étagère UNIQUEMENT les plantes demandées (par exemple : celles entourées sur le grimoire)
une fois les plantes rassemblées
on a un autre livre, de recette ?????? pour aider au mélange
on doit prendre des feuilles des plantes les mélanger avec des épices ou autre
ça fait des potions
Pour donner au roi pcq il est nul à la bagarre et que il veut impressionner la reine et que elle, elle veut elle veut que avec les hommes musclés

## Implémentation gameplay (prototype jouable)

Le projet inclut maintenant une boucle de jeu 3D complète pour **"L'Herboriste du Grand Siècle"** :

1. **Collecte** : le joueur prend les plantes dans la serre et les dépose sur l'étagère de validation.
2. **Recette** : une fois l'étagère validée, le joueur jette les plantes demandées dans le chaudron.
3. **Victoire** : quand la recette est complète, un événement de fin est déclenché.

### Scripts ajoutés
- `PlantItem` : identifie chaque plante avec un `plantId`.
- `HerbariumGameManager` : gère objectifs, progression et textes UI.
- `ShelfDepositZone` : trigger de l'étagère.
- `CauldronRecipeZone` : trigger du chaudron + effet/son.

### Mise en place dans Unity (5 minutes)

1. Ouvrir la scène principale (`Assets/Scenes/SaveurSavante VR.unity`).
2. Créer un objet vide `GameManager` et ajouter **HerbariumGameManager**.
3. (Optionnel) Créer un Canvas World Space + 3 textes TextMeshPro :
   - `QuestText`
   - `RecipeText`
   - `FeedbackText`
   Puis les lier dans l'inspecteur du `GameManager`.
4. Sur chaque plante interactible :
   - ajouter un `Collider` (isTrigger = false),
   - ajouter un `Rigidbody` (si objet manipulable),
   - ajouter le script **PlantItem**,
   - renseigner `plantId` (ex. `menthe`, `lavande`, `romarin`).
5. Sur la zone d'étagère (objet trigger) :
   - ajouter **ShelfDepositZone**,
   - assigner `GameManager`.
6. Sur la zone interne du chaudron (trigger) :
   - ajouter **CauldronRecipeZone**,
   - assigner `GameManager`,
   - (optionnel) assigner `AudioSource` et `SplashEffect`.
7. Dans `HerbariumGameManager` :
   - remplir `Requested Plants`,
   - configurer la `Recipe` (ou laisser vide pour reprendre automatiquement la liste demandée).

### Conseils XR (PICO)
- Garder des colliders simples (box/capsule) pour la performance.
- Limiter les particules et les matériaux transparents dans la serre.
- Utiliser les prefabs de plantes déjà présents pour accélérer le level design.
