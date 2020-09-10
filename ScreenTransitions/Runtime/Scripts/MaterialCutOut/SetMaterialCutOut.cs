using UnityEngine;
using UnityEngine.UI;

namespace FredericRP.ScreenTransitions
{
  public class SetMaterialCutOut : MonoBehaviour
  {
    public float cutOut;

    Image _image;

    private void Start()
    {
      _image = GetComponent<Image>();
    }

    private void Update()
    {
      _image.material.SetFloat("_Cutoff", cutOut);
    }
  }
}