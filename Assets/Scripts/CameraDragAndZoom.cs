using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraDragAndZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;

    [Header("Pan (drag)")]
    [Tooltip("World-space bounds for camera center (min.x, min.y) .. (max.x, max.y)")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);
    [Tooltip("Pan speed multiplier")]
    public float panSpeed = 1f;
    [Tooltip("If true, use middle mouse or two-finger drag to pan. If false, left mouse will pan.")]
    public bool requireMiddleMouseForPan = true;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float pinchZoomSpeed = 0.1f;
    public float smoothTime = 0.08f;

    [Header("Orthographic limits (used when camera.orthographic == true)")]
    public float minOrthoSize = 3f;
    public float maxOrthoSize = 20f;

    [Header("Perspective limits (used when camera.orthographic == false)")]
    public float minFov = 20f;
    public float maxFov = 60f;

    // internal
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    private float targetOrthoSize;
    private float orthoVelocity = 0f;

    private float targetFov;
    private float fovVelocity = 0f;

    // touch tracking
    private bool isPanning = false;
    private int panFingerId = -1;
    private Vector2 lastPanScreenPos;

    private void Reset()
    {
        cam = GetComponent<Camera>();
    }

    private void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
        targetPosition = transform.position;

        if (cam.orthographic)
            targetOrthoSize = cam.orthographicSize;
        else
            targetFov = cam.fieldOfView;
    }

    private void Update()
    {
        // --- handle pan input (mouse + touch) ---
        // skip input when pointer over UI
        if (EventSystem.current != null && IsPointerOverUI())
        {
            // don't start new pan or zoom when interacting with UI
        }
        else
        {
            HandleMousePan();
            HandleTouchPanAndPinch();
            HandleMouseZoom();
        }
    }

    private void LateUpdate()
    {
        // Smooth position
        transform.position = Vector3.SmoothDamp(transform.position, ClampPositionToBounds(targetPosition), ref velocity, smoothTime);

        // Smooth zoom / orthographic size or FOV
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize), ref orthoVelocity, smoothTime);
        }
        else
        {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, Mathf.Clamp(targetFov, minFov, maxFov), ref fovVelocity, smoothTime);
        }
    }

    private void HandleMousePan()
    {
        // Which mouse button should pan?
        bool panButton = requireMiddleMouseForPan ? Input.GetMouseButton(2) : Input.GetMouseButton(0);

        // Start panning
        if (panButton && !isPanning)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // clicked on UI

            isPanning = true;
            lastPanScreenPos = Input.mousePosition;
        }

        // End panning
        if (!panButton)
        {
            isPanning = false;
            panFingerId = -1;
        }

        // Perform panning
        if (isPanning)
        {
            Vector2 current = (Vector2)Input.mousePosition;
            Vector2 delta = current - lastPanScreenPos;
            lastPanScreenPos = current;

            // convert screen delta to world delta
            Vector3 worldDelta = ScreenDeltaToWorldDelta(delta);
            targetPosition -= worldDelta * panSpeed;
        }
    }

    private void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            if (cam.orthographic)
            {
                targetOrthoSize -= scroll * zoomSpeed;
                targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
            }
            else
            {
                targetFov -= scroll * zoomSpeed * 10f;
                targetFov = Mathf.Clamp(targetFov, minFov, maxFov);
            }
        }
    }

    private void HandleTouchPanAndPinch()
    {
        if (Input.touchCount == 0)
        {
            panFingerId = -1;
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                // start pan
                panFingerId = t.fingerId;
                lastPanScreenPos = t.position;
                isPanning = true;
            }
            else if (t.phase == TouchPhase.Moved && isPanning && t.fingerId == panFingerId)
            {
                Vector2 delta = t.position - lastPanScreenPos;
                lastPanScreenPos = t.position;

                Vector3 worldDelta = ScreenDeltaToWorldDelta(delta);
                targetPosition -= worldDelta * panSpeed;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                isPanning = false;
                panFingerId = -1;
            }
        }
        else if (Input.touchCount >= 2)
        {
            // pinch to zoom
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // calculate previous positions
            Vector2 prev0 = t0.position - t0.deltaPosition;
            Vector2 prev1 = t1.position - t1.deltaPosition;

            float prevDist = Vector2.Distance(prev0, prev1);
            float curDist = Vector2.Distance(t0.position, t1.position);
            float diff = curDist - prevDist;

            if (Mathf.Abs(diff) > 0.0f)
            {
                if (cam.orthographic)
                {
                    targetOrthoSize -= diff * pinchZoomSpeed * Time.deltaTime;
                    targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
                }
                else
                {
                    targetFov -= diff * pinchZoomSpeed * Time.deltaTime * 10f;
                    targetFov = Mathf.Clamp(targetFov, minFov, maxFov);
                }
            }

            // while two fingers, disable single-finger pan
            isPanning = false;
            panFingerId = -1;
        }
    }

    /// <summary>
    /// Convert a screen-space delta (pixels) into a world-space delta at the camera's current view.
    /// Works sensibly for orthographic and top-down / free cameras.
    /// </summary>
    private Vector3 ScreenDeltaToWorldDelta(Vector2 screenDelta)
    {
        if (cam.orthographic)
        {
            // orthographic: size -> world units
            float worldPerPixelY = (cam.orthographicSize * 2f) / cam.pixelHeight;
            float worldPerPixelX = worldPerPixelY * cam.aspect;
            Vector3 worldDelta = new Vector3(screenDelta.x * worldPerPixelX, screenDelta.y * worldPerPixelY, 0f);

            // move relative to camera orientation (so dragging respects camera rotation)
            Vector3 right = cam.transform.right;
            Vector3 up = cam.transform.up;
            return right * -worldDelta.x + up * -worldDelta.y;
        }
        else
        {
            // perspective: convert by raycasting from screen points
            // approximate by using two rays a pixel apart and taking difference at plane through targetPosition
            Vector3 screenPoint = new Vector3(cam.pixelWidth / 2f, cam.pixelHeight / 2f, 0f);
            Ray rayCenter = cam.ScreenPointToRay(screenPoint);
            Plane plane = new Plane(Vector3.up, targetPosition); // plane at Y of target; good for top-down cameras

            if (plane.Raycast(rayCenter, out float enterCenter))
            {
                Vector3 worldCenter = rayCenter.GetPoint(enterCenter);

                Ray rayOffset = cam.ScreenPointToRay(screenPoint + (Vector3)screenDelta);
                if (plane.Raycast(rayOffset, out float enterOffset))
                {
                    Vector3 worldOffset = rayOffset.GetPoint(enterOffset);
                    return worldOffset - worldCenter;
                }
            }

            // fallback
            return cam.transform.TransformDirection(new Vector3(-screenDelta.x * 0.001f, -screenDelta.y * 0.001f, 0f));
        }
    }

    private Vector3 ClampPositionToBounds(Vector3 pos)
    {
        // For orthographic camera we should consider camera extents so camera doesn't show outside bounds
        if (cam.orthographic)
        {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            float minX = minBounds.x + horzExtent;
            float maxX = maxBounds.x - horzExtent;
            float minY = minBounds.y + vertExtent;
            float maxY = maxBounds.y - vertExtent;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            // preserve z
            pos.z = transform.position.z;
            return pos;
        }
        else
        {
            // For perspective camera we cannot easily compute frustum extents at arbitrary rotation.
            // We'll clamp center position to bounds (useful for top-down cams). Adjust if needed.
            pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
            pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
            pos.z = transform.position.z;
            return pos;
        }
    }

    private bool IsPointerOverUI()
    {
        // For touch: check if any touch is over UI
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;
            }
            return false;
        }

        // For mouse
        return EventSystem.current.IsPointerOverGameObject();
    }

    // PUBLIC helpers to center camera quickly from other scripts
    public void JumpTo(Vector3 worldPos)
    {
        targetPosition = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }

    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
        targetPosition = ClampPositionToBounds(targetPosition);
    }
}
