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
}