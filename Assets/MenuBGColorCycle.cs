using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuBGColorCycle : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private float transitionDuration = 30f; // Time to transition between colors
    private Color[] colors = new Color[]
    {
        new Color32(0x1F, 0x1F, 0x29, 0xFF), // #1F1F29
        new Color32(0x9C, 0x63, 0x0F, 0xFF), // #9C630F
        new Color32(0x23, 0x88, 0x96, 0xFF), // #238896
    };

    private int colorIndex = 0;
    private Color targetColor;

    private void Start()
    {
        targetColor = colors[1 % colors.Length]; // Initialize the first target color
        bg.color = colors[0];                   // Set the starting color
    }

    private void FixedUpdate()
    {
        // Incrementally approach the target color
        bg.color = Vector4.MoveTowards(bg.color, targetColor, Time.fixedDeltaTime / transitionDuration);

        // Check if the target color is reached
        if (bg.color == targetColor)
        {
            // Move to the next color in the array
            colorIndex = (colorIndex + 1) % colors.Length;
            targetColor = colors[colorIndex];
        }
    }
}