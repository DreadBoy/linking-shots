using UnityEngine;

class CustomCursor : MonoBehaviour
{
    public Texture2D texture = null;
    public CursorMode cursorMode = CursorMode.Auto;
    public bool drawRay = false;

    private void Start()
    {
        Texture2D resized = Instantiate(texture);
        //float factor = Screen.width / 1920f;
        //resized.Resize((int)(texture.width * factor), (int)(texture.height * factor));
        Cursor.SetCursor(resized, new Vector2(resized.width / 2, resized.height / 2), cursorMode);
    }

    private void Update()
    {
        if (!drawRay)
            return;

        Vector3 currentMouse = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(currentMouse);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        Debug.DrawLine(ray.origin, hit.point);
    }
}