using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour
{
    public float MaxReduction = 0.1f;
    public float MaxIncrease = 0.1f;
    public float RateDamping = 0.1f;
    public float Strength = 300;
    public bool StopFlickering;

    private Light light;
    private float intensity;
    private bool isFlickering;

    public void Start()
    {
        light = GetComponent<Light>();
        intensity = light.intensity;
        StartCoroutine(Flicker());
    }

    public void Update()
    {
        if (!StopFlickering && !isFlickering)
        {
            StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker()
    {
        isFlickering = true;
        while (!StopFlickering)
        {
            light.intensity = Mathf.Lerp(light.intensity, Random.Range(intensity - MaxReduction, intensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
        isFlickering = false;
    }
}