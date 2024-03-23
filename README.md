
# Dungeon Frenzy

The sample presents whether Netick is capable to create a fast action online 2D Platformer shooter provided with full source code.

![Preview](https://github.com/StinkySteak/project-firestorm/blob/docs/overview.gif)


| Version | Release Date |
| :-------- | :------- 
| 0.1.0 | 22/03/2024  |

## Technical Info
- Unity: 2021.3.21f1
- Netick 2 Beta 0.10.13
- Platforms: PC (Windows)

## Highlights
### Technical
- Efficient projectile spawn system
- Proper spawn/despawning system
- Custom Interpolation on Weapon rotation
- Optional Lag Compensation scripts
- Server-auth Raycast

### Gameplay
- Double Jump
- Fast paced
- Weapon Heat system

## Guides

### Lag Compensation
#### 1. Updating Script
Lag compensation is a feature available only in Netick Pro, if you have Netick Pro, and wanted to enable It, go ahead to `PlayerCharacterWeapon.cs` and find these lines. You can uncomment isHit for `ShootLagComp` and remove the `#if NETICK_LAGCOMP` symbol definition

```cs
//Enable if you have LagComp (Netick Pro) otherwise use Unity default Raycast
//bool isHit = ShootLagComp(originPoint, direction, out ShootingRaycastResult hitResult);

bool isHit = ShootUnity(originPoint, direction, out ShootingRaycastResult hitResult);
```
#### 2. Adding `HitShape` Component
Follow these Netick documentation on how to add HitShape,
https://netick.net/docs/2/articles/lag-compensation.html
and make sure to also change HitShapeContainer layer to `Player`.
Lastly, change the player's circle collider layer to `Default`

#### 3. Netick Config 
Enable Lag Compensation in Netick Config

#### 4. (Optional) Disable Server-auth LagComp
The Weapon is designed for server auth only, to disable that and allowing clients to predict bullets, remove the `IsServer` check on `ProcessShooting()`

## Credits
- Dungeon Platformer Tile Set (https://incolgames.itch.io/dungeon-platformer-tile-set-pixel-art?download)
