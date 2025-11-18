using UnityEngine;
using Accord.Statistics.Analysis;
using System.Linq;

public class PCAUtils
{
    public static (Vector2 direction, float strength) ComputerPCAFromVectors(Vector2[] points)
    {
        // Convert Vector2 data points to array
        double[][] data = points.Select(point => new double[] {point.x, point.y}).ToArray();

        // Create the pca class
        PrincipalComponentAnalysis pca = new PrincipalComponentAnalysis()
        {
            Method = PrincipalComponentMethod.Center,
            NumberOfInputs = 1
        };

        // Compute the PCA
        pca.Learn(data);

        // Extract the values
        double[][] components = pca.ComponentVectors;
        double[] pc1 = components[0];

        Vector2 direction = new Vector2(
            (float) pc1[0],
            (float) pc1[1]
        ).normalized;

        double[] eigenValues = pca.Eigenvalues;
        float strength = (float)eigenValues[0];

        return (direction, strength);
    }
}
