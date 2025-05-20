using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Webcam : MonoBehaviour
{
    WebCamTexture webcam;
    public RawImage image;
    public RenderTexture renderTexture;

    private void Start()
    {
        webcam = new WebCamTexture(224, 224); // taille de l’image pour le modèle
     
      
        image.texture = webcam;
        webcam.Play();
    }

    private void Update()
    {
        if (webcam.didUpdateThisFrame)
        {
            Graphics.Blit(webcam, renderTexture);
        }
    }
  
}
