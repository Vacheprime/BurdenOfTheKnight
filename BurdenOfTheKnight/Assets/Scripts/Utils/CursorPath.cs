using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CursorPath
{
    private readonly int cursorDataPointLimit;
    private readonly LinkedList<(Vector2 dataPoint, double timeStamp)> cursorDataPoints;

    public CursorPath(int cursorDataPointLimit = 5)
    {
        this.cursorDataPointLimit = cursorDataPointLimit;
        cursorDataPoints = new LinkedList<(Vector2 dataPoint, double timeStamp)>();
    }

    public void AddCursorCoordinate(Vector2 coord, double timeStamp)
    {
        // Remove last
        if (cursorDataPoints.Count == cursorDataPointLimit)
        {
            cursorDataPoints.RemoveLast();
        }

        // Add first
        cursorDataPoints.AddFirst((dataPoint: coord, timeStamp));
    }

    public bool IsFilled()
    {
        return cursorDataPoints.Count == cursorDataPointLimit;
    }

    public double GetAvgMouseVelocity()
    {
        // Check if at least two points exist
        if (cursorDataPoints.Count < 2)
        {
            return 0f;
        }

        List<double> velocities = new List<double>();

        // Loop over every data point and compute velocity with neighbour
        for (int i = 0; i < cursorDataPoints.Count - 1; i++)
        {

            // Get current point
            (Vector2 dataPoint, double timeStamp) recentPoint = cursorDataPoints.ElementAt(i);

            // Get next point (previous because it happened before)
            (Vector2 dataPoint, double timeStamp) previousPoint = cursorDataPoints.ElementAt(i + 1);

            // Calc total time
            double deltaTime = recentPoint.timeStamp - previousPoint.timeStamp;

            // Calc distance travelled
            double distance = Vector2.Distance(recentPoint.dataPoint, previousPoint.dataPoint);

            // Add velocity
            velocities.Add(distance / deltaTime);
        }

        // Return average velocity of all points
        return velocities.Sum() / velocities.Count;
    }

    public double GetDistanceTravelled()
    {
        double totalDistance = 0f;
        for (int i = 1; i < cursorDataPoints.Count; i++)
        {
            totalDistance += Vector2.Distance(
                cursorDataPoints.ElementAt(i - 1).dataPoint,
                cursorDataPoints.ElementAt(i).dataPoint);
        }
        return totalDistance;
    }

    public void ClearCursorPoints()
    {
        cursorDataPoints.Clear();
    }

    public Vector2 GetLastCursorPos()
    {
        return cursorDataPoints.First.Value.dataPoint;
    }

    public (string direction, float strength) GetSwipeData()
    {
        // Compute PCA of cursor data points
        (Vector2 direction, float strength) cursorPathData = PCAUtils.ComputerPCAFromVectors(cursorDataPoints.Select(point => point.dataPoint).ToArray());

        Vector2 direction = cursorPathData.direction;
        float strength = cursorPathData.strength;

        // Get movement direction
        Vector2 movementDirection = (cursorDataPoints.Last.Value.dataPoint - cursorDataPoints.First.Value.dataPoint).normalized;
        Debug.Log(cursorPathData);
        // If horizontal movement is greater
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return (movementDirection.x > 0 ? "RIGHT SWIPE" : "LEFT SWIPE", strength);
        }
        else
        {
            return (movementDirection.y > 0 ? "DOWN SWIPE" : "UP SWIPE", strength);
        }
    }

    public override string ToString()
    {
        return $"CursorPath(AvgVel: {GetAvgMouseVelocity()}, TotalDist: {GetDistanceTravelled()})";
    }
}
