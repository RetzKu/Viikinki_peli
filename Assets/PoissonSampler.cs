using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PoissonSampler : MonoBehaviour
{
    public const int Width = 30;
    public const int Height = 35;
    public int Count;
    public const float Radius = 4f;
    public bool NeedsUpdate = true;

    public const int Size = 100;

    public static readonly float W = Radius / Mathf.Sqrt(2);
    private static readonly int cols = (int)Mathf.Floor(Width / W);
    private static readonly int rows = (int)Mathf.Floor(Height / W);
    private Vector2[] grid = new Vector2[cols * rows];

    public List<Vector2> Active = new List<Vector2>(100);

    // as the limit of samples to choose before rejection in the algorithm
    public int K = 30;

    private bool init = false;
    void OnDrawGizmos()
    {
        if (!init)
        {
            ResetAll();
        }

        Draw();

        // for (int i = 0; i < Count; i++)
        // {
        //     Gizmos.color = Color.cyan;

        //     Vector3 Position = new Vector3(Random.Range(0, Width), Random.Range(0, Height));
        //     Gizmos.DrawSphere(Position, Radius);
        // }
    }

    void Draw()
    {
        // While( ) {    }
        if (Active.Count > 0)
        {
            int randIndex = (Random.Range(0, Active.Count));
            Vector2 pos = Active[randIndex];
            bool found = false;


            bool ok = true;
            int col = 0, row = 0;
            Vector2 sample = new Vector2(-1, -1);
            for (int n = 0; n < K; n++)
            {
                sample = Random.insideUnitCircle.normalized;
                float m = Random.Range(Radius, 2 * Radius);
                sample *= m;
                sample += pos;

                col = (int)(sample.x / W);
                row = (int)(sample.y / W);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int index = (col + i) + (row + j) * cols;
                        if (index >= 0 && index < grid.Length)
                        {
                            Vector2 neighbour = grid[index];

                            if (neighbour.x != -1)
                            {
                                float dis = Vector2.Distance(sample, neighbour);
                                if (dis < Radius)
                                {
                                    ok = false;
                                    // maybe take a break
                                    // break;
                                }
                            }
                        }
                        if (ok)
                        {
                            found = true;
                            index = col + row * cols;
                            if (index >= 0 && index < grid.Length)
                            {
                                grid[index] = sample;
                                Active.Add(sample);
                            }

                        }
                    }
                }
            }



            if (!found)
            {
                Active.RemoveAt(randIndex); // opt
            }


        }

        Gizmos.color = Color.red;
        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i].x != -1)
            {
                Gizmos.DrawSphere(grid[i], Radius * 0.25f);
            }
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < Active.Count; i++)
        {
            Gizmos.DrawSphere(Active[i], Radius * 0.25f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Active.Clear();
            ResetAll();
        }
    }

    private void ResetAll()
    {
        grid = new Vector2[cols * rows];
        for (int i = 0; i < cols * rows; i++)
        {
            grid[i] = new Vector2(-1, -1);
        }

        {
            int x = Random.Range(0, Width);
            int y = Random.Range(0, Height);
            int i = Mathf.FloorToInt(y / W); // ympyrä indeksiksi
            int j = Mathf.FloorToInt(y / W);
            Vector2 pos = new Vector2(x, y);
            grid[i + j * cols] = pos;
            Active.Add(pos);
        }

        init = true;
    }
}
