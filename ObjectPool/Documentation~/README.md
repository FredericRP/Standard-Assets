# Object Pool & Inspector

Use ObjectPool to instantiate game objects during loading time and not during gameplay.
This version allows you to have multiple pools in your game.

![object pool inspector](Documentation~/images/object_pool.png)

## Usage

1. Put the ObjectPool script in a game object that won't be destroyed during your game, and add the prefabs you want in the list in the inspector. Set the buffered amount, the default parent *(optional)*, and you're set.
2. To retrieve an already instantiated game object and use it in your game, use
```C#
ObjectPool.GetObjectPool("mypool").GetFromPool("myPrefabName")
```

3. To pool and deactivate a previously instantiated and retrieved game object, use
```C#
ObjectPool.GetObjectPool("mypool").Pool(usedObject)
```

> Attach DontDestroyOnLoad script on the same Game Object