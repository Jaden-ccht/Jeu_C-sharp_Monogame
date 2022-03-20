# VRCouchotCellier

## Introduction

Mystic Woods est un jeu réalisé dans le cadre du cours de Réalité Virtuel en 2ème année de DUT Informatique à l'IUT de Clermont-Ferrand.
Nous avons choisi le framework Monogame pour réalisé ce jeu.


## But du jeu

###### Objectif du jeu

Vous incarnez un personnage qui doit nettoyer une étrange forêt peuplé de gobelin et de champignons malveillants.
Trois niveaux sont disponibles, plus vous avancez dans les niveaux, plus ça se complique.

Si vous avez exterminez tous les monstres de chaque niveau, vous pourrez sortir de la forêt.

###### Commandes

Z et la flèche directionnelle du haut vous permettront de monter.
Q et la flèche directionnelle de gauche vous permettront d'aller sur la gauche.
S et la flèche directionnelle du bas vous permettront de descendre.
D et la flèche directionnelle de droite vous permettront d'aller sur la droite.
La barre d'espace vous permettra de donner des coups.

Vous pouvez utliser deux touches de déplacement en même temps pour pouvoir se déplacer en diagonale (si le deux touches de déplacement sont des opposés, votre personnage ne pourra pas se déplacer).

## Architecture

###### Classes et packages

Le jeu contient quatre packages :

- Animation qui contient deux classes Animation et AnimationPlayer, Animation est une classe métier recensant l'intégralité des paramètres, getters et setters nécessaires et AnimationPlayer qui permet à chaque entité d'avoir des animations que ça soit pour les déplacements, les morts, les coups ou bien même quand rien fait.

- Collisions est un package qui contient une classe Collisionneur. Cette classe est destiné à la gestion des collision entre les éléments de la TiledMap et les entités (Character, Gobelin, Mush) mais aussi les collisions entre le personnage et les mobs.

- GameEntities est un package recensant toutes les classes pour les entités. La classe mère est GameObject, cette dernière hérite de DrawableGameComponent, elle contient un constructeur qui fait passé en argument Game et un spritebach afin que toutes les entités héritent du même spritebach. 
Entité est une classe qui recense tous les attributs et méthodes que chaque entité doivent hériter. 
Character est la classe destiné au personnage que l'on incarne avec les textures dont il a besoin.
Mob est une classe que les monstres vont hériter afin de les différencier du personnage et Mush/Gobelin sont des classes nécessaires aux chargements des textures liées à ces derniers.

- Levels est le package contenat la classe Level qui permettra la gestion de niveau que ça soit le chargement correcte de la map liée au niveau mais aussi l'instanciation de la liste de mobs, etc.

- La classe Game est le main de notre jeu. Il contient le menu et fait appel aux classes cités précédemment.

###### Packages NuGet

Nous avons utilisé :

- Apos.Gui destiné à la création d'un menu afin de quitter ou lancer une partie, entrez un pseudonyme ou une vue de fin personnalisé en fonction d'une partie réussite ou non.

- Monogame.Extended (.Pipeline et .Tiled) sont destinés pour les maps car les maps ont été créer sur un logiciel nommé Tiled.

## NOTES IMPORTANTES 

Un problème de clone du projet entraîne un oubli de copie de la police d'écriture de notre jeu.  

Pour résoudre ce problème, il faut 

- Récupérer la police d'écriture dans vrcouchotcellier\code\ProjetVR.Core\Content\dpcomic.ttf

- Coller la dans vrcouchotcellier\code\ProjetVR.DesktopGL\bin\Debug\netcoreapp3.1\Content

- Normalement, le projet devrait s'éxécuter normalement si votre SDK est à jour et que vous avez mgcb-editor sur votre appareil.