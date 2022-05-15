/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

Design
-----------------------------------
I went for an overhead 3D implementation of the classic game, as I wanted to make use 3D effects while maintaining the classic mechanic of the edges of the screen being wrapped around. Also instead of one hit kills, I added health the ship, enemies and asteroids; so it takes more hits to destroy some. The smaller the asteroid the less damage it does, but also the faster it moves across the screen. Another addition is that now your can shoot yourself, so you need to be carefull with your shoots, as they wrap around the edges just like everything else. Additionally I added several powerups, difficulty levels, enemy types and ships you can pilot which are detailed below.

Instructions
-----------------------------------
Use Arrow keys to move and Rigth Control to fire. Watch out for your ship's health and score on the top left. 
Try to evade or destroy the asteroids and enemies, while looking for power ups.

PowerUps
-----------------------------------
Repair Kit - Restores health
Mega Bomb - Destroys all asteroids
Triple Shoot - Shoots triple bullets
Invincibility - Force field that protects from damage

Enemies
-----------------------------------
3 Types. They move across the screen and fire on the player at different rates. They also require different ammount of shoots to take down. The faster the enemy, the less shoots the player needs to do so.

Ships
-----------------------------------
3 Types. Heavy(more health slower), Standard and Fast(less health but faster). The faster the ship the smaller it is, including it's collider, which makes it easier to miss asteroids.

Difficulties
-----------------------------------
3 Types. The higher the more large asteroids at the same time you will have and the faster enemies will spawn/move.

Assets Used
-----------------------------------
Ultimate Spaceship Creator
Forge3D Planets
ToDo List
SciFi Pixel Art UI Kit
Sci-Fi Arsenal
3D Starfields
Ultimate Game Music Collection