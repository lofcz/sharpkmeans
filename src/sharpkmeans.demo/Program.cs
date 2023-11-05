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
        KMeansResultSilhouette[] result = KMeans.Evaluate(5, 15, data.Select(x => new[] { x.X, x.Y }));
        
        string json = JsonConvert.SerializeObject(result[0].Result);
        await File.WriteAllTextAsync("methods_tensors_reduced_silhoutte.json", json);
        Console.WriteLine(json);
    }
}