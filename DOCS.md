# Documentation

### Main Scripts:
 - [BoxHandler.cs](#`BoxHandler.cs`)
 - [AudioManager.cs](#`AudioManager.cs`)
 - [SceneHandler.cs](#`SceneHandler.cs`)
 - [SaveSystem.cs](#`SaveSystem.cs`)
 - [LevelHandler.cs](#`LevelHandler.cs`)
 - [OptionsHandler.cs](#`OptionsHandler.cs`)
# `BoxHandler.cs`
 - Handles box & player behavior

```csharp
private void Start()
 ```
Player and box positions are stored in the start of the level via an array of `Box`'s
```csharp
private void OnMove(InputValue value)
 ```
The `value` parameter will be stored as the input vector for the direction of player movement
```csharp
private bool CheckWallCollisions(Vector2 position, Vector2 moveVector)
 ```
Takes `position` and the resulting destination of a `Box` via the `moveVector` to decide if the `Box` will collide with a wall object
```csharp
public Box CheckBoxCollision(Vector2 position, Vector2 moveVector)
 ```
Same as above but uses `moveVector` against another block
```csharp
private bool PushRowOfBoxes(Vector2 position, Vector2 moveVector)
 ```
When moving, this is used to deal with the player moving and pushing in general.
It checks if there is a box in the direction you are moving, then checks again using that box's center,
And so on until you hit a wall or run out of boxes.
Returns true if the player can move.
If there is not a wall, it will move every box in the row.
```csharp
private bool PullRowOfBoxes(Vector2 position, Vector2 moveVector)
 ```
Pushes in negative move vector.
See `PushRowOfBoxes()` for a similar process.
```csharp
private void CalculateMovementPlayer()
 ```
Deals with sending input to the player target (preventing it from jumping around).
Checks wall and box collision before the movement vector is applied.
```csharp
private void UpdateMoveHistory()
 ```
Saves a history of the positions of every `Box` and `Player` instance per frame as a `List`
```csharp
private void CalculateFallingMovement()
 ```
Checks if the Falling `Box` is supported and calculates the necessary move vector
```csharp
private Vector2 CalcMoveVector(Vector2 input)
 ```
Calculates the direction of the `Player` in terms of a movement vector
```csharp
private void Update()
```
General update loop which calls each function aside from `Start()` in some manner 
# `AudioManager.cs`
- Handles audio output

# `SceneHandler.cs`
- Handles scene loading & transitions

# `SaveSystem.cs`
- Handles save data

# `LevelHandler.cs`
- Handles level logic

# `OptionsHandler`
- Handles option management