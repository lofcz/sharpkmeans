namespace sharpkmeans.tests;

public static class Utils
{
    public const float Tolerance = 0.1e-8f;
    public const float ToleranceHigh = 0.1e-5f;

    public static bool EqualsWithinTolerance(float a, float b, float tolerance = Tolerance)
    {
        return a switch
        {
            float.NaN => float.IsNaN(b),
            float.NegativeInfinity => float.IsNegativeInfinity(b),
            float.PositiveInfinity => float.IsPositiveInfinity(b),
            _ => Math.Abs(a - b) < tolerance
        };
    }
    
    public static bool EqualsWithinTolerance(double a, double b, double tolerance = Tolerance)
    {
        return a switch
        {
            double.NaN => double.IsNaN(b),
            double.NegativeInfinity => double.IsNegativeInfinity(b),
            double.PositiveInfinity => double.IsPositiveInfinity(b),
            _ => Math.Abs(a - b) < tolerance
        };
    }
    
    public static bool EqualsWithinTolerance(float[] a, float[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        return !a.Where((t, i) => !EqualsWithinTolerance(t, b[i])).Any();
    }
}