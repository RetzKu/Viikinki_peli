using UnityEngine;

// Apuluokka TouchControllerille
public class CurvedLineRendererController : MonoBehaviour
{
    public GameObject[] LinePoints = new GameObject[15];
    private int _pointIndex = 0;
    private int  MaxPoints = 15;

    public void Start()
    {
        for (int i = 0; i < LinePoints.Length; i++)
        {

            LinePoints[i].SetActive(false);
        }
    }

    public void SetPoint(Vector3 position)
    {
        if (_pointIndex < MaxPoints)
        {
            var point = LinePoints[_pointIndex++];
            point.transform.position = position;
            point.SetActive(true);
        }
    }

    public void ResetPoints()
    {
        for (int i = 0; i < _pointIndex; i++)
        {
            LinePoints[i].SetActive(false);
        }
        _pointIndex = 0;
    }

    public Vector3 GetLastPointPosition()
    {
        if (_pointIndex > 0)
        {
            return LinePoints[_pointIndex-1].transform.position;
        }
        return new Vector3(-1, -1, -1);
    }

    public int GetLinePointCount()
    {
        return _pointIndex + 1;
    }
}
