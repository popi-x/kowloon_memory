using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Header("材质设置")]
    public Material originalMaterial;  // 物体原本的材质
    public Material pickedMaterial;    // 被选中时的高亮材质

    // 内部状态
    private Renderer objectRenderer;
    private Material[] originalMaterials;  // 保存所有子材质的原始状态
    private bool isHighlighted = false;

    void Start()
    {
        // 获取渲染器组件
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError($"MovableObject需要Renderer组件: {gameObject.name}");
            return;
        }

        // 如果没有指定原始材质，使用当前的第一个材质
        if (originalMaterial == null && objectRenderer.materials.Length > 0)
        {
            originalMaterial = objectRenderer.materials[0];
        }

        // 保存所有子材质的原始状态（支持多材质物体）
        SaveOriginalMaterials();

        // 确保有碰撞体（用于射线检测）
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.LogWarning($"为 {gameObject.name} 添加了BoxCollider以便选择");
        }
    }

    void SaveOriginalMaterials()
    {
        if (objectRenderer != null)
        {
            originalMaterials = objectRenderer.materials;
        }
    }

    // 选中物体时调用
    public void HighlightObject(bool highlight)
    {
        if (objectRenderer == null || pickedMaterial == null) return;

        isHighlighted = highlight;

        if (highlight)
        {
            // 应用高亮材质（所有子材质都替换）
            Material[] highlightedMaterials = new Material[objectRenderer.materials.Length];
            for (int i = 0; i < highlightedMaterials.Length; i++)
            {
                highlightedMaterials[i] = pickedMaterial;
            }
            objectRenderer.materials = highlightedMaterials;
        }
        else
        {
            // 恢复原始材质
            objectRenderer.materials = originalMaterials;
        }
    }

    // 检查是否正在高亮显示
    public bool IsHighlighted()
    {
        return isHighlighted;
    }

    // 获取当前Y轴限制范围
    public Vector2 GetYLimits()
    {
        //return new Vector2(minYOffset, maxYOffset);
        return Vector2.zero;
    }

    // 为了方便调试，在Scene视图中显示范围
    void OnDrawGizmosSelected()
    {
        //if (!isHighlighted) return;

        //Gizmos.color = new Color(0, 1, 0, 0.3f);
        //Vector3 center = transform.position;
        //Vector3 size = GetComponent<Collider>()?.bounds.size ?? Vector3.one;

        //// 绘制Y轴移动范围
        //Vector3 minPos = center + Vector3.up * minYOffset;
        //Vector3 maxPos = center + Vector3.up * maxYOffset;

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(minPos, maxPos);
        //Gizmos.DrawSphere(minPos, 0.1f);
        //Gizmos.DrawSphere(maxPos, 0.1f);
    }
}