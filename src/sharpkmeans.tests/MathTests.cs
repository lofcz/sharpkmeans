namespace sharpkmeans.tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGeometricMean()
    {
        float[][] points = {
            new [] { 0f, 0f },
            new [] { 10f, 10f }
        };
        
        float[] gm = Math2.GeometricMean(points);
        Assert.That(Utils.EqualsWithinTolerance(gm, new [] { 5f, 5f }));
    }
    
    [Test]
    public void TestGeometricMean2()
    {
        float[][] points = {
            new [] { 0f, 0f },
            new [] { 1f, 0f },
            new [] { 2f, 3f }
        };
        
        float[] gm = Math2.GeometricMean(points);
        Assert.That(Utils.EqualsWithinTolerance(gm, new [] { 1f, 1f }));
    }
    
    [Test]
    public void TestGeometricMean3()
    {
        float[][] points = {
            new [] { 0f, 0f, 1f },
            new [] { 1f, 0f, 0f },
            new [] { 2f, 3f, 8f },
            new [] { 4f, 4f, 2f }
        };
        
        float[] gm = Math2.GeometricMean(points);
        Assert.That(Utils.EqualsWithinTolerance(gm, new [] { 1.75f, 1.75f, 2.75f }));
    }
}