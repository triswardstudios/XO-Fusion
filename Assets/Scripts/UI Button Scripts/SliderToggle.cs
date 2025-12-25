using UnityEngine;

public class SliderToggle : MonoBehaviour
{
    void OnClick()
    {
        if (gameObject.GetComponent<UnityEngine.UI.Slider>().value == 0)
        {
            gameObject.GetComponent<UnityEngine.UI.Slider>().value = 1;
        }
        else
        {
            gameObject.GetComponent<UnityEngine.UI.Slider>().value = 0;
        }
    }
}
