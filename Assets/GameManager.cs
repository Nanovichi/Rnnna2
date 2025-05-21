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

    private string currentGesture = "";

    void Update()
    {

        // Run model prediction only once every second
        timer += Time.deltaTime;
        if (timer >= gestureInterval)
        {
            timer = 0f;
            Tensor input = recognizer.PreprocessTexture(inputTexture);
            int gesture = recognizer.PredictGesture(input);
            currentGesture = gestureLabels[gesture];
            Debug.Log("Detected: " + currentGesture);
        }

        // Act based on the last known gesture, every frame
        switch (currentGesture)
        {
            case "Gesture_0":
                player.MoveForward();
                break;
            case "Gesture_2":
                player.MoveBackward();
                break;
            case "Gesture_5":
                player.MoveRight();
                break;
            case "Gesture_13":
                player.MoveLeft();
                break;
            case "Gesture_7":
                player.JumpForward();
                break;
            default:
                player.Idle();
                break;
        }

    }
}