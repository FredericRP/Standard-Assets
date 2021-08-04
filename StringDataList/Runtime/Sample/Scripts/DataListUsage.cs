using FredericRP.StringDataList;
using UnityEngine;

public class DataListUsage : MonoBehaviour
{

  [System.Serializable]
  class Animal
  {
    public int maximumAge;
    public int pawCount;
    [Select("animal")]
    public string bestFriend;
  }

  [System.Serializable]
  class AnimalList : DataList<Animal> { }

  [SerializeField]
  [Select("animal")]
  string favoriteAnimal;

  [SerializeField]
  [DataList("animal")]
  AnimalList animalCharacteristics;
}
