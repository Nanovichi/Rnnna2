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
        Debug.Log("Model V3 input shape: [" + string.Join(", ", modelV3.inputs[0].shape) + "]");
    }

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

        Color[] pixels = tex.GetPixels();
        float[] image = new float[224 * 224 * 3];

        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                int i = y * 224 + x;
                Color pixel = pixels[i];

                // NHWC: index = (y * width + x) * channels + c
                int baseIndex = i * 3;
                image[baseIndex + 0] = (pixel.r - 0.485f) / 0.229f;
                image[baseIndex + 1] = (pixel.g - 0.456f) / 0.224f;
                image[baseIndex + 2] = (pixel.b - 0.406f) / 0.225f;
            }
        }

        UnityEngine.Object.Destroy(tex); // Dispose texture to avoid memory leak
        return new Tensor(1, 224, 224, 3, image);
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
