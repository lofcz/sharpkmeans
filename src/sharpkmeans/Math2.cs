namespace sharpkmeans;

public static class Math2
{
    public static float[] GeometricMean(float[][] points)
    {
        float[] result = new float[points[0].Length];

        foreach (float[] t in points)
        {
            for (int j = 0; j < t.Length; ++j)
            {
                result[j] += t[j] / points.Length;
            }
        }

        return result;
    }

    public static float SquaredEuclideanDistance(float[] a, float[] b)
    {
        float[] diffs = new float[a.Length];
        
        for (int i = 0; i < a.Length; i++)
        {
            diffs[i] = a[i] - b[i];
        }

        return diffs.Aggregate(0f, (f, f1) => f + f1 * f1);
    }
}