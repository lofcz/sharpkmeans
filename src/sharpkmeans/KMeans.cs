namespace sharpkmeans;

public class KMeansSettings
{
    public static readonly KMeansSettings Default = new KMeansSettings();
    
    public int Iterations { get; set; } = 50;
    public float? RequiredDifferenceBetweenIterations { get; set; } = 0.1e-5f;
}

/// <summary>
/// Computed datapoint
/// </summary>
/// <param name="Index">Index in input collection</param>
/// <param name="Vector">Input vector</param>
/// <param name="Cluster">Index of output cluster</param>
public record KMeansDatapoint(int Index, float[] Vector, int Cluster);

/// <summary>
/// Computed cluster
/// </summary>
/// <param name="Position">Space in the input space</param>
public record KMeansCluster(float[] Position);

/// <summary>
/// Output of <see cref="KMeans.Evaluate"/>
/// </summary>
/// <param name="Clusters">Computed centroids</param>
/// <param name="Datapoints">Input data assigned to <see cref="Clusters"/></param>
/// <param name="Convergence">Convergence report</param>
public record KMeansResult(KMeansCluster[] Clusters, KMeansDatapoint[] Datapoints, List<float> Convergence);

public record KMeansClusterWithDatapoints(float[] Position, List<KMeansDatapoint> Datapoints);

public record KMeansResultSilhouette(KMeansResult Result, float SilhouetteCoefficient, int IterationInde);

public static class KMeans
{
    public static float[][] InitializePlusPlus(int clusters, float[][] data, int? randomSeed = null)
    {
        int firstIndex = randomSeed ?? Random.Shared.Next(data.Length);
        int centroidsSolved = 1;
        float[][] centroids = Utils.CreateJaggedArray2D(clusters, data[0].Length);
        Array.Copy(data[firstIndex], centroids[0], centroids[0].Length);

        for (int i = 1; i < clusters; ++i)
        {
            float[] squaredDistances = new float[data.Length];

            for (int j = 0; j < data.Length; j++)
            {
                float[] distances = new float[centroidsSolved];
                float min = float.MaxValue;

                for (int k = 0; k < centroidsSolved; ++k)
                {
                    float[] diff = Math2.Substract(centroids[k], data[j]);
                    float distance = Math2.DotProduct(diff, diff);
                    distances[k] = distance;

                    if (distance < min)
                    {
                        min = distance;
                    }
                }

                squaredDistances[j] = min;
            }

            float[] probabilites = Math2.Divide(squaredDistances, squaredDistances.Sum());
            int index = Array.IndexOf(probabilites, probabilites.Max());

            if (index > 0)
            {
                Array.Copy(data[index], centroids[centroidsSolved], data[index].Length);
                centroidsSolved++;
            }
        }

        return centroids;
    }

    public static int[] ClosestCentroids(float[][] data, float[][] centroids)
    {
        int[] labels = new int[data.Length];

        for (int i = 0; i < data.Length; ++i)
        {
            int minIndex = -1;
            float minDistance = float.MaxValue;

            for (int j = 0; j < centroids.Length; ++j)
            {
                float distance = Math2.SquaredEuclideanDistance(data[i], centroids[j]);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = j;
                }
            }

            labels[i] = minIndex;
        }

