# Wheel Of Luck Package Documentation
 
## Overview

The WheelOfLuck package provides a tools for creating and managing a "Wheel of Luck" feature, commonly used in games. This package includes interfaces, classes, and editors to facilitate the creation, customization, and management of wheel items, spin costs, and the view of the wheel.

## Package Structure
> - `WheelOfLuck/Demo` - contains all assets with example usage of package
### Scene
> - `WheelOfLuck/Demo/WheelOfLuckDemoScene` - scene with example of Wheel Of Luck
### Assets
> - `WheelOfLuck/Prefab` - contains prefabs which you should use (you can create your own)
> - `WheelOfLuck/Sprites` - folder with sprites, some of them was used for example (required for default prefabs)
### Scripts
> - `WheelOfLuck/Scripts` - main scripts folder
> - `WheelOfLuck/Scripts/Editor` - contains scripts with custom editor features for this package
> - `WheelOfLuck/Scripts/Interfaces`
> - `WheelOfLuck/Scripts/Models` - has scripts for basic models of the wheel item, item package, etc.
> - `WheelOfLuck/Scripts/Controllers` - has scripts that make up the wheel (wheel controller and spin cost)
> - `WheelOfLuck/Scripts/View` - has a WheelDrawer script, used to customize the appearance of the wheel

## Documentation:

## Architecture
### Interfaces
- ICollectable +
- IReadOnlyWheelItem + 
### Abstract Classes 
- BaseSpinCostProvider +
- WheelItem : IReadOnlyWheelItem, ICollectable +
### Models
- WheelItemsPack +
### Controllers
- SpinController +
- BaseSpinCostProvider +
### View
- WheelDrawer +
### Editor
- WheelItemsPackEditor + 
### Main Class
- WheelOfLuck

<details open>
<summary><h2>ICollectable Interface<h2></summary>

## ICollectable Interface
### Overview

    The ICollectable interface defines a contract for items that can be collected in the "Wheel of Luck" game. Any class implementing this interface must provide an implementation for the Collect method.

## Methods
### Collect

    Defines the logic to be executed when the item is collected.

```C#
void Collect();
```

## Usage Example

    To use the ICollectable interface, you would implement it in a class and provide the logic for the Collect method.

```C#
public class CoinItem : ScriptableObject, ICollectable
{
    public void Collect()
    {
        // Implement logic to add coins to the player's balance.
        Player.Coins += 10;
        Debug.Log("Collected 10 coins!");
    }
}
```

### In a MonoBehaviour script:

```C#
void Start()
{
    wheelItem.Collect();
}
```

</details>

<details open>
<summary><h2>IReadOnlyWheelItem Interface</h2></summary>

## IReadOnlyWheelItem Interface
### Overview

    The IReadOnlyWheelItem interface defines a contract for read-only properties of wheel items in the "Wheel of Luck" game. Any class implementing this interface must provide read-only access to the Icon and Label properties.

## Properties

```C#
public Sprite Icon { get; }
// Gets the icon representing the wheel item.

public string Label { get; }
// Gets the label of the wheel item.
```

## Usage Example

    To use the IReadOnlyWheelItem interface, you would implement it in a class and provide the logic for the Icon and Label properties.

```C#
public class CoinItem : WheelItem, IReadOnlyWheelItem, ICollectable
{
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;

    public string Label => "Coin";
    
    public override void Collect()
    {
        Debug.Log("You collect Coin");
    }
}
```
### In a MonoBehaviour script:

```C#
void Start()
{
IReadOnlyWheelItem coinItem = ScriptableObject.CreateInstance<CoinItem>();
Debug.Log("Item label: " + coinItem.Label);
Debug.Log("Item icon: " + coinItem.Icon.name);
}
```
</details>

<details open>
<summary>Wheel Item Class</summary>

## WheelItem Class
### Overview

    The WheelItem class is an abstract class representing an item on the wheel in the "Wheel of Luck" game. It implements both the ICollectable and IReadOnlyWheelItem interfaces, providing the basic properties and methods required for wheel items.

### Inherits From

    ScriptableObject, ICollectable, IReadOnlyWheelItem

### Properties

```C#
public Sprite Icon => _icon;
    // The icon representing the wheel item.
    
public abstract string Label { get; }
    // The label of the wheel item, to be implemented by derived classes.
```

### Serialized Fields

