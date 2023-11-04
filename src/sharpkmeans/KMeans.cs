namespace sharpkmeans;

public class KMeansSettings
{
    public static readonly KMeansSettings Default = new KMeansSettings();
    
    public int Iterations { get; set; } = 50;
}

public static class KMeans
{
    public static float[][] InitializePlusPlus(int clusters, float[][] data, int? randomSeed = null)
    {
        int firstIndex = randomSeed ?? Random.Shared.Next(data.Length);
        int centroidsSolved = 1;
        float[][] centroids = Utils.CreateJaggedArray2D(clusters, data[0].Length);
        Array.Copy(data[firstIndex], centroids[0], centroids[0].Length);

        for (int i = 1; i < clusters; ++i)
        {
            float[] squaredDistances = new float[data.Length];

            for (int j = 0; j < data.Length; j++)
            {
                float[] distances = new float[centroidsSolved];
                float min = float.MaxValue;

                for (int k = 0; k < centroidsSolved; ++k)
                {
                    float[] diff = Math2.Substract(centroids[k], data[j]);
                    float distance = Math2.DotProduct(diff, diff);
                    distances[k] = distance;

                    if (distance < min)
                    {
                        min = distance;
                    }
                }

                squaredDistances[j] = min;
            }

            float[] probabilites = Math2.Divide(squaredDistances, squaredDistances.Sum());
            int index = Array.IndexOf(probabilites, probabilites.Max());

            if (index > 0)
            {
                Array.Copy(data[index], centroids[centroidsSolved], data[index].Length);
                centroidsSolved++;
            }
        }

        return centroids;
    }

    public static int[] ClosestCentroids(float[][] data, float[][] centroids)
    {
        int[] labels = new int[data.Length];

        for (int i = 0; i < data.Length; ++i)
        {
            int minIndex = -1;
            float minDistance = float.MaxValue;

            for (int j = 0; j < centroids.Length; ++j)
            {
                float distance = Math2.SquaredEuclideanDistance(data[i], centroids[j]);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = j;
                }
            }

            labels[i] = minIndex;
        }

        return labels;
    }
    
    public static void Evaluate(int clusters, float[][] data, KMeansSettings? settings = null)
    {
        settings ??= KMeansSettings.Default;
        float[][] centroids = InitializePlusPlus(clusters, data);
        float[] labels = new float[data.Length];

        for (int i = 0; i < settings.Iterations; ++i)
        {
            
        }
    }
}