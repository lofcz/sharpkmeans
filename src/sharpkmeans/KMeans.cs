namespace sharpkmeans;

public static class KMeans
{
    private static float[][] InitializePlusPlus(int clusters, float[][] data)
    {
        int firstIndex = Random.Shared.Next(clusters + 1);
        float[][] centroids = Utils.CreateJaggedArray2D(clusters, data.Length);
        
        for (int i = 1; i < clusters; ++i)
        {
            foreach (float[] point in data)
            {
                Math2.SquaredEuclideanDistance(centroids[i], point);
            }
        }

        return centroids;
    }
}