        return labels;
    }

    public static float[][] ComputeCentroids(float[][] data, int[] labels, int clusters)
    {
        float[][] centroids = Utils.CreateJaggedArray2D(clusters, data[0].Length);
        List<float[]> points = new();
        
        for (int i = 0; i < clusters; ++i)
        {
            points.Clear();    
            
            for (int j = 0; j < labels.Length; ++j)
            {
                if (labels[j] != i)
                {
                    continue;
                }

                points.Add(data[j]);
            }

            centroids[i] = Math2.Mean(points);
        }

        return centroids;
    }


    public static float MeanDistancePointToCluster(KMeansDatapoint point, KMeansClusterWithDatapoints cluster)
    {
        float sumOfDistances = 0;
        foreach (KMeansDatapoint pointInCluster in cluster.Datapoints)
        {
            if (pointInCluster == point)
            {
                continue;
            }
            
            float diff = Math2.SquaredEuclideanDistance(pointInCluster.Vector, point.Vector);
            sumOfDistances += diff;
        }
        
        float meanDistance = sumOfDistances / cluster.Datapoints.Count;
        return meanDistance;
    }

    public static KMeansClusterWithDatapoints NeighborCluster(KMeansDatapoint point, KMeansClusterWithDatapoints[] clusters)
    {
        KMeansClusterWithDatapoints bestCluster = clusters[0];
        float bestDist = float.MaxValue;
        
        foreach (KMeansClusterWithDatapoints t in clusters)
        {
            if (t.Datapoints.Contains(point))
            {
                continue;
            }

            float dist = Math2.SquaredEuclideanDistance(point.Vector, t.Position);

            if (dist < bestDist)
            {
                bestDist = dist;
                bestCluster = t;
            }
        }

        return bestCluster;
    }
    
    
    public static KMeansResultSilhouette[] Evaluate(int clustersMin, int clustersMax, IEnumerable<IEnumerable<float>> data, KMeansSettings? settings = null)
    {
        KMeansResultSilhouette[] results = new KMeansResultSilhouette[clustersMax - clustersMin];
        int idx = 0;
        IEnumerable<IEnumerable<float>> enumerable = data as IEnumerable<float>[] ?? data.ToArray();
        
        for (int k = clustersMin; k < clustersMax; k++)
        {
            // create k clusters using kmeans
            KMeansResult result = Evaluate(k, enumerable, settings);

            KMeansClusterWithDatapoints[] clusters = new KMeansClusterWithDatapoints[result.Clusters.Length];

            for (int i = 0; i < result.Clusters.Length; ++i)
            {
                clusters[i] = new KMeansClusterWithDatapoints(result.Clusters[i].Position, new List<KMeansDatapoint>());
            }
            
            foreach (KMeansDatapoint datapoint in result.Datapoints)
            {
                clusters[datapoint.Cluster].Datapoints.Add(datapoint);
            }
            
            float silhouetteAccu = 0;


            foreach (KMeansClusterWithDatapoints cluster in clusters)
            {
                float clusterAccu = 0;
                
                foreach (KMeansDatapoint point in cluster.Datapoints)
                {
                    float cohesion = MeanDistancePointToCluster(point, clusters[point.Cluster]);
                    float separation = MeanDistancePointToCluster(point, NeighborCluster(point, clusters));
                    float silhouetteCoefficient = (separation - cohesion) / Math.Max(separation, cohesion);
                    clusterAccu += silhouetteCoefficient;
                }

                clusterAccu /= cluster.Datapoints.Count;
                silhouetteAccu += clusterAccu;
            }

            silhouetteAccu /= clusters.Length;
            results[idx] = new KMeansResultSilhouette(result, silhouetteAccu, idx);
            idx++;
        }

        return results.OrderByDescending(x => x.SilhouetteCoefficient).ToArray();
    }

    public static KMeansResult Evaluate(int clusters, IEnumerable<IEnumerable<float>> data, KMeansSettings? settings = null)
    {
        float[][] enumeratedData = data.Select(x => x.ToArray()).ToArray();
        
        settings ??= KMeansSettings.Default;
        float[][] centroids = InitializePlusPlus(clusters, enumeratedData);
        List<float> differences = new();
        int[] labels = new int[enumeratedData.Length];
        
        for (int i = 0; i < settings.Iterations; ++i)
        {
            labels = ClosestCentroids(enumeratedData, centroids);
            float[][] nextCentroids = ComputeCentroids(enumeratedData, labels, clusters);
            float diff = Math2.Difference(centroids, nextCentroids);
            differences.Add(diff);
            
            if (settings.RequiredDifferenceBetweenIterations is not null)
            {
                if (diff < settings.RequiredDifferenceBetweenIterations.Value)
                {
                    break;
                }
            }

            centroids = nextCentroids;
        }

        KMeansCluster[] resultClusters = new KMeansCluster[clusters];

        for (int i = 0; i < clusters; ++i)
        {
            resultClusters[i] = new KMeansCluster(centroids[i]);
        }

        KMeansDatapoint[] resultDatapoints = new KMeansDatapoint[enumeratedData.Length];

        for (int i = 0; i < enumeratedData.Length; ++i)
        {
            resultDatapoints[i] = new KMeansDatapoint(i, enumeratedData[i], labels[i]);
        }
        
        return new KMeansResult(resultClusters, resultDatapoints, differences);
    }
}