```C#
[SerializeField] private Sprite _icon;
    // The icon for the wheel item.

[Tooltip("Probability in %")]
[Range(0f, 100f)]
public float Chance = 100f;
    // The probability of the item being selected, adjustable in the range from 0 to 100%.

[HideInInspector] public int Index;
    // The index of the item on the wheel, hidden in the inspector.

[HideInInspector] public double _weight = 0f;
    // The weight of the item, used for internal calculations, hidden in the inspector.
```

## Public Methods
### Collect

> Abstract method that handles the logic when the item is collected. Must be implemented by derived classes.

```C#
public abstract void Collect();
```

## Usage Example

    To use the WheelItem class, you would create a derived class that implements the abstract properties and methods.

```C#
[CreateAssetMenu(menuName = "WheelOfLuck/Items/CoinItem")]
public class CoinItem : WheelItem
{
    public override string Label => "Coin";

    public override void Collect()
    {
        // Implement logic to add coins to the player's balance.
        Player.Coins += 10;
        Debug.Log("Collected 10 coins!");
    }
}
```

### In a MonoBehaviour script:

```C#
void Start()
{
    WheelItem coinItem = ScriptableObject.CreateInstance<CoinItem>();
    coinItem.Collect();
    Debug.Log("Item label: " + coinItem.Label);
}
```

</details>

<details open>
<summary><h2>Wheel Items Pack Class</h2></summary>

## WheelItemsPack Class
### Overview

    The WheelItemsPack class represents a pack of wheel items in the "Wheel of Luck" game. It handles the selection, replacement, and weighting of items on the wheel.

### Inherits From

    ScriptableObject

## Properties

```C#
public List<WheelItem> Items => _selectedItems;
    // The list of selected items for the wheel.
    
public IReadOnlyList<int> NonZeroChanceItemIndexes => _selectedItemIndexes;
    // The list of indexes of items with a non-zero chance.
    
public double TotalWeight => _accumulatedWeight;
    // The total accumulated weight of the items.
```

## Serialized Fields

```C#
[SerializeField] private int _firstItemGrantedCount = 2;
    // The number of items guaranteed to be granted at the start.

[SerializeField] private int _itemsOnWheelCount = 5;
    // The number of items to be displayed on the wheel.

[SerializeField] private int _consumableItemsCount = 3;
    // The number of consumable items to be included in the pack.

[SerializeField] private int _uniqueItemsCount = 2;
    // The number of unique items to be included in the pack.

[Header("Pool of wheel's items")]
[Tooltip("Will drop as ordered")]
[SerializeField] private List<WheelItem> _grantedItems;
    // The list of items guaranteed to be granted.

[SerializeField] private List<WheelItem> _itemsPool;
    // The pool of available items for selection.

[Space]
[Header("Automated")]
[SerializeField] private List<WheelItem> _selectedItems;
    // The list of selected items for the wheel. Ganerates automaticly OnValidate or class method invokation.
```

## Private Fields

```C#
private int _grantedCollectedItemsCount = 0;
    // The count of granted items that have been collected.

private double _accumulatedWeight;
    // The accumulated weight of the items.

private List<int> _selectedItemIndexes;
    // The list of selected item indexes.
    
private int previousParam1;
    // A variable used for validation of consumable items count.

private int previousParam2;
    // A variable used for validation of unique items count.
```

## Public Methods
### ReplaceItem

> Replaces a collected item with a new item from the pool and recalculates weights and indices.

```C#
public void ReplaceItem(WheelItem collectedItem)
```
    Parameters:

        collectedItem: The item that was collected and needs to be replaced.

### GetRandomItemIndex

> Gets a random item index based on the weights of the items.

```C#
public virtual int GetRandomItemIndex()
```
    Returns:
        The index of the randomly selected item.

### CalculateWeightsAndIndices

> Calculates the weights and indices for the selected items.

```C#
public void CalculateWeightsAndIndices()
```

## Private Methods
### OnValidate

> Validates the wheel items and items pool, then calculates weights and indices.

```C#
private void OnValidate()
```

### ValidateWheelItems

>Validates the consumable and unique items counts, ensuring they match the total items on the wheel.

```C#
private void ValidateWheelItems()
```

### ValidateItemsPool

> Validates and selects items from the pool to be included in the wheel.

```C#
private void ValidateItemsPool()
```

## Context Menu Methods
## Reset

>Resets the accumulated weight and the count of granted collected items.

