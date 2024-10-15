
# Dungeon Frenzy

The sample presents whether Netick is capable to create a fast action online 2D Platformer shooter provided with full source code.

![Preview](https://github.com/StinkySteak/project-firestorm/blob/docs/overview.gif)


| Version | Release Date |
| :-------- | :------- 
| 0.1.2 | 03/04/2024  |

## Technical Info
- Unity: 2021.3.21f1
- Netick 2 Beta 0.11.16
- Platforms: PC (Windows)

## Highlights
### Technical
- Efficient projectile spawn system
- Proper spawn/despawning system
- Custom Interpolation on Weapon rotation
- Optional Lag Compensation scripts
- Server-auth Raycast
- Custom Execution Order

### Gameplay
- Double Jump
- Fast paced
- Weapon Heat system

### Gameplay Simulation vs Visual

Anything about simulation and visual is very recommended to be seperated. I Recommend this design, because it is good for your clean code & easier to manage if you plan to build a dedicated server (or headless server). You can disable the component entirely if its a headless server.

In this project, we seperate Health Logic & It's visual. In this example case It was the spawning the VFX when the player's get hit.
The Logic class broadcast the event and let the visual component handles the spawning VFX.



#### Gameplay Simulation
```cs
     public class PlayerCharacterHealth : NetworkBehaviour
     {
     [Networked] private int _health { get; set; }

     public event Action OnHealthChanged;
     public event Action OnHealthReduced;

     public int Health => _health;

     public void ReduceHealth(int amount)
     {
         _health -= amount;
     }

     [OnChanged(nameof(_health))]
     private void OnChangedHealth(OnChangedData onChangedData)
     {
         OnHealthChanged?.Invoke();

         int previousHealth = onChangedData.GetPreviousValue<int>();
         int currentHealth = _health;

         if (currentHealth < previousHealth)
         {
             OnHealthReduced?.Invoke();
         }
     }
```
#### Visual
```cs
public class PlayerCharacterHealthVisual : NetickBehaviour
{
    [SerializeField] private PlayerCharacterHealth _health;
    [Space]
    [SerializeField] private GameObject _vfxBloodPrefab;

    public override void NetworkStart()
    {
        _health.OnHealthReduced += OnDamaged;
    }

    private void OnDamaged()
    {
        Sandbox.Instantiate(_vfxBloodPrefab, transform.position, Quaternion.identity);
    }
```

### Custom Interpolation
We were unable to use the default interpolation from Netick for the weapon rotation visual, there could be a case where a "rotation wrapping exist" meaning the rotation is e.g resetting from 359 to 1 instead of continuing to 360 or 361.

```cs

 public class PlayerCharacterWeapon : NetworkBehaviour
 {
     [Networked][Smooth(false)] public float Degree { get; private set; }
 }
```

```cs
 public class PlayerCharacterWeaponVisual : NetickBehaviour
 {
     [SerializeField] private PlayerCharacterWeapon _weapon;
     [SerializeField] private Transform _weaponVisual;
     [SerializeField] private SpriteRenderer _weaponRenderer;

     public override void NetworkRender()
     {
         UpdateWeaponRotationVisual();
     }

     private void UpdateWeaponRotationVisual()
     {
         var interpolator = _weapon.FindInterpolator(nameof(_weapon.Degree));
         bool didGetData = interpolator.GetInterpolationData(InterpolationSource.Auto, out float from, out float to, out float alpha);

         float interpolatedDegree;
        
         if (didGetData)
             interpolatedDegree = LerpDegree(from, to, alpha);
         else
             interpolatedDegree = _weapon.Degree;

         _weaponVisual.rotation = Quaternion.Euler(0, 0, interpolatedDegree);

         bool flipY = _weapon.Degree < 89 && _weapon.Degree > -89;
         _weaponRenderer.flipY = !flipY;
     }

     private const float INTERPOLATION_TOLERANCE = 100f;

     private float LerpDegree(float from, float to, float alpha)
     {
         float difference = Mathf.Abs(from - to);

         if (difference >= INTERPOLATION_TOLERANCE)
         {
             return to;
         }

         return Mathf.Lerp(from, to, alpha);
     }
```

### Player Management Architecture
#### Player Session vs Player Character
There are multiple ways to manage players (keep track, spawning, despawn)

A Player existence on the session is represented by `PlayerSession`, the object is not visible whatsoever, and It's purpose is to store about the player state e.g Nickname, Score, Player's team.

While the player we control was `PlayerCharacter` that has gameplay property such as health, position, weapons.

`PlayerSession` Network Object will be spawned immediately/destroyed the time a player joined/left the session.

#### Management
In order to keep track the player's list (both `PlayerSession` and `PlayerCharacter`) there are 2 managers that can handle this which are `GlobalPlayerManager` and `LocalPlayerManager`.

#### GlobalPlayerManager
- Keep track of all players
- Broadcast events when a player is registered/removed

#### LocalPlayerManager
- Keep track of only local player
- Broadcast events whenever local player is registered/removed

#### Drawback
There is a drawback on this architecture. We register each player's on `Spawned()` callback to the manager. However there could a racing condition where the `PlayerCharacter` was trying to access It's `PlayerSession` on `Spawned()` eventhough the `PlayerSession` hasn't been registered to the `GlobalPlayerManager`.

~~This is because Netick `Spawned()` execution order won't be the same for each peers (not deterministic). A Current solving technique for this is, on `OnSceneLoaded()` callback from `NetworkEventsListener`, we uses the `FindObjectOfTypes` API to register the player's object outside `Spawned()` callback.~~ 

This now is solved by using `[ExecutionOrder]` attribute, this lets us to customize the NetworkStart order from each network behaviour (Netick 2 Beta 0.11.16)

```cs
    //Will be executed first (lower is priority)
    [ExecutionOrder(-100)]
    public class PlayerSession : NetworkBehaviour
    {

    }
```

```cs
    [ExecutionOrder(-99)]
    public class PlayerCharacter : NetickBehaviour
    {

    }
```

It's good for you to know `OnSceneLoaded()` is called earlier than any network object `Spawned()`.

### Initializing UI
If you take a look at `GUIGameplay.cs` It implements `INetickSceneLoaded` interface. A Custom Interface made for this project (not built-in from Netick). This interface will be searched in the scene from `MatchManager`

Because of that, `GUIGameplay` now has the access of `NetworkSandbox` and may listen to the simulation. Another case for this is the `CameraManager`

```cs
public class GUIGameplay : MonoBehaviour, INetickSceneLoaded
{
    // ...
    private NetworkSandbox _networkSandbox;

    public void OnSceneLoaded(NetworkSandbox sandbox)
    {
        _networkSandbox = sandbox;

        // Logic goes here...
    }
```

```cs
public class MatchManager : NetworkEventsListener
{
    public override void OnSceneLoaded(NetworkSandbox sandbox)
    {
        List<INetickSceneLoaded> listeners = ObjectFinder.FindPreAlloc<INetickSceneLoaded>();

        foreach (INetickSceneLoaded listener in listeners)
            listener.OnSceneLoaded(sandbox);
    }
```

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

### Alternative to Lag Compensation
#### 1. Enable raycast prediction
Raycast immediately without waiting for server, there is a `IsServer` check to disable prediction.

#### 2. Client-side auth
Using a RPC to deal damage to the players on hit

### Cinemachine Sandboxing
This project is compatible with Netick sandboxing. However, by default This doesn't really do well for Cinemachine. Disabling just the camera won't be enough, we also have to disable the `CinemachineVirtualCamera` component. This issue has been solved in `CameraManager`. This code is for editor only and can be a little expensive to running it everytime, because we only this feature only in editor
```cs
        public void OnSceneLoaded(NetworkSandbox sandbox)
        {
            AttachBehaviour(sandbox);
            // ....
        }
        private void AttachBehaviour(NetworkSandbox sandbox)
        {
#if UNITY_EDITOR
            sandbox.AttachBehaviour(this);
#endif
        }
#if UNITY_EDITOR

        public override void NetworkRender()
        {
            _cinemachineVirtualCamera.enabled = Sandbox.IsVisible;
        }
#endif
```

## To Do/Issues
- Empty

## Credits
- Dungeon Platformer Tile Set (https://incolgames.itch.io/dungeon-platformer-tile-set-pixel-art?download)
