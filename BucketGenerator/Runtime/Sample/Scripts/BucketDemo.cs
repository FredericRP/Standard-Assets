using FredericRP.BucketGenerator;
using UnityEngine;
using UnityEngine.UI;

public class BucketDemo : MonoBehaviour
{
  [SerializeField]
  int max = 20;
  [SerializeField]
  Text text = null;

  float nextUpdateTime;
  string randomText;
  string numberSerie;
  Bucket bucket;
  int count;

  // Start is called before the first frame update
  void Start()
  {
    bucket = new Bucket(max);
    nextUpdateTime = 0;
    numberSerie = null;
    count = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time > nextUpdateTime)
    {
      nextUpdateTime = Time.time + 1;
      if (count++ == max)
      {
        count = 0;
        numberSerie = null;
      }
      int number = bucket.GetRandomNumber();
      if (numberSerie == null)
        numberSerie = number.ToString();
      else
        numberSerie += ", "+ number;
      randomText = "Random number (0," + max + ") is " + number + "\n" + numberSerie;
      Debug.Log(randomText);
      text.text = randomText;
    }
  }
}
