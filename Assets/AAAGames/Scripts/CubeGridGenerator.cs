using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;

public class CubeGridGenerator : MonoBehaviour
{
    public static CubeGridGenerator instance;
    public CrateManager _crateManager;
    public int startIndex = 0;
    public int endIndex = 8;
    public float revealDelay = 0.1f;

    public int rows = 3;
    public int cols = 3;
    public GameObject cubePrefab;
    public Texture2D mainTexture;
    public Material baseMaterial;

    public ColorType revealGroup = ColorType.Red;
    [Header("Reveal Settings")]
    public int maxRevealCounts = 5;

    public List<CellColour> cubeList = new List<CellColour>();
    public List<Renderer> cubeRenderers = new List<Renderer>();
    public Transform borderDown,BoederUp; 

    [Header("Debug Grouped Cubes")]
    public List<ColorGroup> groupedCells = new List<ColorGroup>();

    [Header("Follower Settings")]
    public Transform followerObject;
    public float moveDuration = 0.25f;
    public Vector3 followerOffset = Vector3.zero;

    public GameObject PAINT_FRAME;
    public GameObject PAINT_FRAME_HIDING;
    public GameObject PAINT_FRAME_WIN;

    private Dictionary<ColorType, Color> colorTypeToColor = new Dictionary<ColorType, Color>()
    {
        { ColorType.Red,       new Color(1f, 0f, 0f) },
        { ColorType.Green,     new Color(0f, 1f, 0f) },
        { ColorType.Blue,      new Color(0f, 0f, 1f) },
        { ColorType.Yellow,    new Color(1f, 1f, 0f) },
        { ColorType.Orange,    new Color(1f, 0.5f, 0f) },
        { ColorType.Purple,    new Color(0.5f, 0f, 0.5f) },
        { ColorType.Black,     new Color(0f, 0f, 0f) },
        { ColorType.White,     new Color(1f, 1f, 1f) },
        { ColorType.Gray,      new Color(0.5f, 0.5f, 0.5f) },
        { ColorType.Lightblue, new Color(0.5f, 0.8f, 1f) },
        { ColorType.Pink,      new Color(1f, 0.4f, 0.7f) },
        { ColorType.Brown,     new Color(0.4f, 0.26f, 0.13f) },
        { ColorType.Darkgreen, new Color(0f, 0.3f, 0f) }
    };

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //Application.targetFrameRate = 60;
        _crateManager = FindObjectOfType<CrateManager>();
        if (Application.isPlaying)
        {
            GenerateGrid();
        }
        //
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            revealDelay = 0.001F;
            RevealTilesInRange(startIndex, endIndex);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            LAST_WAVE();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //STAGE_COUNT();
        }
    }

    public void GenerateGrid()
    {
        ClearGrid();
        float spacing = 0.3f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int index = y * cols + x;
                int flippedY = (rows - 1) - y;

                Vector3 position = new Vector3(transform.position.x + x * spacing, transform.position.y - flippedY * spacing, transform.position.z);
                Quaternion rotation = transform.rotation;
                GameObject cube = Instantiate(cubePrefab, position, rotation, transform);

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    cube.hideFlags = HideFlags.DontSave;
                }
