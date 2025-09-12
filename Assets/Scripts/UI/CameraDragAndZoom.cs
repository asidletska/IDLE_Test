using UnityEngine;

public class CameraDragAndZoom : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 0.5f;
    public Vector2 panLimitX = new Vector2(-10, 10);
    public Vector2 panLimitZ = new Vector2(-10, 10);

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.05f;
    public float minZoom = 3f;
    public float maxZoom = 15f;

    private Vector2 lastPanPosition;
    private int panFingerId;
    private bool isPanning;

    void Update()
    {
        if (Input.touchCount == 1) // свайп одним пальцем
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
                isPanning = true;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved && isPanning)
            {
                PanCamera(touch);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPanning = false;
            }
        }
        else if (Input.touchCount == 2) // pinch-to-zoom
        {
            isPanning = false; // вимикаємо свайп, якщо з’явився другий палець
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;
            Vector2 touch2Prev = touch2.position - touch2.deltaPosition;

            float prevMagnitude = (touch1Prev - touch2Prev).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ZoomCamera(difference * zoomSpeed);
        }
    }

    void PanCamera(Touch touch)
    {
        Vector2 delta = touch.position - lastPanPosition;
        lastPanPosition = touch.position;

        Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, 0, -delta.y * panSpeed * Time.deltaTime);
        transform.Translate(move, Space.World);

        // обмеження по X і Z
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        transform.position = pos;
    }

    void ZoomCamera(float increment)
    {
        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            cam.orthographicSize -= increment;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
