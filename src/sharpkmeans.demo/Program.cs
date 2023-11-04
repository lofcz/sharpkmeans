using Newtonsoft.Json;
namespace sharpkmeans.demo;

public class StringEmbeddingPairReduced
{
    public string Text { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        List<StringEmbeddingPairReduced> data = JsonConvert.DeserializeObject<List<StringEmbeddingPairReduced>>(await File.ReadAllTextAsync("methods_tensors_reduced.json"))!;
        KMeansResult result = KMeans.Evaluate(10, data.Select(x => new[] { x.X, x.Y }));

        string json = JsonConvert.SerializeObject(result);
        Console.WriteLine(json);
    }
}