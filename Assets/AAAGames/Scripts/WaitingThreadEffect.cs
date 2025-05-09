using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaitingThreadEffect : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    [Header("Thread Settings")]
    public int pointCount = 20;
    public float drawDuration = 1f;

    [Header("Wave Motion")]
    public float waveAmplitude = 0.1f;
    public float waveFrequency = 2f;
    public float animationSpeed = 3f;
    public float curveTightness = 1f;

    private LineRenderer line;
    private float timer = 0f;

    private enum Phase { Idle, Drawing, Active }
    private Phase currentPhase = Phase.Idle;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;

        line.material.color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        
    }
    void Start()
    {
        
    }

    public void ROPE_GO()
    {
        timer = 0f;
        currentPhase = Phase.Drawing;
    }

    public void StopLine()
    {
        line.positionCount = 0;
        currentPhase = Phase.Idle;
        //
    }

    void Update()
    {
        if (endPoint != null)
        {
            if (Input.GetKeyDown(KeyCode.M) && currentPhase == Phase.Idle)
            {
                ROPE_GO();
            }

            if (Input.GetKeyDown(KeyCode.N)) // Optional test for stopping the line
            {
                StopLine();
            }
        }

        switch (currentPhase)
        {
            case Phase.Drawing:
                timer += Time.deltaTime;
                float drawProgress = Mathf.Clamp01(timer / drawDuration);
                int drawCount = Mathf.CeilToInt(pointCount * drawProgress);
                line.positionCount = drawCount;

                for (int i = 0; i < drawCount; i++)
                {
                    float t = i / (float)(pointCount - 1);
                    line.SetPosition(i, GetThreadPosition(t));
                }

                if (drawProgress >= 1f)
                {
                    currentPhase = Phase.Active;
                }
                break;

            case Phase.Active:
                for (int i = 0; i < pointCount; i++)
                {
                    float t = i / (float)(pointCount - 1);
                    line.SetPosition(i, GetThreadPosition(t));
                }
                break;
        }
    }

    Vector3 GetThreadPosition(float t)
    {
        Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, t);
        Vector3 dir = (endPoint.position - startPoint.position).normalized;
        Vector3 up = Vector3.Cross(dir, Camera.main.transform.forward).normalized;

        float wave = Mathf.Sin(t * waveFrequency * Mathf.PI + Time.time * animationSpeed) * waveAmplitude;
        float curveOffset = Mathf.Sin(t * Mathf.PI) * curveTightness;

        return position + up * (wave + curveOffset);
    }
}
