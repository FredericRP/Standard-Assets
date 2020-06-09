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
  Bucket bucket;

  // Start is called before the first frame update
  void Start()
  {
    bucket = new Bucket(max);
    nextUpdateTime = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time > nextUpdateTime)
    {
      nextUpdateTime = Time.time + 1;
      randomText = "Random number (0," + max + ") is " + bucket.GetRandomNumber();
      Debug.Log(randomText);
      text.text = randomText;
    }
  }
}
