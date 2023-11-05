namespace sharpkmeans;

public static class Math2
{
    public const float Sqrt2Pi = 2.506628274631f;
    public const float TwoOverSqrtPi = 1.12837916709551f;
    
    public static float SquaredEuclideanDistance(float[] a, float[] b)
    {
        float[] diffs = new float[a.Length];

        for (int i = 0; i < a.Length; i++)
        {
            diffs[i] = a[i] - b[i];
        }

        return diffs.Aggregate(0f, (f, f1) => f + f1 * f1);
    }

    public static float DotProduct(float[] a, float[] b)
    {
        return a.Zip(b, (d1, d2) => d1 * d2).Sum();
    }

    public static float[] Substract(float[] a, float[] b)
    {
        float[] result = new float[a.Length];

        for (int i = 0; i < a.Length; ++i)
        {
            result[i] = a[i] - b[i];
        }

        return result;
    }

    public static float[] Divide(float[] a, float b)
    {
        float[] result = new float[a.Length];

        for (int i = 0; i < a.Length; ++i)
        {
            result[i] = a[i] / b;
        }

        return result;
    }

    public static float Mean(float[] points)
    {
        return points.Sum() / points.Length;
    }
    
    public static float[] Mean(List<float[]> points)
    {
        float[] result = new float[points[0].Length];
        
        for (int i = 0; i < points[0].Length; ++i)
        {
            result[i] = points.Sum(t => t[i]) / points.Count;
        }

        return result;
    }

    public static float Difference(float[][] a, float[][] b)
    {
        float diff = 0;
        
        for (int i = 0; i < a.Length; ++i)
        {
            for (int j = 0; j < a[0].Length; ++j)
            {
                diff += Math.Abs(a[i][j] - b[i][j]);
            }
        }

        return diff;
    }

    public static float StandardDeviation(float[] data, int deltaDegreesOfFreedom = 0)
    {
        float avg = data.Average();
        float accu = data.Select(x => x - avg).Select(z => z * z / (data.Length - deltaDegreesOfFreedom)).Sum();
        return (float)Math.Sqrt(accu);
    }

    public static float NormalCdf(float x, float u, float variance)
    {
        return (float)(1 / (variance * Sqrt2Pi) * Math.Exp(-(Math.Pow(x - u, 2) / (2 * Math.Pow(variance, 2)))));
    }
    
    public static float NormalErf(float x)
    {
        float oX = x;
        
        for (int n = 1; n < 100; ++n) 
        {
            float d = 2 * n + 1;
            float p = (float)(Math.Pow(oX, d) / (Factorial(n) * d));
            x += n % 2 == 1 ? -p : p;
        }

        return TwoOverSqrtPi * x;    
    }
    
    public static float[] Normalize(float[] data)
    {
        float[] result = new float[data.Length];
        float mean = Mean(data);
        float deviation = StandardDeviation(data, 1);

        for (int i = 0; i < data.Length; ++i)
        {
            result[i] = (data[i] - mean) / deviation;
        }

        return result;
    }
    
    public static int Factorial(int n)
    {
        if (n is 0 or 1) {
            return 1;
        }

        int r = 1;
        
        for (int i = 1; i <= n; ++i) {
            r *= i;
        }

        return r;
    }
    
    public static void TestAndersonDarling(float[] data)
    {
        data = data.Order().ToArray();
        float mean = Mean(data);
        float std = StandardDeviation(data, 1);
        float[] w = new float[data.Length];

        for (int i = 0; i < w.Length; ++i)
        {
            w[i] = (data[i] - mean) / std;
        }

        int z = 0;
    }
}