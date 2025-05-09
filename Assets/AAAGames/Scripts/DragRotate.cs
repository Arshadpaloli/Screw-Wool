using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotate : MonoBehaviour
{
    public static DragRotate Instance;
    public Transform CubeToRotate;
    public CrateManager _crateManager;
    private Vector2 lastTouchPosition;
    private float rotateSpeed = 0.35f;
    private Quaternion targetRotation;
    private float smoothTime = 10f;
    private bool isDragging = false;
    public bool GameStop;

    [SerializeField] private float dragThreshold = 5f;
    private bool isZooming = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        _crateManager = transform.GetComponent<CrateManager>();
        targetRotation = CubeToRotate.transform.rotation;
    }
  
void Update()
{
    if (!GameStop)
    {
        isZooming = false; // Reset each frame

        // PINCH ZOOM LOGIC
        if (Input.touchCount == 2)
        {
            if (EventSystem.current.IsPointerOverGameObject() ||
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) ||
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
            {
                return;
            }

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevMag = (touch0Prev - touch1Prev).magnitude;
            float currMag = (touch0.position - touch1.position).magnitude;

            float deltaMag = currMag - prevMag;

            float sliderStep = 1f / UI_MANAGER.Instance.maxZoomInSteps;
            float newSliderValue = UI_MANAGER.Instance.zoomSlider.value + Mathf.Sign(deltaMag) * sliderStep * Time.deltaTime * 5f;
            newSliderValue = Mathf.Clamp01(newSliderValue);

            if (Mathf.Abs(deltaMag) > 1f)
            {
                UI_MANAGER.Instance.zoomSlider.value = newSliderValue;
                UI_MANAGER.Instance.SetZoomFromSlider(newSliderValue);
                isZooming = true; // block rotation
            }
        }

        if (!isZooming)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() ||
                    (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                {
                    return;
                }

                lastTouchPosition = Input.mousePosition;
                isDragging = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Connect"))
                {
                    holdTarget = hit.collider.gameObject;
                    isHolding = true;
                    holdTimer = 0f;
                }
                else
                {
                    isHolding = false;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() ||
                    (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                {
                    return;
                }

                // Handle holding
                if (isHolding)
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdTime)
                    {
                        Debug.Log("Held on Connect object: " + holdTarget.name);
                        SetTransparentMaterials(holdTarget); // Triggered only after holdTime
                        isHolding = false; // Prevent repeated calls
                    }
                }

                Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;

                if (delta.sqrMagnitude > dragThreshold * dragThreshold)
                {
                    isDragging = true;
                }

                if (isDragging)
                {
                    float rotationX = delta.y * rotateSpeed;
                    float rotationY = -delta.x * rotateSpeed;

                    Quaternion xRotation = Quaternion.AngleAxis(rotationX, Vector3.right);
                    Quaternion yRotation = Quaternion.AngleAxis(rotationY, Vector3.up);

                    targetRotation = yRotation * xRotation * targetRotation;

                    lastTouchPosition = Input.mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() ||
                    (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                {
                    return;
                }

                // Only revert transparency if object was successfully held
                if (holdTarget != null)
                {
                    SetOpaqueMaterials(holdTarget);
                }

                isHolding = false;
                holdTimer = 0f;
                holdTarget = null;

                if (!isDragging)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Cap"))
                    {
                        BottlecapTomove(hit.collider.gameObject);
                        SoundHapticManager.instance.PlayAudio("TOUCH");

                        if (TUTORIAL.instance != null)
                        {
                            if (TUTORIAL.instance.TOUCHES == 0)
                            {
                                TUTORIAL.instance.FIRST_HAND_OFF();
                                TUTORIAL.instance.TOUCHES++;
                            }
                            else if (TUTORIAL.instance.TOUCHES == 1)
                            {
                                TUTORIAL.instance.SECOND_HAND_OFF();
                            }
                        }
                    }
                }
            }

            // Apply smooth rotation if not zooming
            CubeToRotate.transform.rotation = Quaternion.Lerp(CubeToRotate.transform.rotation, targetRotation, Time.deltaTime * smoothTime);
        }
    }
}

private void SetOpaqueMaterials(GameObject obj)
{
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer != null)
    {
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            Material material = materials[i];
            Color color = material.color;
            color.a = 1f;
            material.color = color;

            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.renderQueue = 2000;
        }
    }
    else
    {
        Debug.LogError("No Renderer found on this object.");
    }

    List<EachCap> TrayHold = obj.GetComponent<Tray>().TrayHold;

// ENABLE outline on TrayHold objects
    foreach (EachCap cap in TrayHold)
    {
        // Access child 0 of the EachCap GameObject
        Transform child = cap.transform.GetChild(0);
        Renderer capRenderer = child.GetComponent<Renderer>();

        if (capRenderer != null)
        {
            foreach (Material mat in capRenderer.materials)
            {
               
                if (mat.HasProperty("_OutlineWidth"))
                {
                    mat.SetFloat("_OutlineWidth", 0f);
                    Debug.Log("Set _OutlineWidth = 1 on: " + cap.name);
                }
                else
                {
                    Debug.LogWarning("Material on " + cap.name + " does not have _Outline or _OutlineWidth.");
                }
            }
        }
    }

}

private void SetTransparentMaterials(GameObject obj)
{
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer != null)
    {
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            Material material = materials[i];
            Color color = material.color;
            color.a = 0.5f;
            material.color = color;

            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.renderQueue = 3000;
        }
    }
    else
    {
        Debug.LogError("No Renderer found on this object.");
    }
    List<EachCap> TrayHold = obj.GetComponent<Tray>().TrayHold;

// ENABLE outline on TrayHold objects
    foreach (EachCap cap in TrayHold)
    {
        Transform child = cap.transform.GetChild(0);
        Renderer capRenderer = child.GetComponent<Renderer>();

        if (capRenderer != null)
        {
            foreach (Material mat in capRenderer.materials)
            {
               
                 if (mat.HasProperty("_OutlineWidth"))
                {
                    mat.SetFloat("_OutlineWidth", 4.4f);
                    Debug.Log("Set _OutlineWidth = 1 on: " + cap.name);
                }
                else
                {
                    Debug.LogWarning("Material on " + cap.name + " does not have _Outline or _OutlineWidth.");
                }
            }
        }
    }



}


public float holdTime;
private float holdTimer = 0f;
private bool isHolding = false;
private GameObject holdTarget;


    public void BottlecapTomove(GameObject Caps)
    {
        _crateManager.ThreadToSlot(Caps.GetComponent<EachCap>());
    }
}
