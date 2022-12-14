# VRCouchotCellier


## Introduction

Mystic Woods est un jeu réalisé dans le cadre du cours de Réalité Virtuel en 2ème année de DUT Informatique à l'IUT de Clermont-Ferrand.

Nous avons choisi le framework Monogame pour réaliser ce jeu.

###### Vidéo de présentation 

[![Vidéo](https://img.youtube.com/vi/fsD2U7LXFfQ/0.jpg)](https://www.youtube.com/watch?v=fsD2U7LXFfQ)

## But du jeu

###### Objectif du jeu

Vous incarnez un personnage qui doit nettoyer une étrange forêt peuplée de gobelin et de champignons malveillants.

Trois niveaux sont disponibles, plus vous avancez dans les niveaux, plus ça se complique.

Si vous avez exterminé tous les monstres de chaque niveau, vous pourrez sortir de la forêt.

###### Commandes

Z et la flèche directionnelle du haut vous permettront de monter.
Q et la flèche directionnelle de gauche vous permettront d'aller sur la gauche.
S et la flèche directionnelle du bas vous permettront de descendre.
D et la flèche directionnelle de droite vous permettront d'aller sur la droite.
La barre d'espace vous permettra de donner des coups.

Vous pouvez utiliser deux touches de déplacement en même temps pour pouvoir vous déplacer en diagonale (si les deux touches de déplacement sont opposées, votre personnage ne pourra pas se déplacer).


## Architecture

###### Classes et packages

Le jeu contient quatre packages :

- Animation qui contient deux classes : Animation et AnimationPlayer, Animation est une classe métier recensant l'intégralité des paramètres, getters et setters nécessaires et AnimationPlayer qui permet à chaque entité d'avoir des animations que ça soit pour les déplacements, les morts, les coups ou lors du repos de l'entité.

- Collisions est un package qui contient une classe Collisionneur. Cette classe est destinée à la gestion des collisions entre les éléments de la TiledMap et les entités (Character, Gobelin, Mush) mais aussi les collisions entre le personnage et les mobs.

- GameEntities est un package recensant toutes les classes pour les entités. La classe mère est GameObject, cette dernière hérite de DrawableGameComponent, elle contient un constructeur qui fait passer en argument Game et un spritebatch afin que toutes les entités héritent du même spritebatch.
Entité est une classe qui recense tous les attributs et méthodes dont chaque entité doit hériter.
Character est la classe destinée au personnage que l'on incarne avec les textures dont il a besoin.
Mob est une classe dont les monstres vont hériter afin de les différencier du personnage et Mush/Gobelin sont des classes nécessaires aux chargements des textures liées à ces derniers.

- Levels est le package contenant la classe Level qui permettra la gestion de niveau que ça soit le chargement correct de la map liée au niveau, mais aussi l'instanciation de la liste de mobs, etc.

- Movements contient la classe MovementManager qui est un déplaceur d'entité que ça soit le personnage en fonction de la touche pressée ou bien les mobs en fonction de la position du personnage.

- La classe Game est le main de notre jeu. Il contient le menu et fait appel aux classes citées précédemment.

###### Packages NuGet

Nous avons utilisé :

- Apos.Gui destiné à la création d'un menu afin de quitter ou lancer une partie, entrer un pseudonyme ou une vue de fin personnalisée en fonction d'une partie réussie ou non.

- Monogame.Extended (.Pipeline et .Tiled) sont destinés pour les maps, car les maps ont été créées sur un logiciel nommé Tiled.


## NOTES IMPORTANTES

Un problème de clone du projet entraîne un oubli de copie de la police d'écriture de notre jeu.

Pour résoudre ce problème, il faut :

- Récupérer la police d'écriture dans vrcouchotcellier\code\ProjetVR.Core\Content\dpcomic.ttf

- Collez la dans vrcouchotcellier\code\ProjetVR.DesktopGL\bin\Debug\netcoreapp3.1\Content

- Normalement, le projet devrait s'exécuter normalement si votre SDK est à jour et que vous avez mgcb-editor sur votre appareil.
