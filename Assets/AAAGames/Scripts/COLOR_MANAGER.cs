using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLOR_MANAGER : MonoBehaviour
{
    public static COLOR_MANAGER Instance;

    public List<Material> ROLL_COLORS;
    public List<Material> TOP_ROLL_COLORS;
    public List<Material> THREAD_HOLDER_COLORS;
    public List<Material> KEY_COLORS;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDrawGizmos()
    {
        Instance = this;
    }
}
