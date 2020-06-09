using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FredericRP.Tips
{
  public class DisplayTips : MonoBehaviour
  {
    [SerializeField]
    float delayBetweenTips = 2.5f;
    [SerializeField]
    TextMeshProUGUI text = null;

    float nextTip;
    // Start is called before the first frame update
    void Start()
    {
      nextTip = 0;
    }

    // Update is called once per frame
    void Update()
    {
      if (Time.time > nextTip)
      {
        nextTip = Time.time + delayBetweenTips;
        DisplayNextTip();
      }
    }

    void DisplayNextTip()
    {
      text.text = TipsLoader.Instance.GetTip();
    }
  }

}