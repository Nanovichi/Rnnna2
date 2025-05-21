using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GestureRecognizer recognizer;
    public RenderTexture inputTexture;
    public PlayerController player;

    string[] gestureLabels = new string[]
{
    "Gesture_0", "Gesture_1", "Gesture_10", "Gesture_11",
    "Gesture_12", "Gesture_13", "Gesture_2", "Gesture_3",
    "Gesture_4", "Gesture_5", "Gesture_6", "Gesture_7",
    "Gesture_8", "Gesture_9"
};

    private float timer = 0f;
    private float gestureInterval = 1f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= gestureInterval)
        {
            timer = 0f;
            Tensor input = recognizer.PreprocessTexture(inputTexture);
            int gesture = recognizer.PredictGesture(input);
            string predictedLabel = gestureLabels[gesture];
            Debug.Log("Detected: " + predictedLabel);
            switch (predictedLabel)
            {
                case "Gesture_0":

                    break;
            }
        }
    }
}