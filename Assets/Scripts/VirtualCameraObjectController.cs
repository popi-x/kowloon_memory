using UnityEngine;
using Cinemachine;

public class VirtualCameraObjectController : MonoBehaviour
{
    [Header("必需引用")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("控制参数")]
    public float followDistance = 3f;      // 物体在相机前方的距离
    public float ySensitivity = 0.5f;      // Y轴移动灵敏度
    public float moveSmoothness = 8f;      // 移动平滑度

    [Header("选择设置")]
    public LayerMask selectableLayers = -1;
    public float maxSelectionDistance = 50f;

    // 内部状态
    private GameObject selectedObject;
    private bool isDragging = false;
    private float currentYOffset = 0f;
    private Vector3 targetPosition;

    void Start()
    {
        // 自动查找虚拟相机
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera == null)
                Debug.LogWarning("未找到CinemachineVirtualCamera，请手动指定");
        }

        // 确保有主相机用于射线检测
        if (Camera.main == null)
            Debug.LogWarning("场景中需要有一个标为Main Camera的相机");
    }

    void Update()
    {
        // 1. 处理左键选择
        if (Input.GetMouseButtonDown(0))
            TrySelectObject();

        // 2. 处理右键拖拽
        HandleDragInput();

        // 3. 更新选中物体的位置
        if (selectedObject != null)
            UpdateSelectedObjectPosition();
    }

    void TrySelectObject()
    {
        // 使用主相机进行射线检测（更可靠）
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayers))
        {
            // 选择新物体
            selectedObject = hit.collider.gameObject;
            currentYOffset = 0f;

            Debug.Log($"选择了: {selectedObject.name}");

            // 可选：添加选择视觉效果
            HighlightObject(selectedObject, true);
        }
        else
        {
            // 点击空白处取消选择
            if (selectedObject != null)
            {
                HighlightObject(selectedObject, false);
                selectedObject = null;
            }
        }
    }

    void HandleDragInput()
    {
        // 右键按下开始拖拽
        if (Input.GetMouseButtonDown(1) && selectedObject != null)
        {
            isDragging = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // 右键释放结束拖拽
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 拖拽时更新Y偏移
        if (isDragging)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            currentYOffset += mouseY * ySensitivity;

            // 可选：限制Y轴移动范围
            // currentYOffset = Mathf.Clamp(currentYOffset, -5f, 5f);
        }
    }

    void UpdateSelectedObjectPosition()
    {
        if (virtualCamera == null) return;

        // 获取虚拟相机的transform
        Transform camTransform = virtualCamera.transform;

        // 计算目标位置：相机前方固定距离 + Y轴偏移
        Vector3 basePosition = camTransform.position + camTransform.forward * followDistance;
        targetPosition = basePosition + Vector3.up * currentYOffset;

        // 平滑移动到目标位置
        selectedObject.transform.position = Vector3.Lerp(
            selectedObject.transform.position,
            targetPosition,
            Time.deltaTime * moveSmoothness
        );

        // 可选：让物体始终面向相机
        // selectedObject.transform.LookAt(camTransform);
    }

    void HighlightObject(GameObject obj, bool highlight)
    {
        MovableObject mobj = obj.GetComponent<MovableObject>();
        mobj.HighlightObject(highlight);
    }

    // 公共方法供其他脚本调用
    public GameObject GetSelectedObject() { return selectedObject; }
    public bool IsDragging() { return isDragging; }
    public void ForceSelect(GameObject obj)
    {
        if (selectedObject != null)
            HighlightObject(selectedObject, false);

        selectedObject = obj;
        currentYOffset = 0f;

        if (selectedObject != null)
            HighlightObject(selectedObject, true);
    }
}