```C#
[ContextMenu("Custom Reset")]
public void Reset()
```

</details>

<details open>
<summary><h2>Spin Controller Class</h2></summary>

# SpinController Class
## Overview

    The SpinController class manages the spinning of the wheel. It handles the initialization of the wheel items, the spinning mechanics, and the determination of the winning item.

## Inherits From
    MonoBehaviour

## Serialized Fields

```C#
[Header("Wheel settings :")]

[Range(1, 20)]
[SerializeField] private float _spinDuration = 8;
    // The duration of the spin, adjustable in the range from 1 to 20.

[Range(1, 3)]
[SerializeField] private int _spinsBeyond360 = 1;
    // The number of spins beyond 360 degrees, adjustable in the range from 1 to 3.

[SerializeField] private Ease _spinEasing = Ease.InOutQuart;
    // The easing function for the spin animation.

[Space]
[SerializeField] private Transform _wheelCircle;
    // The transform of the wheel circle.
```

## Events

```C#
public event Action OnSpinStart;
    // Event triggered at the start of the spin.

public event Action<WheelItem> OnSpinEnd;
    // Event triggered at the end of the spin, passing the winning WheelItem.
```

## Private Fields

```C#
private IReadOnlyList<WheelItem> _items;
    // The list of items on the wheel.

private float _itemAngle;
    // The angle between each item on the wheel.

private float _halfItemAngle;
    // Half of the angle between each item on the wheel.

private float _halfItemAngleWithPaddings;
    // Half of the angle between each item on the wheel, adjusted for padding.
```

## Methods

### Initialize Method

> Initializes the wheel with a given list of items. Sets up the item angles.

```C#
public void Initialize(IReadOnlyList<WheelItem> items)
```

    Parameters:

        items: The list of items to be displayed on the wheel.

### Spin Method

> Spins the wheel to a random item index or a selected item index based on the provided indexes.

```C#
public void Spin(int randomItemIndex, IReadOnlyList<int> selectedItemsIndexes)
```

    Parameters:

        randomItemIndex: The index of the item to be randomly selected.
        selectedItemsIndexes: The list of selected item indexes to choose from if the random item has a chance of 0.

## Private Methods

### OnUpdate Method

> Updates the rotation and handles the indicator crossing the item line.

    Note: This method is part of the sequence in the Spin method and does not have its own signature.

### AppendCallback

Invokes the OnSpinEnd event with the winning item.

    Note: This method is part of the sequence in the Spin method and does not have its own signature.

</details>

<details open>
<summary><h2>Base Spin Cost Provider Class</h2></summary>

## BaseSpinCostProvider Class
### Overview

    The BaseSpinCostProvider class is an abstract class that provides the base functionality for determining if a spin is available and handling the cost of a spin in the "Wheel of Luck" game.

## Inherits From

    MonoBehaviour

## Properties

```C#
public bool Available { get; protected set; }
    // Indicates whether a spin is currently available.
```

## Events

```C#
public event Action<bool> SpinAvailableUpdated;
    // Event triggered when the availability of a spin is updated, passing the new availability status.
```

## Protected Methods

### UpdateAvailable
> Abstract method that updates the availability status of a spin. Must be implemented by derived classes.

```C#
protected abstract void UpdateAvailable();
```

### OnSpinAvailableUpdated
> Invokes the SpinAvailableUpdated event with the new availability status. Can be overridden by derived classes.

```C#
protected virtual void OnSpinAvailableUpdated(bool available)
```
    Parameters:

        available: The new availability status to be passed to the event.

## Public Methods
### OnSpinStart

> Abstract method that handles the logic when a spin starts. Must be implemented by derived classes.

```C#
public abstract void OnSpinStart();
```

## Usage Example

    To use the BaseSpinCostProvider class, you would create a derived class that implements the abstract methods and potentially overrides the virtual method.

```C#
public class CoinSpinCostProvider : BaseSpinCostProvider
{
    protected override void UpdateAvailable()
    {
        // Implement logic to update the availability status based on the player's coin balance.
        Available = Player.Coins >= spinCost;
        OnSpinAvailableUpdated(Available);
    }

    public override void OnSpinStart()
    {
        // Implement logic to deduct the cost of a spin from the player's coin balance.
        Player.Coins -= spinCost;
        UpdateAvailable();
    }
}
```

### In a MonoBehaviour script:

```C#
void Start()
{
    CoinSpinCostProvider spinCostProvider = GetComponent<CoinSpinCostProvider>();
    spinCostProvider.SpinAvailableUpdated += OnSpinAvailableUpdated;

    // Initialize and check spin availability.
    spinCostProvider.UpdateAvailable();
}

void OnSpinAvailableUpdated(bool available)
{
    // Handle spin availability update (e.g., update UI).
    Debug.Log("Spin available: " + available);
}
```

</details>


<details open>
<summary><h2>Wheel Drawer Class</h2></summary>

## WheelDrawer Class
### Overview

    The WheelDrawer class is displaying the items on the wheel. It handles the initialization, layout, and visual representation of the wheel items and their slots.
### Inherits From

    MonoBehaviour

### Serialized Fields
```C#
    [Range(0.2f, 2f)][SerializeField] private float _wheelSize = 1f;
        // The size of the wheel, adjustable in the range from 0.2 to 2.

    [SerializeField] private Transform _wheelTransform;
        // The transform of the wheel.

    [SerializeField] private GameObject _wheelItemPrefab;
        // The prefab used for individual wheel items.

    [SerializeField] private Transform _wheelItemsParent;
        // The parent transform where the wheel items will be instantiated.

    [SerializeField] private GameObject _linePrefab;
        // The prefab used for the lines between wheel items.

    [SerializeField] private Transform _linesParent;
        // The parent transform where the lines will be instantiated.
```

### Private Fields
```C#
    private readonly Vector2 _itemMinSize = new Vector2(81f, 146f);
        // The minimum size for the wheel items.

    private readonly Vector2 _itemMaxSize = new Vector2(144f, 213f);
        // The maximum size for the wheel items.

    private float _itemAngle;
        // The angle between each wheel item.

    private float _halfItemAngle;
        // Half of the angle between each wheel item, used for positioning.

    private IReadOnlyList<WheelItem> _items;
        // The list of items to be displayed on the wheel.

    private List<WheelItemSlot> _wheelItemSlotList;
        // The list of item slots on the wheel.

    private readonly int _minItemsCount = 2;
        // The minimum number of items allowed on the wheel.

    private readonly int _maxItemsCount = 12;
        // The maximum number of items allowed on the wheel.
```

### Initialize

> Initializes the wheel with a given list of items. Sets up the item angles, sizes, and prepares the item slots.

```C#
public void Initialize(IReadOnlyList<WheelItem> items)
```

    Parameters:
        items: The list of items to be displayed on the wheel.

### DrawWheelItems

> Draws all the items on the wheel by setting each item in its corresponding slot.
```C#
public void DrawWheelItems()
```

### InstantiateAndInitializeItemSlots

> Instantiates and initializes item slots for each item on the wheel. Sets up the item transforms, images, and labels.

```C#
private void InstantiateAndInitializeItemSlots()
```

### DrawItem

> Draws a single item at the specified index.

```C#
private void DrawItem(int index)
```

    Parameters:
        index: The index of the item to be drawn.

### DrawLine

> Draws a line between wheel items.

```C#
private void DrawLine(int itemIndex)
```

    Parameters:
        itemIndex: The index of the item for which the line is being drawn.

### InstantiateItem

> Instantiates a new item prefab and returns the instantiated GameObject.

```csharp
private GameObject InstantiateItem()
```

    Returns: The instantiated item GameObject.

### OnValidate

> Called when the script is loaded or a value is changed in the Inspector. Adjusts the wheel transform scale based on _wheelSize.

```csharp
private void OnValidate()
```

### Inner Class: WheelItemSlot
### Overview

> Represents a slot for a wheel item, containing its transform, image, and label text.
<br>
> **Used for non glitched item replacement.**

### Fields
```C#
    public Transform ItemTransform;
        The transform of the item.

    public Image ItemImage;
        The image component of the item.

    public TMP_Text LabelText;
        The text component for the item's label.
```

### Method: SetItem

> Sets the item data for the slot.

```csharp
public void SetItem(IReadOnlyWheelItem wheelItem)
```

    Parameters:
        wheelItem: The item to be displayed in the slot.

</details>

<details open>
<summary><h2>WheelItemsPackEditor Class</h2></summary>

## WheelItemsPackEditor Class
### Overview

    The WheelItemsPackEditor class is a custom editor for the WheelItemsPack scriptable object in the "Wheel of Luck" game. It provides additional functionality in the Unity Inspector for managing the WheelItemsPack objects.
