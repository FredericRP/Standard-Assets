using UnityEngine;
using UnityEngine.UI;

public class SetMaterialCutOut : MonoBehaviour
{
    public float cutOut;

    Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        // Instantiate new material to update values
        //Material mat = Instantiate(_image.material);
        //mat.SetFloat("_Cutoff", 0.4f);
        //_image.material = mat;
    }

    private void Update()
    {
        _image.material.SetFloat("_Cutoff", cutOut);
    }
}
