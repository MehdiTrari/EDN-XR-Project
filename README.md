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

## Mise à jour gameplay (itération 2)

Suite aux retours équipe, le gameplay a été renforcé pour correspondre exactement au besoin :

- **Déplacement dans la serre** : ajout d'un contrôleur de déplacement simple (`SimpleGreenhouseMovement`) pour test desktop rapide (clavier/souris), en complément du mode XR.
- **Grimoire / Book de botanique** : ajout d'un système de pages avec **image + description** des plantes à trouver (`HerbariumBookUI`).
- **Données de plantes** : ajout de `PlantDefinition` (ID, nom, description, illustration).
- **Recette soupe du Roi** : ajout de `SoupRecipeDefinition` pour définir la demande royale et les quantités d'ingrédients.
- **Taille du personnage** : ajout de `PlayerSizeAdjuster` pour augmenter la taille du joueur (XR Origin / PlayerRoot) directement depuis l'inspecteur.

### Nouveau flow recommandé

1. Le joueur lit la lettre / demande royale.
2. Il ouvre le grimoire pour voir les plantes nécessaires (texte + image).
3. Il se déplace dans la serre, attrape les plantes demandées.
4. Il valide les plantes demandées (étape étagère).
5. Il dépose les plantes dans le chaudron selon les quantités demandées.
6. La soupe est complétée et l'événement final est déclenché.

### Configuration Unity détaillée (nouveaux scripts)

1. **Créer les données de plantes** :
   - `Create > SaveurSavante > Plant Definition`
   - Renseigner `plantId`, `displayName`, `description`, `illustration`.
2. **Créer la recette** :
   - `Create > SaveurSavante > Soup Recipe`
   - Renseigner le nom de recette, le texte de demande royale, puis la liste des plantes + quantités.
3. **Configurer le manager** (`HerbariumGameManager`) :
   - Assigner la `SoupRecipeDefinition`.
   - Ajouter les `PlantDefinition` au `plantCatalog` (si besoin).
4. **Configurer les plantes 3D** :
   - Sur chaque plante de la scène: `PlantItem` + référence vers la bonne `PlantDefinition`.
5. **Configurer le grimoire UI** (`HerbariumBookUI`) :
   - Lier les champs TextMeshPro (titre, page, nom, description, quantité) + `Image`.
   - Lier le `HerbariumGameManager`.
   - Connecter des boutons UI à `NextPage()` et `PreviousPage()`.
6. **Ajuster la taille joueur** :
   - Ajouter `PlayerSizeAdjuster` sur un objet technique.
   - Assigner `playerRoot` (souvent XR Origin).
   - Régler `playerScale` (ex: `1.2` à `1.4`) puis `Apply Scale`.
7. **Déplacement test non-VR** :
   - Ajouter `SimpleGreenhouseMovement` sur un objet joueur avec `CharacterController`.

### Remarque
- Le README est mis à jour à chaque itération pour garder une description fidèle de l'état actuel du jeu.

## Résolution de conflits Git (important)

Si Git affiche des conflits dans les scripts (`HerbariumGameManager`, `PlantItem`, etc.), **ne pas valider automatiquement "Both changes" partout**.

### Pourquoi
- "Both changes" peut dupliquer des champs/méthodes (`requestedPlants`, `Start()`, `GetRecipeCount()`, etc.).
- En C#, ces doublons créent des erreurs de compilation Unity (symboles redéfinis, blocs incomplets, accolades cassées).

### Règle simple
- Garder **une seule version cohérente** de chaque bloc de code.
- Supprimer tous les marqueurs de conflit (`<<<<<<<`, `=======`, `>>>>>>>`).
- Vérifier qu'il n'y a qu'une seule déclaration par membre (un seul champ, une seule méthode du même nom/signature).

### Check rapide avant commit
1. Ouvrir la Console Unity : aucune erreur CS0102/CS0111 (membres dupliqués).
2. Vérifier que ces fichiers compilent sans doublons :
   - `Assets/SaveurSavante/Script/HerbariumGameManager.cs`
   - `Assets/SaveurSavante/Script/PlantItem.cs`
3. Tester en Play Mode : progression étagère -> chaudron toujours fonctionnelle.
