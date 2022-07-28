using UnityEngine;

public static class bl_JoystickUtils
{

    public static Vector3 TouchPosition(this Canvas _Canvas, int touchID)
    {
        Vector3 Return = Vector3.zero;

        if (_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
            Return = Input.GetTouch(touchID).position;
#else
            Return = Input.mousePosition;
#endif
        }
        else if (_Canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 pos = Vector3.zero;
            Vector2 tempVector = Vector2.zero;
#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
            if (touchID > -1)
            {
               pos = Input.GetTouch(touchID).position;

            }
            else
            {
                pos = Input.mousePosition;
            }
#else
             pos = Input.mousePosition;
#endif
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_Canvas.transform as RectTransform, pos, _Canvas.worldCamera, out tempVector);
            Return = _Canvas.transform.TransformPoint(tempVector);
        }

        return Return;
    }
}