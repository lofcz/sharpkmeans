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
    
    [Test]
    public void TestSquaredEuclideanDistance()
    {
        float d = Math2.SquaredEuclideanDistance(new [] { 0f, 0f }, new [] { 4f, 4f});
        Assert.That(Utils.EqualsWithinTolerance(d, 32f));
    }
    
    [Test]
    public void TestSquaredEuclideanDistance2()
    {
        float d = Math2.SquaredEuclideanDistance(new [] { 0f, 0f, 9f, 100f }, new [] { 4f, 4f, 9f, 0f});
        Assert.That(Utils.EqualsWithinTolerance(d, 10032f));
    }
    
    [Test]
    public void TestSquaredEuclideanDistance3()
    {
        float d = Math2.SquaredEuclideanDistance(new [] { 0f, 0.2f, 0.1f, 0.05f }, new [] { 6.9f, 0, 2, 10});
        Assert.That(Utils.EqualsWithinTolerance(d, 150.2625f));
    }
    
    [Test]
    public void TestSubstract()
    {
        float[] a = { -4.6961695f, 7.2027797f };
        float[] b = { 6.16681858f, 1.71612863f };
        
        float[] d = Math2.Substract(a, b);
        Assert.That(Utils.EqualsWithinTolerance(d, new [] { -10.8629875f, 5.48665142f }));
    }
    
    [Test]
    public void TestDotProduct()
    {
        float[] a = { -4.6961695f, 7.2027797f };
        float[] b = { 6.16681858f, 1.71612863f };
        float[] substract = Math2.Substract(a, b);
        
        float d = Math2.DotProduct(substract, substract);
        Assert.That(Utils.EqualsWithinTolerance(d, 148.107849f));
    }
}