## Inherits From

    UnityEditor.Editor

## Methods
### OnInspectorGUI

>Overrides the default inspector GUI to add a custom button for resetting collected items.

```C#
public override void OnInspectorGUI()
```

## Custom Inspector Button

### Reset Collected Items
    Adds a button to the inspector that, when clicked, resets the collected items in the WheelItemsPack.

```C#
if (GUILayout.Button("Reset Collected Items"))
{
WheelItemsPack itemsPack = target as WheelItemsPack;

    itemsPack?.Reset();
}
```

## Usage Example

    To use the WheelItemsPackEditor class, ensure it is placed in an Editor folder within your Unity project. This will automatically enhance the inspector for any WheelItemsPack objects.

```C#
[CreateAssetMenu(fileName = "NewWheelItemsPack", menuName = "WheelOfLuck/NewItemsPack")]
public class NewWheelItemsPack : WheelItemsPack
{
// Custom implementation for a new items pack if needed.
}
```

## In the Unity Editor:

    1. Create a new WheelItemsPack scriptable object via the Assets menu.
    2. Select the WheelItemsPack object in the Project window.
    3. In the Inspector window, you will see the "Reset Collected Items" button added by the custom editor.

</details>

<details open>
<summary><h2>Wheel Of Luck Class</h2></summary>

## WheelOfLuck Class
### Overview

    The WheelOfLuck class manages the main functionalities of the "Wheel of Luck" game. It handles the initialization, spinning logic, and updating the visual representation of the wheel.

### Inherits From
    MonoBehaviour

## Serialized Fields

```C#
[SerializeField] private WheelItemsPack _itemsPack;
// The scriptable object containing the pack of wheel items.

[SerializeField] private WheelDrawer _drawer;
// The drawer responsible for displaying the items on the wheel.

[SerializeField] private SpinController _spinController;
// The controller managing the spin logic.

[SerializeField] private BaseSpinCostProvider _spinCostProvider;
// The provider handling the cost logic for spinning the wheel.

[SerializeField] private Button _spinButton;
// The button used to initiate a spin.
```

## Events

```C#
public event Action SpinStarted;
// Event triggered when the spin starts.

public event Action<WheelItem> SpinEnded;
// Event triggered when the spin ends, providing the item landed on.
```

## Properties

```C#
public bool IsSpinning { get; private set; }
// Indicates whether the wheel is currently spinning.

private IReadOnlyList<WheelItem> Items => _itemsPack.Items;
// Gets the list of items from the items pack.
```

## Methods

### Awake
> Initializes the wheel drawer, spin controller, and event subscriptions.

```C#
private void Awake()
```

### Spin

>Initiates a spin if the wheel is not already spinning.

```C#
public void Spin()
```

### SpinStartEventAction
>Sets the IsSpinning flag to true and triggers the SpinStarted event.

```C#
private void SpinStartEventAction()
```

### SpinEndEventAction
> Sets the IsSpinning flag to false, triggers the SpinEnded event with the landed item, replaces the item, and redraws the wheel.

```C#
private void SpinEndEventAction(WheelItem item)
```

### OnSpinAvailableUpdated
>Updates the interactable state of the spin button based on the availability status.

```C#
private void OnSpinAvailableUpdated(bool available)
```

### OnDestroy
>Unsubscribes from events to prevent memory leaks.

```C#
private void OnDestroy()
```

### Usage Example

    To use the WheelOfLuck class, attach it to a GameObject in your Unity scene and assign the necessary serialized fields in the Inspector.

```C#
public class GameManager : MonoBehaviour
{
[SerializeField] private WheelOfLuck _wheelOfLuck;

    private void Start()
    {
        _wheelOfLuck.SpinStarted += OnSpinStarted;
        _wheelOfLuck.SpinEnded += OnSpinEnded;
    }

    private void OnSpinStarted()
    {
        Debug.Log("Spin started!");
    }

    private void OnSpinEnded(WheelItem item)
    {
        Debug.Log("Spin ended! Landed on item: " + item.Label);
    }
}
```

## In the Unity Editor:

    1. Create a new GameObject and attach the WheelOfLuck component to it.
    2. Assign the WheelItemsPack, WheelDrawer, SpinController, BaseSpinCostProvider, and Button references in the Inspector.
    3. Optionally, create another GameObject and attach a script to handle SpinStarted and SpinEnded events.

</details>