#endif

                Material mat = new Material(baseMaterial);
                mat.SetVector("_GridSize", new Vector2(cols, rows));
                mat.SetVector("_GridIndex", new Vector2(x, y));
                Texture2D texToUse = Application.isPlaying ? Texture2D.whiteTexture : mainTexture;
                mat.SetTexture("_MainTex", texToUse);

                Renderer renderer = cube.GetComponent<Renderer>();
                renderer.material = mat;
                cube.name = $"Cube_{index}";

                CellColour cell = cube.GetComponent<CellColour>();
                cubeList.Add(cell);
                cubeRenderers.Add(renderer);
            }
        }

        CheckUniqueColors();
    }

    public void ClearGrid()
    {
#if UNITY_EDITOR
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
#else
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
#endif

        cubeList.Clear();
        cubeRenderers.Clear();
        groupedCells.Clear();
    }

    private void CheckUniqueColors()
    {
        if (mainTexture == null)
        {
            Debug.LogWarning("Main texture not set.");
            return;
        }

        float tolerance = 0.05f;
        List<(Color color, GameObject cube)> uniqueColorCubes = new List<(Color, GameObject)>();
        Dictionary<ColorType, List<CellColour>> grouped = new Dictionary<ColorType, List<CellColour>>();

        for (int i = 0; i < cubeRenderers.Count; i++)
        {
            Renderer renderer = cubeRenderers[i];
            Material mat = renderer.material;

            Vector2 gridIndex = mat.GetVector("_GridIndex");
            Vector2 gridSize = mat.GetVector("_GridSize");

            float u = gridIndex.x / (gridSize.x - 1);
            float v = gridIndex.y / (gridSize.y - 1);

            int x = Mathf.RoundToInt(u * (mainTexture.width - 1));
            int y = Mathf.RoundToInt(v * (mainTexture.height - 1));

            Color pixelColor = mainTexture.GetPixel(x, y);
            ColorType closestColor = GetClosestColorType(pixelColor);

            CellColour cell = cubeList[i];
            cell.CellType = closestColor;
            cell.Color = pixelColor;

            if (!grouped.ContainsKey(closestColor))
            {
                grouped[closestColor] = new List<CellColour>();
            }
            grouped[closestColor].Add(cell);

            bool foundSimilar = false;

            foreach (var entry in uniqueColorCubes)
            {
                if (IsColorSimilar(pixelColor, entry.color, tolerance))
                {
                    foundSimilar = true;
                    break;
                }
            }

            if (!foundSimilar)
            {
                uniqueColorCubes.Add((pixelColor, cell.gameObject));
            }
        }

        groupedCells.Clear();
        foreach (var kvp in grouped)
        {
            ColorGroup group = new ColorGroup();
            group.colorType = kvp.Key;
            group.cells = kvp.Value;
            groupedCells.Add(group);
        }

        Debug.Log($"Approx. unique colors: {uniqueColorCubes.Count}");
    }

    private bool IsColorSimilar(Color a, Color b, float threshold)
    {
        float diffR = Mathf.Abs(a.r - b.r);
        float diffG = Mathf.Abs(a.g - b.g);
        float diffB = Mathf.Abs(a.b - b.b);
        float diffA = Mathf.Abs(a.a - b.a);
        return diffR < threshold && diffG < threshold && diffB < threshold && diffA < threshold;
    }

    private ColorType GetClosestColorType(Color pixelColor)
    {
        float minDistance = float.MaxValue;
        ColorType closestType = ColorType.White;

        foreach (var entry in colorTypeToColor)
        {
            Color refColor = entry.Value;

            float rMean = (pixelColor.r + refColor.r) * 0.5f;
            float rDiff = pixelColor.r - refColor.r;
            float gDiff = pixelColor.g - refColor.g;
            float bDiff = pixelColor.b - refColor.b;

            float distance = Mathf.Sqrt(
                (2 + rMean) * rDiff * rDiff +
                4 * gDiff * gDiff +
                (2 + (1 - rMean)) * bDiff * bDiff
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                closestType = entry.Key;
            }
        }

        return closestType;
    }

    void RevealTilesInRange(int start, int end)
    {
        start = Mathf.Clamp(start, 0, cubeRenderers.Count - 1);
        end = Mathf.Clamp(end, 0, cubeRenderers.Count - 1);

        List<(int index, float x)> indexed = new List<(int, float)>();

        for (int i = start; i <= end; i++)
        {
            float xPos = cubeList[i].transform.position.x;
            indexed.Add((i, xPos));
        }

        indexed.Sort((a, b) => a.x.CompareTo(b.x)); // sort by X

        for (int order = 0; order < indexed.Count; order++)
        {
            int idx = indexed[order].index;
            float delay = order * revealDelay;

            DOVirtual.DelayedCall(delay, () =>
            {
                Renderer targetRenderer = cubeRenderers[idx];
                targetRenderer.material.color = targetRenderer.GetComponent<CellColour>().Color;

                Transform t = cubeList[idx].transform;
                t.localScale = Vector3.zero;
                t.DOScale(new Vector3(.3F, .3F, .3F), 0.25f).SetEase(Ease.OutBack);

                if (followerObject != null)
                {
                    followerObject.DOMove(t.position + followerOffset, moveDuration).SetEase(Ease.InOutQuad);
                }
            });
        }
    }


    public void RevealColorGroup(Crate Caller)
    {
        Transform currentFollower = Instantiate(followerObject, Vector3.zero, Quaternion.identity);
        Caller.transform.GetComponent<WaitingThreadEffect>().endPoint = currentFollower;

        foreach (ColorGroup group in groupedCells)
        {
            if (group.colorType != Caller.CrateColor) continue;

            List<CellColour> sorted = new List<CellColour>(group.cells);
            sorted.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y)); // Top to Bottom


            // Matching caps
            var matchingGroups = CrateManager.instance.groupedCaps
                .Where(capGroup => capGroup.colorType == group.colorType)
                .ToList();

            int matchingCapCount = matchingGroups.Sum(capGroup => capGroup.caps.Count);
            if (matchingCapCount == 0) return;

            // Local reveal vars (SCOPED TO THIS CALL)
            int maxRevealCount;
            int revealCount = Mathf.CeilToInt((float)sorted.Count / matchingCapCount);
            int remainder = sorted.Count % matchingCapCount;

            if (matchingCapCount <= 3)
            {
                if (revealCount % 2 == 0)
                {
                    maxRevealCount = revealCount * 3;
                }
                else
                {
                    maxRevealCount = (revealCount * 3) + Mathf.Min(remainder, 3);
                }
            }
            else
            {
                maxRevealCount = revealCount * 3;
            }

            // ✅ Clamp to avoid IndexOutOfRangeException
            maxRevealCount = Mathf.Min(maxRevealCount, sorted.Count);

            maxRevealCounts = maxRevealCount;

            foreach (var capGroup in matchingGroups)
            {
                int removeCount = Mathf.Min(3, capGroup.caps.Count);
                capGroup.caps.RemoveRange(0, removeCount);
            }

            // Local state for this invocation
            int Revel = 0;
            Crate thisCaller = Caller;
            List<CellColour> thisSorted = sorted;
            int thisMaxRevealCount = maxRevealCount;
            ColorGroup thisGroup = group;

            for (int i = 0; i < thisMaxRevealCount; i++)
            {
                int idx = i;
                float delay = idx * revealDelay;
                CellColour cell = thisSorted[idx];
                if (cell == null) continue;

                DOVirtual.DelayedCall(delay, () =>
                {
                    if (cell == null) return;

                    Renderer rend = cell.GetComponent<Renderer>();
                    if (rend != null)
                        rend.material.color = cell.Color;

                    Transform t = cell.transform;
                    t.localScale = Vector3.zero;
                    t.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.25f).SetEase(Ease.OutBack);

                    Revel++;
                    if (Revel == thisMaxRevealCount)
                    {
                        thisCaller.GetComponent<WaitingThreadEffect>().StopLine();
                        DOVirtual.DelayedCall(.3f, () =>
                        {
                            _crateManager.BringToSlot(thisCaller);

                        });
                    }

                    if (followerObject != null)
                    {
                        SoundHapticManager.instance.PlayAudio("STICHING",15);


                        currentFollower.DOMove(t.position + followerOffset, moveDuration).SetEase(Ease.InOutQuad);
                        gameObject.transform.DOScale(1.015F, 0.05F).SetEase(Ease.OutCirc).OnComplete(() =>
                        {
                            gameObject.transform.DOScale(1, 0.05F);
                        });

                        PAINT_FRAME.transform.DOScale(1.015F, 0.05F).SetEase(Ease.OutCirc).OnComplete(() =>
                        {
                            PAINT_FRAME.transform.DOScale(1, 0.05F);
                        });

                    }
                    ManageTheCrate.Instance.CancelFail();
                    cell.SORTED = true;
                    thisGroup.cells.Remove(cell);
                    if (BoederUp != null)
                    {
                        ScrollIfOutOfBounds(t);

                    }

                });
            }

            break; // done with this color
        }

    }
    private void ScrollIfOutOfBounds(Transform targetCell)
    {
        float cellY = targetCell.position.y;
        float borderDownY = borderDown.position.y;
        float borderUpY = BoederUp.position.y;

        float scrollAmount = 0f;

        if (cellY < borderDownY)
        {
            scrollAmount = borderDownY - cellY;
        }
        else if (cellY > borderUpY)
        {
            scrollAmount = borderUpY - cellY;
        }

        if (Mathf.Abs(scrollAmount) > 0.01f)
        {
            transform.DOMoveY(transform.position.y + scrollAmount, 0.05f).SetEase(Ease.Linear);
        }
    }
    public void LAST_WAVE()
    {
        gameObject.transform.DOMoveY(24.5F, 0.5F).SetEase(Ease.InBack).OnComplete(() =>
        {
            UI_MANAGER.Instance.PROGRESSION_BAR.SetActive(false);

            ManageTheCrate.Instance.transform.parent.gameObject.transform.DOScale(0, 0.25F).SetEase(Ease.InBack);

            PAINT_FRAME_WIN.SetActive(true);
            PAINT_FRAME_WIN.transform.SetParent(transform);
            PAINT_FRAME.transform.DOScale(0, 0.25F).SetEase(Ease.InBack);
            PAINT_FRAME_HIDING.SetActive(false);
            UI_MANAGER.Instance.zoomSlider.transform.parent.gameObject.SetActive(false);

            Camera.main.transform.DOMove(GAME_MANGER.Instance.CAM_LAST_POS, 1.5F).SetEase(Ease.Linear);
            Camera.main.DOOrthoSize(GAME_MANGER.Instance.CAM_LAST_ZOOM, 1.5F).SetEase(Ease.Linear);

            gameObject.transform.DOMoveY(15, 1.5F).SetEase(Ease.InBack).OnComplete(() =>
            {
                GAME_MANGER.Instance.LAST_EFFECT.SetActive(true);
                StartCoroutine(PlayLastWaveEffect());
            });
        });
        
    }

    IEnumerator PlayLastWaveEffect()
    {
        // Find center
        Vector3 center = Vector3.zero;
        foreach (var cube in cubeList)
            center += cube.transform.position;
        center /= cubeList.Count;

        // Create wave steps (group by rounded distance)
        var waveSteps = cubeList
            .Where(c => c.CellType != ColorType.White)
            .GroupBy(c => Mathf.RoundToInt(Vector3.Distance(c.transform.position, center) * 10f)) // *10f for better grouping resolution
            .OrderBy(g => g.Key);

        foreach (var group in waveSteps)
        {
            foreach (var cube in group)
            {
                GameObject CUBESS = cube.gameObject;

                // Scale animation
                CUBESS.transform.DOScale(0.45F, 0.25F)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        CUBESS.transform.DOScale(0.3F, 0.25F)
                            .SetEase(Ease.Linear);
                    });

                //// Optional bounce animation
                //CUBESS.transform.DOMoveY(CUBESS.transform.position.y + 0.02F, 0.25f)
                //    .SetEase(Ease.OutBack)
                //    .OnComplete(() =>
                //    {
                //        CUBESS.transform.DOMoveY(CUBESS.transform.position.y - 0.02F, 0.25F)
                //            .SetEase(Ease.Linear);
                //    });
            }

            yield return new WaitForSeconds(0.03f); // delay between wave groups
        }


        gameObject.transform.DOScale(.6F, 0.5F).SetEase(Ease.InBack).OnComplete(()=>
        {
            UI_MANAGER.Instance.WIN_PANEL.SetActive(true);

            gameObject.transform.DOMoveX(-2.75F, 0.5F).SetEase(Ease.Linear).OnComplete(()=>
            {
                if (NEW_FEATURE_PROGRESSION.INSTANCE != null)
                {
                    NEW_FEATURE_PROGRESSION.INSTANCE.FILL_COLOR();
                }
            });

        });
    }


#if UNITY_EDITOR
    public void GenerateGridInEditor()
    {
        GenerateGrid();
    }
#endif
}

[System.Serializable]
public class ColorGroup
{
    public ColorType colorType;
    public List<CellColour> cells = new List<CellColour>();
}
