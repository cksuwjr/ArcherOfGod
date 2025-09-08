//using UnityEngine;

//public class CameraResolution : MonoBehaviour
//{
//    private void Awake()
//    {
//        if (TryGetComponent<Camera>(out var camera))
//        {
//            var cameraRect = camera.rect;

//            float xRatio = 9;
//            float yRatio = 16;

//            float scale_height = ((float)Screen.width / Screen.height) / (yRatio / xRatio);
//            float scale_width = 1f / scale_height;

//            if (scale_height < 1f)
//            {
//                cameraRect.height = scale_height;
//                cameraRect.y = (1f - scale_height) / 2f;
//            }
//            else
//            {
//                cameraRect.width = scale_width;
//                cameraRect.x = (1f - scale_width) / 2f;
//            }
//            camera.rect = cameraRect;
//        }
//    }
//}

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResolution : MonoBehaviour
{
    public float targetWidth = 1080f;   // 목표 해상도 (예: 1080x2340)
    public float targetHeight = 2340f;

    void Awake()
    {
        Camera cam = GetComponent<Camera>();

        float targetAspect = targetWidth / targetHeight;          // 목표 비율
        float windowAspect = (float)Screen.width / Screen.height; // 현재 실행 비율
        float scale = windowAspect / targetAspect;

        if (scale < 1.0f)
        {
            // 현재 화면이 세로가 더 길다 → 위아래에 검은 여백 추가됨
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scale;
            rect.x = 0;
            rect.y = (1.0f - scale) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // 현재 화면이 가로가 더 넓다 → 좌우에 검은 여백 추가됨
            float scaleWidth = 1.0f / scale;

            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}