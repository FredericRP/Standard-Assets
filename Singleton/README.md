# Singleton

A singleton is a development pattern that prevents having multiple instances of the same component in a scene.
It allows also any script to access this one wihout requiring a manual link between the two.

## Description

A generic Singleton that allows you to inherit from it from your class to access its instance with the static Instance property.

## Usage

Extends the Singleton<T> class from your own classes like this :

```C#
public class MyBrandNewClass : Singleton<MyBrandNewClass> {
// ...
}
```

You can call it from other scripts like this :

```C#
MyBrandNewClass.Instance.PublicMethod()
```