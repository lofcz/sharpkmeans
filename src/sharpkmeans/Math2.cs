using System.Numerics;
namespace sharpkmeans;

public static class Math2
{
    public const double TwoOverSqrtPi = 1.12837916709551d;
    
    private static readonly BigInteger[] factorialsTable = new BigInteger[101];

    static Math2()
    {
        for (int i = 0; i < factorialsTable.Length; ++i)
        {
            factorialsTable[i] = Factorial(i, false);
        }
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
    
    public static double NormalCdf(float x, float u = 0f, float variance = 1f)
    {
        double d = (x - u) / Math.Sqrt(2 * Math.Pow(variance, 2));
        double e = NormalErf(d) + 1;
        return e / 2f;
    }
    
    public static double NormalErf(double x)
    {
        double accu = x;
        
        for (int n = 1; n < 100; ++n) 
        {
            double d = 2 * n + 1;
            double p = Math.Pow(x, (int)d) / ((double)Factorial(n) * d);
            accu += n % 2 == 1 ? -p : p;
        }

        double t = accu * TwoOverSqrtPi;
        return t;
    }
    
    public static double NormalPValue(float[] data)
    {
        if (data.Length < 5)
        {
            return 0;
        }

        data = Normalize(data);
        data = data.Order().ToArray(); 
        double s = data.Select(t => NormalCdf(t)).Select((cdf, i) => (2 * i + 1) * Math.Log(cdf) + (2 * (data.Length - (i + 1)) + 1) * Math.Log(1 - cdf)).Sum();

        double a2 = -data.Length - 1 / (double)data.Length * s;
        a2 *= 1 + 0.75 / data.Length + 2.25 / Math.Pow(data.Length, 2);

        return a2 switch
        {
            >= 0.6f => Math.Exp(1.2937 - 5.709 * a2 + 0.0186 * Math.Pow(a2, 2)),
            > 0.34f => Math.Exp(0.9177 - 4.279 * a2 - 1.38 * Math.Pow(a2, 2)),
            > 0.2f => 1 - Math.Exp(-8.318 + 42.796 * a2 - 59.938 * Math.Pow(a2, 2)),
            _ => 1 - Math.Exp(-13.436 + 101.14 * a2 - 223.73 * Math.Pow(a2, 2))
        };
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
    
    public static BigInteger Factorial(int n, bool lookup = true)
    {
        if (n is 0 or 1) 
        {
            return 1;
        }

        if (lookup && n <= 100)
        {
            return factorialsTable[n];
        }

        BigInteger s = 1;
        
        for (int i = 1; i <= n; ++i) {
            s *= i;
        }
        
        return s;
    }
    
    public static bool TestAndersonDarling(float[] data, float criticality = 0.75f)
    {
        return NormalPValue(data) >= criticality;
    }
}