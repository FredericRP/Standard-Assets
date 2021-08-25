# Persistent data system

The PersistentDataSystem is a fast and maintenable save system.
Data are written in binary files to hide the content from users and to enable fast loading and saving.

## Usage

- Put the script on a gameObject, it's a singleton and you can access to it from anywhere.
- Enter the class name on classToLoad array. This will load this class. Beware an inner class, it will need their two name seperate by a '+' (like : Game+GameData if GameData is declared in the class Game).
- Call LoadAllData or LoadData<T> to load a data after init (this will erase all data of this type and create new one).
- Call GetData<T> to obtain data. This will try to LoadData<T> if **multipleFile** is active and no data of this type is loaded yet. This return all time a data, it will create it from scratch if there is no one saved and call OnInit on this data.
- You can AddNewSavedData<T> if you want another data of this type in the saved data. Saved data allow you to have many data of a type as you want.
- To include some defaults information in a saved data just load default saved data in editor, edit your data and save as default data. It will create files in the streaming asset all included in your build.

## Features

SaveMode: use a single file to store all data or multiple file, one for each type of data.

Controled Serialization: you can implement ISerialization to control the serialization process.blabla
for controlling the serialization with a dictionnary or SavedData.IFullSerializationControl for controlling serialization very accuratly ! SavedData.IFullSerializationControl is better in performance but did not assure serialization algorithm for circular reference.

## Events

- OnDataSaved: a data is saved to file.
- OnDataLoaded: a data is loaded from a file.
