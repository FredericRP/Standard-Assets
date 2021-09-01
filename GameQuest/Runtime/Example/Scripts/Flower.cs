using FredericRP.EventManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Flower : MonoBehaviour, IPointerDownHandler
{
  [Header("Links")]
  [SerializeField]
  Image coloredSprite;
  [SerializeField]
  RectTransform[] flowers;
  [Header("Sources")]
  [SerializeField]
  Sprite[] spriteList;
  [Header("Config")]
  [SerializeField]
  int maxRotation = 72;
  [SerializeField]
  float rotationSpeed = 3;
  [SerializeField]
  float moveSpeed = 3;
  [SerializeField]
  float moveThreshold = 2;
  [SerializeField]
  GameEvent grabFlowerEvent;

  public int id { get; private set; }

  RectTransform rectTransform;
  Vector3 targetRotation;
  Vector2 targetPosition;
  float timeToDie;

  private void Awake()
  {
    rectTransform = GetComponent<RectTransform>();
  }

  public void SetFlower(int id, Vector2 targetPosition, float lifeDuration)
  {
    this.id = id;
    coloredSprite.sprite = spriteList[id - 1]; // ID is 1 or 2, index is 0 or 1
    targetRotation = Vector3.forward * Random.Range(0, maxRotation);
    this.targetPosition = targetPosition;
    // Ensure all flowers images have no rotation
    for (int i = 0; i < flowers.Length; i++)
    {
      flowers[i].localEulerAngles = Vector3.zero;
    }
    timeToDie = Time.time + lifeDuration;
  }

  private void Update()
  {
    if ((flowers[0].localEulerAngles - targetRotation).sqrMagnitude > moveThreshold)
    {
      for (int i = 0; i < flowers.Length; i++)
      {
        flowers[i].localEulerAngles = Vector3.Lerp(flowers[i].localEulerAngles, targetRotation, rotationSpeed * Time.deltaTime);
      }
    }
    if ((targetPosition- rectTransform.anchoredPosition).sqrMagnitude > moveThreshold)
    {
      rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
    }
    if (Time.time > timeToDie)
      Destroy(gameObject);
  }
  public void OnPointerDown(PointerEventData eventData)
  {
    grabFlowerEvent.Raise<int>(id);
    Destroy(gameObject);
  }
}
