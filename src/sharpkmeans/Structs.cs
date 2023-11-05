namespace sharpkmeans;

public class KMeansSettings
{
    public static readonly KMeansSettings Default = new KMeansSettings();
    
    /// <summary>
    /// Number of iterations
    /// </summary>
    public int Iterations { get; set; } = 50;
    /// <summary>
    /// If the sum of centroids shift is lesser than this, the algorithm ends eagerly. Set to null to disable.
    /// </summary>
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

/// <summary>
/// Represents a cluster with assigned datapoints
/// </summary>
/// <param name="Position">Position in the N-Dimensional space</param>
/// <param name="Datapoints">Datapoints assigned to this cluster</param>
public record KMeansClusterWithDatapoints(float[] Position, List<KMeansDatapoint> Datapoints);

/// <summary>
/// Output of <see cref="KMeans.Evaluate"/>
/// </summary>
/// <param name="Result">Output of <see cref="KMeans.Evaluate"/></param>
/// <param name="SilhouetteCoefficient">Score of the run, the higher the better. This is in range of -1, 1.</param>
/// <param name="IterationIndex">Index of the run</param>
public record KMeansResultSilhouette(KMeansResult Result, float SilhouetteCoefficient, int IterationIndex);