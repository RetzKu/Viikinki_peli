using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Vehtor2
{
    public float x, y;

    public Vehtor2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Job : ThreadedJob
{
    public List<Vector2> OutData; // arbitary job data
    public int Index;

    public Job(int index, int width, int height, float radius)
    {
        this.Index = index;

        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
                           Mathf.CeilToInt(height / cellSize)];
 
        rand = new System.Random(Thread.CurrentThread.ManagedThreadId);
    }

    protected override void ThreadFunction()
    {
        // PoissonDiscSampler sampler = new PoissonDiscSampler(20f, 20f, 2f);
        // OutData = sampler.GetSamples();
        OutData = GetSamples();
    }

    protected override void OnFinished()
    {
        // This is executed by the Unity main thread when the job is finished
        Debug.Log("job done");
    }

    private const int k = 20;  // Maximum number of attempts before marking a sample as inactive.

    private readonly Rect rect;
    private readonly float radius2;  // radius squared
    private readonly float cellSize;
    private Vector2[,] grid;
    private List<Vector2> activeSamples = new List<Vector2>();
    private System.Random rand; //  = new System.Random();

    public List<Vector2> GetSamples()
    {
        List<Vector2> solution = new List<Vector2>(200);

        // Random a = new Random();
        AddSample(new Vector2((float)rand.NextDouble() * rect.width, (float)rand.NextDouble() * rect.height));

        while (activeSamples.Count > 0)
        {

            // Pick a random active sample
            int i = (int)rand.NextDouble() * activeSamples.Count;

            // int i = 1;
            Vector2 sample = activeSamples[i];

            // Try `k` random candidates between [radius, 2 * radius] from that sample.
            bool found = false;
            for (int j = 0; j < k; ++j)
            {

                float angle = 2 * Mathf.PI * (float)rand.NextDouble();
                float r = Mathf.Sqrt((float)rand.NextDouble() * 3 * radius2 + radius2); // See: http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus/9048443#9048443
                Vector2 candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Accept candidates if it's inside the rect and farther than 2 * radius to any existing sample.
                if (rect.Contains(candidate) && IsFarEnough(candidate))
                {
                    found = true;
                    // yield return AddSample(candidate);
                    solution.Add(AddSample(candidate));
                    break;
                }
            }

            // If we couldn't find a valid candidate after k attempts, remove this sample from the active samples queue
            if (!found)
            {
                activeSamples[i] = activeSamples[activeSamples.Count - 1];
                activeSamples.RemoveAt(activeSamples.Count - 1);
            }
        }

        return solution;
    }

    private bool IsFarEnough(Vector2 sample)
    {
        Vec2 pos = CalculateGridPos(sample, cellSize);

        int xmin = Mathf.Max(pos.X - 2, 0);
        int ymin = Mathf.Max(pos.Y - 2, 0);
        int xmax = Mathf.Min(pos.X + 2, grid.GetLength(0) - 1);
        int ymax = Mathf.Min(pos.Y + 2, grid.GetLength(1) - 1);

        for (int y = ymin; y <= ymax; y++)
        {
            for (int x = xmin; x <= xmax; x++)
            {
                Vector2 s = grid[x, y];
                if (s != Vector2.zero)
                {
                    Vector2 d = s - sample;
                    if (d.x * d.x + d.y * d.y < radius2) return false;
                }
            }
        }

        return true;

        // Note: we use the zero vector to denote an unfilled cell in the grid. This means that if we were
        // to randomly pick (0, 0) as a sample, it would be ignored for the purposes of proximity-testing
        // and we might end up with another sample too close from (0, 0). This is a very minor issue.
    }

    /// Adds the sample to the active samples queue and the grid before returning it
    private Vector2 AddSample(Vector2 sample)
    {
        activeSamples.Add(sample);

        //GridPos pos = new GridPos(sample, cellSize);
        //grid[pos.x, pos.y] = sample;

        
        Vec2 pos = CalculateGridPos(sample, cellSize);
        grid[pos.X, pos.Y] = sample;

        return sample;
    }

    private Vec2 CalculateGridPos(Vector2 sample, float cellSize)
    {
        return new Vec2((int)(sample.x / cellSize), (int)(sample.y / cellSize));
    }
}
