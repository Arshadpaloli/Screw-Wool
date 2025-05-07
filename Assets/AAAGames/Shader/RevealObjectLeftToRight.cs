using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Renderer))]
public class SolidRevealController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float cutoff = 0f;
    public ColorType ReveleColor;
    public float revealDuration = 1f; // Time in seconds to fully reveal
    public bool Oqq;
    
    private Material mat;
    private Renderer rend;
    private Coroutine revealRoutine;
    public Crate reveal;

    void Start()
    {
        rend = GetComponent<Renderer>();
        reveal = transform.parent.GetComponent<Crate>();
        if (rend != null && rend.material != null)
        {
            mat = Instantiate(rend.material);
            rend.material = mat;
        }
        cutoff = 0f;
        ApplyMaterialProperties();
        // Optional: Start automatically
        // StartReveal(revealDuration);
    }

    private void ApplyMaterialProperties()
    {
        if (rend == null) return;

        mat = rend.material; // refresh material reference after any external color changes
        if (mat == null) return;

        Bounds bounds = rend.bounds;
        mat.SetFloat("_Cutoff", cutoff);
        mat.SetVector("_ObjectBoundsMin", bounds.min);
        mat.SetVector("_ObjectBoundsMax", bounds.max);
    }


    
   //ForGoingToThreadWaiting
   public void Start_revelForWaititngRop()
   {
       StartCoroutine(WaitingVis(0.25f));      
   }
   private IEnumerator WaitingVis(float duration)
   {
       cutoff = 0f;
       float elapsed = 0f;

       while (cutoff < 1f)
       {
           elapsed += Time.deltaTime;
           cutoff = Mathf.Clamp01(elapsed / duration);
           ApplyMaterialProperties();
           yield return null;
       }
        SoundHapticManager.instance.PlayAudioWithOutVibration("ROUNDING");
        revealRoutine = null;

   }
    public void Start_invincibleForWaititngRop()
    {
        if (revealRoutine == null)
        {
            revealRoutine = StartCoroutine(invincibleForWaititng(.3f));
        }
    }
    private IEnumerator invincibleForWaititng(float duration)
    {
        cutoff = 1f;
        float elapsed = 0f;

        while (cutoff > 0f)
        {
            elapsed += Time.deltaTime;
            cutoff = Mathf.Clamp01(1f - (elapsed / duration));
            ApplyMaterialProperties();
            yield return null;
        }
        Oqq = false;

        revealRoutine = null;

    }
    //EndOfWaiting
    public void StartReveal(Transform Center)
    {
        targetCutoff += 1f / 3f;
        targetCutoff = Mathf.Clamp01(targetCutoff); // Don't go beyond 1

        if (cutoff == 0)
        {
            
        }
        else
        {
            Center.position = new Vector3(Center.position.x+.4f, Center.position.y, Center.position.z);

        }
        if (revealRoutine == null)
        {
            revealRoutine = StartCoroutine(RevealOverTime(.5f));
        }
    }
    public void StartOne()
    {
        StartCoroutine(OnetoZ(time()));      
    }
    
    public float time()
    {
        float timee = CubeGridGenerator.instance.revealDelay * CubeGridGenerator.instance.maxRevealCounts ;
        return timee;
    }
   
    private IEnumerator OnetoZ(float duration)
    {
        cutoff = 1f;
        float elapsed = 0f;

        while (cutoff > 0f)
        {
            elapsed += Time.deltaTime;
            cutoff = Mathf.Clamp01(1f - (elapsed / duration));
            ApplyMaterialProperties();

            yield return null;
        }
        // reveal.OnFull();
        gameObject.SetActive(false);
        revealRoutine = null;
    }
    private float targetCutoff = 0f; // Total goal to reach over time
    
    private IEnumerator RevealOverTime(float duration)
    {
        while (cutoff < targetCutoff)
        {
            cutoff = Mathf.MoveTowards(cutoff, targetCutoff, Time.deltaTime / duration);
            ApplyMaterialProperties();
            yield return null;
        }

        revealRoutine = null;
    }

}