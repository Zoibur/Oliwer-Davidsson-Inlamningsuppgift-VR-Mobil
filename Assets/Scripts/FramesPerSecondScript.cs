using System.Collections;
using TMPro;
using UnityEngine;

public class FramesPerSecondScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fpsText;

    [SerializeField]
    private float updateInterval = 1.0f;

    float accumulatedFrameTime;
    int framesSinceUpdate = 0;

    int[] frameRateSamples;
    int sampleIndex = 0;
    [SerializeField]
    int framesToAverage;

    // Start is called before the first frame update
    void Start()
    {
        frameRateSamples = new int[framesToAverage];
        StartCoroutine(UpdateFPSDisplay());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFPS();
    }

    void CalculateFPS() {
        float deltaTime = Time.unscaledDeltaTime;
        accumulatedFrameTime += deltaTime;
        framesSinceUpdate++;

        frameRateSamples[sampleIndex] = Mathf.RoundToInt(1.0f / deltaTime);
        sampleIndex = (sampleIndex + 1) % framesToAverage;
    }

    private IEnumerator UpdateFPSDisplay() { 
    
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            if (framesSinceUpdate == 0)
                continue;

            // Beräkningen av medelvärdet
            int sum = 0;
            for (int i = 0; i < framesToAverage; i++)
            {
                sum += frameRateSamples[i];
            }

            int averageFPS = sum / framesToAverage;

            string statsText = $"FPS: {averageFPS}";
            // Detta string statsText = "FPS: " + averageFPS; är samma som ovan

            fpsText.text = statsText;

            // Nollställ alla räknare och beräkningar
            accumulatedFrameTime = 0f;
            framesSinceUpdate = 0;



        }
    }
}
