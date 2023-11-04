namespace sharpkmeans;

public static class KMeans
{
    public static float[][] InitializePlusPlus(int clusters, float[][] data, int? randomSeed)
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
}