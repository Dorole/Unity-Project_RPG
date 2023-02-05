# ✷✷✷ RPG PROJECT ✷✷✷

_WIP_

### Dev Log #1
*Feb 05, 2023* <br>
I started this project in June 2022. It was supposed to be a practice 3D project where I would learn and experiment with more complicated systems than what I was used to, but now it has evolved to the point where I'm confident I could make a fun, playable demo game out of it. 
***
### Contents:
* [Phase I](#phase-i)
* [Phase II](#phase-ii)
* [Phase III - TO DO](#phase-iii)
* [Story, Mechanics, Visuals](#story-mechanics-visuals)
***
### Phase I
I first used free low-poly medieval-ish assets where the character came with animations, so it was easy to get it set up. In this first phase I implemented the following:
* **Player Controller**: Click-to-move the player, click on the enemy to attack, click on a pick-up to pick up the item.
* **NavMesh**
* **AI**: AI enemies with the patrol and suspicion behavior; enemies chase and attack if the player comes within a certain range.
* **Binary Formatter and initial setup of the saving system**: everything that wants to save its state implements an ISaveable interface and has a SaveableEntity component which is then used by SavingSystem. Prior to this I only knew how to save with PlayerPrefs. Apparently, saving with the Binary Formatter isn’t optimal either, so I plan to look into transferring to json files. I’m not yet familiar with that system, so it’s on the backlog for now.
* **Moving between different scenes**: the player can go to a different scene by passing through portals. Currently both portals in the first scene led to the same second scene and vice versa. The portals are Scriptable Objects so they are easy to set up.
* **Weapons and equipping**: At this point weapons were Scriptable Objects tied to specific pick-ups, but this was changed later. Each SO took in a pick-up, an equipped prefab, damage, range and an animator override. Some weapons, such as a Bow-and-Arrow could also take in projectiles. At this point, both the player and the enemies could use weapons or be unarmed.
* **VFX and audio**: some weapons (such as a Fireball) use a particle effect; others use a trail effect (Arrows). There are also visual effects for when the arrow hits, as well as audio effects for the character’s grunting/death and weapon hits. I implemented a simple audio randomizer for grunting and hits.
* **Progression**: I defined a progression Scriptable Object with the number of points needed to level-up. Each enemy gives a certain amount of XP points, which is dependent on the enemy’s level. The damage done by weapons also changes with the level the player/enemies are on.
* **Stats and stat modifiers**: Stats are declared as enums. Weapons implement the IModifierProvider interface which modify stats when equipped. At this point, all weapons modify only Damage. They can provide either a value or a percentage modification. This is tracked by the class BaseStats. Since Experience is also a stat, BaseStats keeps track of its value and calculates when it is time to level up.
* **UI**: There is a health bar above the enemy’s and the player’s head which activates when they engage in a fight. The amount of damage inflicted is also displayed above the characters’ heads.
* **Cursor visuals**: The cursor changes depending on what it is above: NavMesh, forbidden/restricted areas, an enemy or a pick-up.
* **Aggro behaviour**: When enemies are attacked, even if the player is out of their range, they start to chase the player (the player, however, cannot attack out of the equipped weapon’s range). If the player can run away far enough or if enough time passes without the enemy being able to attack, the enemy will stop the chase and return to its patrol path. If the aggravated enemy passes by another enemy, the second enemy will join the chase.
* **Camera Controller**: The camera will follow the player by default. It can zoom in/out with a mouse wheel or R/F keys. The camera can be unhooked from the player with the F1 key and hooked back to the player with F2. When it’s unhooked, it can move freely around the scene with WASD/arrows and rotated with Q/E. I have yet to implement camera restrictions because right now it can zoom in/out indefinitely and move out of the scene when it is unhooked.
* **Collector**: the player used to be able to pick up weapons from anywhere on click, but with this the player first moves closer to the pick-up, then picks up the item. This will come in handy later, too. This behaviour is controlled by the PlayerController and implements the IAction interface, like Mover and Fighter classes, so that ActionScheduler class can cancel one action and start another in a smooth manner.   

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
### Phase II:
In the 2nd phase I completely changed the visuals (blessed be Black Friday on Unity Asset Store). I quite like the low-poly art style, so I decided to keep it, but the objects are now in a Japanese style. I had to draw my own sprites for the inventory UI, which is not ideal, but it’s good enough for now. <br>
In this phase the following was implemented:

* **Dragging & dropping**: Once the item is picked up and slotted into inventory, it can be dragged and dropped between various inventories and dropped into the world. If the player tries to equip a non-equippable item or move the item into a slot/place that cannot accept it, the item returns to its original slot. Every slot has a maximum number of acceptable items. This logic is guided by the DragItem class which accepts a generic as a parameter. Currently it is utilized by the InventoryDragItem class.
* **Inventory**: The Inventory class is structured as a player’s “backpack”. It has an overview of the slots, which are free, and which contain an item. It also knows how many items are in each slot and whether more items of the same type can be accepted. Inventory can handle Inventory Item scriptable objects - all other item types inherit from this base type. The logic defined in this class is utilized by the Inventory UI, specifically slot UI, which listens to the changes in the Inventory and redraws its visuals accordingly.
* **Equipment**: This is another class on the Player object. It’s simpler than the Inventory since it can only accept certain types of items (Equippable Item) and the Equipment Slots can only accept one of each item. The slot types are stored in an enum which is used as a key in the dictionary the Equipment uses to store what is currently equipped. Equipment UI also listens to the changes in the Equipment to redraw its visuals. The Fighter class listens to the changes in the Equipment to spawn the equipped weapon in the player’s hand. After the weapon is spawned, the Fighter class takes over. 
* **Action bar**: Another inventory on the Player. It has a set number of Action Slots which accept one or several Action Items. When an item in an Action Slot is clicked on, it’s either used or consumed. Currently using an Action Item has no effect, it just prints out a log message. 
* **Tooltip**: When the mouse is over an item slotted into a slot that implements the IItemHolder interface (currently, all types of slots – inventory, equipment, action and loot – do), the tooltip is enabled. It currently displays only the item name and description.
* **Chests and looting**: At first all pick-ups were individual objects spawned into the world via PickupSpawner class. When an enemy died, it generated a random number of random drops via a DropLibrary scriptable object which held all droppable items, calculated the chance of it being dropped and finally dropped some items based on the chance of anything being dropped, the item’s chance of being dropped and the player’s progression level. I didn’t like this system because it cluttered the scene with objects and spawned new objects every time an enemy died. It was also difficult to see where something was dropped, if anything was in the first place. <br> 
I rewrote the system completely so that the pick-ups are now stored in loot containers and the player can click on the container or a dead enemy, which opens a small UI screen with Loot Slots containing items. By clicking on an item, it is transferred into the inventory. The randomization is calculated using the same DropLibrary as earlier. As every Loot class takes in a DropLibrary, different libraries can be created, for example ones with special items, more powerful items which could be dropped by more powerful enemies, custom libraries etc. This system is cleaner and more practical. I kept the old pick-up system too, in case the player drops an item from the inventory and then wants to pick it back up. I introduced a new interface, ICollectable, which is implemented by Loot and Pickups, so the Collector class can handle them both in the same manner. <br> 
<br> 
One thing I want to add here is the option to **split stacks**, but it is currently on backlog, until all core systems are set up (dialogue, quests, abilities, shops) + not everything that can be slotted into the Equipment or the Action Bar has any (visible) effect yet. For now the weapons are more or less the only thing which is working as it should (gets equipped, the correct animation plays, the damage is done, the projectiles/shurikens fired off). <br> 
I also want to add the possibility to **equip more than one weapon**, specifically for shurikens – I want the player to be able to slot a certain number of them and use them until he spends them. I’m still thinking whether I should adapt the equipment system to do this, or change shurikens to ActionItems instead of EquippableItems. This is also on backlog since it’s not crucial at this stage.

***
### Story, Mechanics, Visuals
*DISCLAIMER: The current setup contains nothing of this. Since this started as a practice RPG project and I’ve come up with the idea for the story only recently, the current setup is what you would normally find in any RPG game. The final product will absolutely utilize these mechanics.*<br>
<br>
Currently I have only bits and pieces, so I won’t describe it in great detail. The story will take place in the world where **multiverse travel** is possible. The MC is a woman detective with the authority to travel between parallel universes to solve crimes. At the start of the story she receives an order to visit an iteration of her hometown, Zagreb. Upon arriving, several things become apparent: another universe has expanded onto the one she found herself in and now they are overlapping, causing a knot in the time-space continuum and creating an unstable amalgamation which will soon lead to either an implosion or, which is far, far less likely, to a complete, stable merge.<br> 
<br>
As the two universes continue to collide, a protrusion into the transcendental dimension has opened which causes fantastical beings to pour out on both sides. Most of them dislike the sudden disturbance and are hostile.<br> 
<br>
Next, the apparatus which enables to travel back to her universe has been broken or someone broke it.<br> 
<br>
Finally, people are being murdered in both overlapping universes and the tenants are blaming each-other.<br> 
<br>
These represent **core missions** for the player: solve murders and fix the apparatus before the universe-collision is complete. This means that the player will be under a time-pressure. If the missions aren’t solved before the time runs out, one of two things will happen to mark the end of the game: either the universes will merge (1% chance) or they will implode (99% chance). If the player succeeds, (s)he will be able to travel to the Multiverse Exploration Agency and bring back people who know how to stabilize the universes, start the separation process and close the gap into the transcendental dimension.<br>
<br>
**Core mechanics**: 
* **Interrogate tenants of both universes and collect evidence into the evidence log**. There will also be a suspect-log, where the player will be able to sort the people (s)he talks to according to whether (s)he finds them suspicious or not. I am thinking about making this automatic, based on the evidence logged. At the end, when it is time to reveal the culprit(s), if the evidence is insufficient, the accused won’t be apprehended, even if the accused is/are true culprits. The evidence will be collected mostly by talking to the NPCs and finding items. After the conversation, the player will be able to mark the lines told by the NPC that sound incriminating into the evidence-log. The number of entries that can be logged will be limited so the player has to choose the lines carefully. Items that are collected can also be marked as evidence, subject to the same limitations as lines.
* **Travel between the two collided universes**. Currently I’m imagining it as both universes always being present, but one is always either transparent or black-and-white. There will be an item which will enable the player to switch the active universe. When the universe is inactive, the player can’t interact with NPCs or items. Since the Zagreb universe is the base one, the player will be able to switch to the other one by using the items (s)he must collect. Every time the item is used, it is spent. The time the player can spend in the other universe will be limited and it will grow as the player’s level grows.  
* **collecting the plates with hiragana characters**. Since the collision caused a knot in a time-space continuum, the place and time that collided with the modern-day Zagreb is Kyoto in the feudal era. This is just a preference of mine, since I already have a lot of Japanese assets and I don’t want to part with them + I don’t want to disappoint the Japanophile in me. <br>
Once the characters that make up a word are collected, the corresponding word(s) will appear in a dictionary. The dictionary will contain the word in hiragana, the translation and the kanji. The player will often have to consult the dictionary to unlock new areas, solve quests etc., but most importantly - without collecting the hiragana characters, the player won’t be able to understand what the tenants of the collided universe are saying.
* **fight hostile beings and send them back to their dimension**. In order to do this, the player will have to collect weapons and improve their skills and abilities. Occasionally the player will have to fight people too and knock the unconscious if the situation calls. <br>
<br>
**Visuals**:
<br> 
I quite like the low-poly visuals I used in the sandbox, so I’d like to keep that look. <br>
The story will start in a noir-looking Zagreb. The player will then travel to the iteration of Zagreb which will consist of **three main maps**, I think: Upper Town, Lower Town and Tunnels. I’ll try to make it look recognizable at least, even if the actual locations probably won’t be reproduced 100% faithfully. <br>
As far as the Japanese universe is concerned, I will be satisfied with making it look like a believable Kyoto village of that era. I think the current sandbox shown here already does a nice job. 

***
### Phase III
*TO DO*: <br>
* Gameplay: First I will implement the general mechanics: dialogue, quests/missions, abilities and shops before moving on to the mechanics specific to this game (unless I get impatient and move to the specifics earlier).
* Backlog: Split stacks; expand tooltip; drop menu on right click on the item in one of the inventories; implement the rate at which the items wear off or get spent until they have to be discarded or taken to a repair shop.
