using UnityEngine;

class CustomCursor : MonoBehaviour
{
    public Texture2D texture;
    public CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        float factor = Screen.width / 1920f;
        Texture2D resized = Instantiate(texture);
        //resized.Resize((int)(texture.width * factor), (int)(texture.height * factor));
        Cursor.SetCursor(resized, Vector2.zero, cursorMode);
    }
}