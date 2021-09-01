using FredericRP.EventManagement;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
  [Header("Links")]
  [SerializeField]
  RectTransform flowerParent;
  [Header("Spawn config")]
  [SerializeField]
  Flower flowerPrefab;
  [SerializeField]
  Vector2 minTargetPosition;
  [SerializeField]
  Vector2 maxTargetPosition;
  [SerializeField]
  Vector2 maxRadius;
  [SerializeField]
  int startBurst = 20;
  [SerializeField]
  float intervalBetwenSpawn = 3;
  [SerializeField]
  [Tooltip("More than 1, flowers count will grow, less than 1, they will disappear")]
  float lifeRatio = 1.2f;
  [Header("Events")]
  [SerializeField]
  GameEvent flowerGrabEvent;

  public float IntervalBetwenSpawn { get { return intervalBetwenSpawn; } }

  float nextSpawn;

  // Start is called before the first frame update
  void Start()
  {
    for (int i = 0; i < startBurst; i++)
      SpawnFlower((i+1) * intervalBetwenSpawn * lifeRatio);

    nextSpawn = intervalBetwenSpawn;
  }

  private void OnEnable()
  {
    flowerGrabEvent.Listen<int>(FlowerGrabbed);
  }

  private void OnDisable()
  {
    flowerGrabEvent.Delete<int>(FlowerGrabbed);
  }

  void FlowerGrabbed(int id)
  {
    // Respawn a new flower when grabbing one
    SpawnFlower(intervalBetwenSpawn * lifeRatio * startBurst);
  }

  private void Update()
  {
    if (Time.time > nextSpawn)
    {
      nextSpawn = Time.time + intervalBetwenSpawn;
      SpawnFlower(intervalBetwenSpawn * lifeRatio * startBurst);
    }
  }

  void SpawnFlower(float lifeDuration)
  {
    Flower flower = Instantiate<Flower>(flowerPrefab, flowerParent);
    Vector2 targetPosition = Random.insideUnitCircle;
    targetPosition.Scale(maxRadius);
    //targetPosition.x = Mathf.Lerp(minTargetPosition.x, maxTargetPosition.x, Random.Range(0, 1f));
    //targetPosition.y = Mathf.Lerp(minTargetPosition.y, maxTargetPosition.y, Random.Range(0, 1f));
    // spawn on the floor
    flower.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;// Vector2.right * Random.Range(30, 1890);
    // set color and target position
    flower.SetFlower(Random.Range(0, 40) > 25 ? 1 : 2, targetPosition, lifeDuration);
  }
}
