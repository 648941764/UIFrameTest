using UnityEngine;

public static class MonoUtil
{
    public static void SetActivate(this GameObject gameObject, bool activeSelf)
    {
        if (gameObject.activeSelf != activeSelf)
        {
            gameObject.SetActive(activeSelf);
        }
    }

    public static void SetActivate(this Component component, bool activeSelf)
    {
        if (component.gameObject.activeSelf != activeSelf)
        {
            component.gameObject.SetActive(activeSelf);
        }
    }
}