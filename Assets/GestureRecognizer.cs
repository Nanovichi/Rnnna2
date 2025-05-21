using Unity.Barracuda;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    public NNModel modelV2Asset;
    public NNModel modelV3Asset;

    private IWorker workerV2;
    private IWorker workerV3;

    void Start()
    {
        var modelV2 = ModelLoader.Load(modelV2Asset);
        var modelV3 = ModelLoader.Load(modelV3Asset);

        workerV2 = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, modelV2);
        workerV3 = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, modelV3);

Debug.Log("Model V2 input shape: [" + string.Join(", ", modelV2.inputs[0].shape) + "]");
Debug.Log("Model V3 input shape: [" + string.Join(", ", modelV3.inputs[0].shape) + "]");    }

    public int PredictGesture(Tensor input)
    {
        // Run both models
        workerV2.Execute(input);
        workerV3.Execute(input);

        Tensor outputV2 = workerV2.PeekOutput();
        Tensor outputV3 = workerV3.PeekOutput();

        // Combine predictions (average)
        Tensor combined = new Tensor(1, outputV2.length);
        for (int i = 0; i < outputV2.length; i++)
        {
            combined[0, i] = 0.7f * outputV2[0, i] + 0.3f * outputV3[0, i];
        }

        // Get predicted class
        int predictedClass = ArgMax(combined);

        // Dispose tensors to avoid memory leak
        input.Dispose();
        outputV2.Dispose();
        outputV3.Dispose();
        combined.Dispose();

        return predictedClass;
    }


    public Tensor PreprocessTexture(RenderTexture sourceTexture)
 {
     Texture2D tex = new Texture2D(224, 224, TextureFormat.RGB24, false);
     RenderTexture.active = sourceTexture;
     tex.ReadPixels(new Rect(0, 0, 224, 224), 0, 0);
     tex.Apply();
     RenderTexture.active = null;

     float[] image = new float[224 * 224 * 3];
     UnityEngine.Color[] pixels = tex.GetPixels();

     for (int y = 0; y < 224; y++)
     {
         for (int x = 0; x < 224; x++)
         {
             int pixelIndex = y * 224 + x;
             UnityEngine.Color pixel = pixels[pixelIndex];

             // Convert to CHW layout
             image[0 * 224 * 224 + pixelIndex] = (pixel.r - 0.485f) / 0.229f;
             image[1 * 224 * 224 + pixelIndex] = (pixel.g - 0.456f) / 0.224f;
             image[2 * 224 * 224 + pixelIndex] = (pixel.b - 0.406f) / 0.225f;
         }
     }

     return new Tensor(1, 224, 224, 3, image);  // NCHW
 }

    int ArgMax(Tensor t)
    {
        int maxIndex = 0;
        float maxValue = t[0, 0];
        for (int i = 1; i < t.length; i++)
        {
            if (t[0, i] > maxValue)
            {
                maxValue = t[0, i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    void OnDestroy()
    {
        workerV2.Dispose();
        workerV3.Dispose();
    }
}
