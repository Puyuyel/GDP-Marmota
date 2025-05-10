using UnityEngine;
using UnityEngine.UI;

public class DualSliderController : MonoBehaviour
{
    public Slider sliderLeft;
    public Slider sliderRight;
    public float totalTime = 60f;

    private float elapsedTime = 0f;

    void Start()
    {
        sliderLeft.value = 1f;
        sliderRight.value = 1f;
    }

    void Update()
    {
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float ratio = Mathf.Clamp01(1f - elapsedTime / totalTime);

            sliderLeft.value = ratio;
            sliderRight.value = ratio;
        }
    }
}
