using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThreadEffect : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Crate _crate;
    [Header("Thread Settings")]
    public int pointCount = 20;
    public float drawDuration = 1f;
    public float waitBeforeRetract = 2f;
    public float retractDuration = 1f;

    [Header("Wave Motion")]
    public float waveAmplitude = 0.1f;
    public float waveFrequency = 2f;
    public float animationSpeed = 3f;
    public float curveTightness = 1f;

    private LineRenderer line;
    private float timer = 0f;
    private float waitTimer = 0f;
    private float retractTimer = 0f;

    private enum Phase { Idle, Drawing, Waiting, Retracting }
    private Phase currentPhase = Phase.Idle;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (transform.childCount== 0) return;
        Color child = transform.GetChild(0).transform.GetComponent<MeshRenderer>().material.color;
        if (child != null)
        {
            line.material.color = child;
        }
        line.positionCount = 0;

    }

    public void ROPE_GO()
    {
        if(endPoint != null)
        {
            timer = 0f;
            currentPhase = Phase.Drawing;
        }
        
    }

    void Update()
    {
        if (endPoint != null)
            if (Input.GetKeyDown(KeyCode.M) && currentPhase == Phase.Idle)
        {
            ROPE_GO();
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
                    waitTimer = 0f;
                    currentPhase = Phase.Waiting;
                }
                break;

            case Phase.Waiting:
                waitTimer += Time.deltaTime;

                // Keep animating thread positions
                for (int i = 0; i < pointCount; i++)
                {
                    float t = i / (float)(pointCount - 1);
                    line.SetPosition(i, GetThreadPosition(t));
                }

                if (waitTimer >= waitBeforeRetract)
                {
                    retractTimer = 0f;
                    currentPhase = Phase.Retracting;
                }
                break;

            case Phase.Retracting:
                retractTimer += Time.deltaTime;
                float retractProgress = Mathf.Clamp01(retractTimer / retractDuration);
                int remainingCount = Mathf.CeilToInt(pointCount * (1f - retractProgress));
                remainingCount = Mathf.Max(0, remainingCount); // Safety check
                line.positionCount = remainingCount;

                if (remainingCount > 0)
                {
                    float denominator = Mathf.Max(1, remainingCount - 1);
                    for (int i = 0; i < remainingCount; i++)
                    {
                        float t = Mathf.Lerp(retractProgress, 1f, i / denominator);
                        line.SetPosition(i, GetThreadPosition(t));
                    }
                }

                if (retractProgress >= 1f)
                {
                    currentPhase = Phase.Idle;
                    if (_crate != null)
                    {
                        DOVirtual.DelayedCall(.1f, () =>
                        {
                            _crate.checkFull();
                        });
                        SoundHapticManager.instance.PlayAudioWithOutVibration("ROUNDING");
                    }
                    line.positionCount = 0;
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
