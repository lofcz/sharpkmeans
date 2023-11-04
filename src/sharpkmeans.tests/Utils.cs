namespace sharpkmeans.tests;

public static class Utils
{
    private const float Tolerance = 0.1e-8f;

    public static bool EqualsWithinTolerance(float a, float b)
    {
        return a switch
        {
            float.NaN => float.IsNaN(b),
            float.NegativeInfinity => float.IsNegativeInfinity(b),
            float.PositiveInfinity => float.IsPositiveInfinity(b),
            _ => Math.Abs(a - b) < Tolerance
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