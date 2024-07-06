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
- ICollectable
- IReadOnlyWheelItem
### Abstract Classes 
- BaseSpinCostProvider
- WheelItem : IReadOnlyWheelItem, ICollectable
### Models
- WheelItemsPack
### Controllers
- SpinController
### View
- WheelDrawer
### Editor
- WheelItemsPackEditor
### Main Class
- WheelOfLuck

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
<details>
<summary style="">Methods</summary>
</details>

## Methods

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
