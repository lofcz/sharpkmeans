![Nuget](https://img.shields.io/nuget/v/sharpkmeans)

# SharpKMeans
[K-Means++](https://en.wikipedia.org/wiki/K-means%2B%2B) implementation for the .NET platform, includes [Silhouette K-Estimator](https://en.wikipedia.org/wiki/Silhouette_(clustering)) and [Anderson-Darling](https://en.wikipedia.org/wiki/Anderson%E2%80%93Darling_test) statistical test.

## Getting Started

```
Install-Package sharpkmeans
```

Create a dataset of N `IEnumerable<float>` items where each item represents an embedding in M-dimensional space. For example, this could be ada-002 embeddings of answers to some semi-open-ended questions. In the case of large M, consider first reducing the dimensions via [UMAP](https://github.com/curiosity-ai/umap-sharp) as SharpKMeans uses Euclidean distance as its distance function which loses meaning in higher order dimensions fast.

SharpKMeans allows for the clusterization of the dataset if we expect it to have distinct groups. KMeans works best on spherical data, in the case of non-regular shapes, consider [DBSCAN](https://github.com/yusufuzun/dbscan).

There are two routines available for this:

- `Evaluate(int clustersMin, int clustersMax, IEnumerable<IEnumerable<float>> data)` if we don't know the exact K but we know a range in which K is.
- `Evaluate(int clusters, IEnumerable<IEnumerable<float>> data)` if we know the exact K.

Both routines are thread-safe and can take an optional argument with settings of type `KMeansSettings`. The settings available are:

- `Iterations` - increase the value if suboptimal clusters are found.
- `RequiredDifferenceBetweenIterations` - allows to skip slow convergence near the end and end the algorithm eagerly if the centroids shift only by a very small amount.

An example of usage:

```cs
float[][] data = {
    new [] { 0f, 0.2f, 6f },
    new [] { 2.0f, 4f, 1.2f }
    // more data, the data should have at least two dimensions
    // for anderson-darling check, each cluster needs at least 5 datapoints
};

KMeansResultSilhouette[] result = KMeans.Evaluate(3, 20, data));
KMeansResult bestResult = result[0].Result;
```

The output structure contains:
- `Clusters` - an array of clusters where each cluster is defined by its [medoid](https://en.wikipedia.org/wiki/Medoid)
- `Datapoints` - input datapoints assigned to the clusters
- `Convergence` - the convergence progression report

An example of plotted results via [ImageSharp](https://github.com/SixLabors/ImageSharp.Drawing), here K was inferred as 7:

![image](https://github.com/lofcz/sharpkmeans/assets/10260230/d80dd9eb-7e38-4917-968b-023aee6a9e7c)


## Acknowledgments

- [Aneta Kahleová](https://github.com/anetakahle) - for help with implementing the silhouette coefficient.
