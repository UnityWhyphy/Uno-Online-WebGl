using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f; // Interval at which to update FPS display
    public Text fpsText; // Reference to the UI Text component to display FPS

    public static FPSCounter instance;

    private float accum = 0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeLeft; // Time left for current interval

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
    }

    public Text dateTimeText;


    void Start()
    {
        timeLeft = updateInterval;
    }

    void Update()
    {

        DateTime currentTime = DateTime.Now;

        string formattedDateTime = currentTime.ToString("HH:mm:ss");

        dateTimeText.text = formattedDateTime;

        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeLeft <= 0.0)
        {
            float fps = accum / frames;

            if (fpsText != null)
            {
                fpsText.text = fps.ToString("F0");
            }

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
