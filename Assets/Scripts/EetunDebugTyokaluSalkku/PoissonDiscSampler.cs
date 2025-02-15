﻿using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// Poisson-disc sampling using Bridson's algorithm.
/// Adapted from Mike Bostock's Javascript source: http://bl.ocks.org/mbostock/19168c663618b7f07158
///
/// See here for more information about this algorithm:
///   http://devmag.org.za/2009/05/03/poisson-disk-sampling/
///   http://bl.ocks.org/mbostock/dbb02448b0f93e4c82c3
///
/// Usage:
///   PoissonDiscSampler sampler = new PoissonDiscSampler(10, 5, 0.3f);
///   foreach (Vector2 sample in sampler.Samples()) {
///       // ... do something, like instantiate an object at (sample.x, sample.y) for example:
///       Instantiate(someObject, new Vector3(sample.x, 0, sample.y), Quaternion.identity);
///   }
///
/// Author: Gregory Schlomoff (gregory.schlomoff@gmail.com)
/// Released in the public domain
public class PoissonDiscSampler
{
    private const int k = 25;  // Maximum number of attempts before marking a sample as inactive.

    private  Rect rect;
    private  float radius2;  // radius squared
    private  float cellSize;
    private Vector2[,] grid;
    private List<Vector2> activeSamples = new List<Vector2>();


    private static PoissonDiscSampler _instance = null;
    public static PoissonDiscSampler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PoissonDiscSampler((float)Chunk.CHUNK_SIZE, (float)Chunk.CHUNK_SIZE, 1.5f);
            }
            return _instance;
        }
    }



    /// Create a sampler with the following parameters:
    ///
    /// width:  each sample's x coordinate will be between [0, width]
    /// height: each sample's y coordinate will be between [0, height]
    /// radius: each sample will be at least `radius` units away from any other sample, and at most 2 * `radius`.
    private int w, h;
    private float r;
    
    public PoissonDiscSampler(float width, float height, float radius)
    {
        r = radius;
        w = (int)width;
        h = (int)height;
        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
                           Mathf.CeilToInt(height / cellSize)];


        for (int i = 0; i < 10; i++)
        {
            preCalculatedSamples[i] = GetSamples();
            ReInit(radius, (int)width, (int)height);
            activeSamples.Clear();
        }

        // Instance = this;
    }

    public void ReInit(float radius, int width, int height)
    {
        
        r = radius;
        w = (int)width;
        h = (int)height;
        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
                           Mathf.CeilToInt(height / cellSize)];
    }

    /// Return a lazy sequence of samples. You typically want to call this in a foreach loop, like so:
    ///   foreach (Vector2 sample in sampler.Samples()) { ... }
    public IEnumerable<Vector2> Samples()
    {
        // First sample is choosen randomly
        yield return AddSample(new Vector2(Random.value * rect.width, Random.value * rect.height));

        while (activeSamples.Count > 0)
        {

            // Pick a random active sample
            int i = (int)Random.value * activeSamples.Count;
            Vector2 sample = activeSamples[i];

            // Try `k` random candidates between [radius, 2 * radius] from that sample.
            bool found = false;
            for (int j = 0; j < k; ++j)
            {
                 
                float angle = 2 * Mathf.PI * Random.value;
                float r = Mathf.Sqrt(Random.value * 3 * radius2 + radius2); // See: http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus/9048443#9048443
                Vector2 candidate = sample + r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Accept candidates if it's inside the rect and farther than 2 * radius to any existing sample.
                if (rect.Contains(candidate) && IsFarEnough(candidate))
                {
                    found = true;
                    yield return AddSample(candidate);
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
    }


    private List<Vector2>[] preCalculatedSamples = new List<Vector2>[10];
    private int index = 0;


    // worker
    private Thread PoissonSamplerThread;


    public List<Vector2> GetThreadedSamples()
    {
        CheckJobs();
        List<Vector2> value = preCalculatedSamples[index];

        // lauch job
        GenerateSamples(index);

        index++;

        if (index == 9)
        {
            index = 0;
        }
        
        return value; // VAROITUS !!!
    }
    // k k k g g g g g g g g t t t

    private int workIndex = 0;
    Job[] workers = new Job[3];

    private void GenerateSamples(int preCalcArrayIndex)
    {
        workers[workIndex] = new Job(preCalcArrayIndex, w, h, r);
        var worker = workers[workIndex];

        workIndex++;
        if (workIndex == 3)
        {
            workIndex = 0;
        }
        worker.Start();
    }

    private void CheckJobs()
    {
        for (int i = 0; i < workers.Length; i++)
        {
            var worker = workers[i];
            if (worker != null)
            {
                if (worker.Update())
                {
                    // job done!
                    preCalculatedSamples[worker.Index] = worker.OutData;
                    worker = null;
                }
            }
        }
    }

   


    public List<Vector2> GetSamples()
    {
        List<Vector2> solution = new List<Vector2>(200);
        AddSample(new Vector2(Random.value * rect.width, Random.value * rect.height));

        while (activeSamples.Count > 0)
        {

            // Pick a random active sample
            int i = (int)Random.value * activeSamples.Count;
            Vector2 sample = activeSamples[i];

            // Try `k` random candidates between [radius, 2 * radius] from that sample.
            bool found = false;
            for (int j = 0; j < k; ++j)
            {

                float angle = 2 * Mathf.PI * Random.value;
                float r = Mathf.Sqrt(Random.value * 3 * radius2 + radius2); // See: http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus/9048443#9048443
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

    ///// Helper struct to calculate the x and y indices of a sample in the grid
    //private struct GridPos
    //{
    //    public int x;
    //    public int y;

    //    public GridPos(Vector2 sample, float cellSize)
    //    {
    //        x = (int)(sample.x / cellSize);
    //        y = (int)(sample.y / cellSize);
    //    }
    //}
}