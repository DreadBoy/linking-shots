using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPopup : MonoBehaviour
{

    public Player player;
    public InteractionTrigger[] triggers;
    public Canvas canvas;
    public RectTransform rectTransform;
    public Text text;

    private void Update()
    {
        InteractionTrigger trigger = triggers.FirstOrDefault(tr => (player.transform.position2D() - tr.GetCenter()).sqrMagnitude < Mathf.Pow(tr.distance, 2));
        if (!trigger)
        {
            gameObject.SetActiveOnChildren(false);
            return;
        }
        text.text = " " + trigger.text + " (" + trigger.keyCode + ") ";
        var position = Camera.main.WorldToViewportPoint(trigger.GetCenter());
        rectTransform.anchoredPosition = new Vector2(canvas.GetComponent<RectTransform>().sizeDelta.x * position.x + 10, canvas.GetComponent<RectTransform>().sizeDelta.y * position.y + 10);
        gameObject.SetActiveOnChildren(true);
    }


    private void Reset()
    {
        player = FindObjectOfType<Player>();
        triggers = FindObjectsOfType<InteractionTrigger>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<Text>();
    }
}
