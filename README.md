# ✷✷✷ RPG PROJECT ✷✷✷

_WIP_

### Description
A practice project in RPG style. 
***
### Features
* Click to move
* Click on an item to move closer and collect
* Click on an enemy to attack (after moving within the weapon range)
* AI enemies notice the player when the player is in range and attack if attacked
* AI enemies alarm nearby enemies to attack the player
* Collect different weapons (katana, bow, fire-ball) or fight barehanded
* Different enemies use different weapons and their health point vary
* Progression system for levelling up
* Portals for moving between different scenes
* Saving/loading (BinaryFormatter)
* Hook and unhook camera with F1/F2, rotate camera with Q/E; move camera with WASD or arrows when unhooked from the player
***
![Demo level design](https://user-images.githubusercontent.com/35565194/206260512-18b1e71f-6818-4635-9a7b-a67684ee6924.png)
*Demo level design*

![The player moves with a mouse click](https://user-images.githubusercontent.com/35565194/206260548-a1c9a49d-6856-482f-8d2d-da4adcb87693.png)
*The player moves with a mouse click.*

![The player attacks the enemies with the collected fire ball](https://user-images.githubusercontent.com/35565194/206260590-675cf1f8-63e6-42b7-a021-6ca7c5fa4f69.png)
*The player attacks the enemies with the collected fire ball.*

![The attacked enemy alerts the nearby enemy and they chase the player. The archer attacks when the player comes within its range.](https://user-images.githubusercontent.com/35565194/206260604-b0b433b6-72f5-41df-b9fb-d93050b07768.png)
*The attacked enemy alerts the nearby enemy and they chase the player. The archer attacks when the player comes within its range.*

***
### This project taught me:
* How to use namespaces properly
* How to decouple code and invert dependencies
* How to use BinaryFormatter for serialization/saving
* How to schedule actions
* How to use animation blend trees
* How to "portal" between scenes using connected portals
* How to implement cutscenes
* How to implement a progression system for levelling up
* How to understand and work with YAML files
***
### TO DO:
Implement inventory systems, quests, dialogues, shops and abilities to create a short RPG prototype.
