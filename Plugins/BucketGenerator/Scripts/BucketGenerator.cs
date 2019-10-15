using UnityEngine;

namespace FredericRP.BucketGenerator
{
  public class Bucket
  {
    private int max;
    int[] randomList;
    int indexInList;
    bool resetWhenLooping = true;

    public Bucket(int max, bool resetWhenLooping = true)
    {
      SetMax(max);
      this.resetWhenLooping = resetWhenLooping;
    }

    public void SetMax(int max)
    {
      this.max = max;
      randomList = new int[max];
      ResetBucket();
    }

    public int GetRandomNumber()
    {
      if (randomList == null)
      {
        ResetBucket();
      }
      // Pick next one
      int result = randomList[indexInList++];
      // If end of list is reached, loop
      if (indexInList >= max)
      {
        indexInList = 0;
        if (resetWhenLooping)
          ResetBucket();
      }
      return result;
    }

    /// <summary>
    /// Generates a list containing numbers from 0 to max, in a shuffled order
    /// </summary>
    /// <param name="max">Max</param>
    public void ResetBucket()
    {
      indexInList = 0;

      // Fill the bucket with numbers
      for (int i = 0; i < max; i++)
      {
        randomList[i] = i;
      }
      // 2. Shuffle the list
      for (int i = max - 1; i > 0; i--)
      {
        int temp = randomList[i];
        int newIndex = Random.Range(0, i);
        randomList[i] = randomList[newIndex];
        randomList[newIndex] = temp;
      }
    }
  }
}
