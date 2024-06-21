using UnityEngine;
using UnityEngine.EventSystems;

public class IsometricCamera : MonoBehaviour
{
    public Transform player;

    private float zoomScale = 2f;
    private float zoomMin = 3;
    private float zoomMax = 9.5f;

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    private void LateUpdate()
    {
        transform.position = player.position;
        transform.LookAt(player.transform);
    }

    void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomScale, zoomMin, zoomMax);
        }
    }
}