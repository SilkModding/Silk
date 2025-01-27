using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Silk.API {
    /// <summary>
    /// UI helpers for mods.
    /// </summary>
    public class UI {
        /// <summary>
        /// The singleton instance of the UI helper.
        /// </summary>
        public static UI? instance;

        /// <summary>
        /// Initializes the UI helper.
        /// </summary>
        public void Initialize() { 
            instance = this;
        }

        /// <summary>
        /// Creates a panel
        /// </summary>
        /// <returns>The created panel.</returns>
        public GameObject CreatePanel()
        {
            // Create the panel
            GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            RectTransform rect = panel.GetComponent<RectTransform>();

            // Set default properties
            rect.sizeDelta = new Vector2(400, 600); 
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localPosition = Vector3.zero;
            
            // Set the background
            Image bg = panel.GetComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.8f); // Semi-transparent background

            return panel;
        }

        /// <summary>
        /// Creates a TMP text object with the given text.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="parent">The parent object of the text.</param>
        /// <param name="fontSize">The font size of the text. Defaults to 20.</param>
        /// <param name="alignment">The alignment of the text. Defaults to Center.</param>
        /// <param name="color">The color of the text. Defaults to white.</param>
        /// <returns>The created text object.</returns>
        public TMP_Text CreateText(string text, GameObject parent, int fontSize = 20, TextAlignmentOptions alignment = TextAlignmentOptions.Center, Color? color = null)
        {   
            // Create the text
            GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TMP_Text));
            RectTransform rect = textObj.GetComponent<RectTransform>();

            // Set default properties
            rect.sizeDelta = new Vector2(380, 40);
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);

            // Set the text
            TMP_Text tmpText = textObj.GetComponent<TMP_Text>();
            tmpText.text = text;
            tmpText.fontSize = fontSize;
            tmpText.alignment = alignment;
            tmpText.color = color ?? Color.white;

            // Set the parent
            textObj.transform.SetParent(parent.transform, false);
            return tmpText;
        }

        /// <summary>
        /// Creates a button with the given name and click handler.
        /// </summary>
        /// <param name="buttonName">The name of the button.</param>
        /// <param name="parent">The parent object of the button.</param>
        /// <param name="onClick">The click handler of the button.</param>
        /// <returns>The created button.</returns>
        public Button CreateButton(string buttonName, GameObject parent, UnityAction onClick)
        {
            // Create the button
            GameObject buttonObj = new GameObject("Button", typeof(RectTransform), typeof(Button), typeof(Image));
            RectTransform rect = buttonObj.GetComponent<RectTransform>();

            // Set default properties
            rect.sizeDelta = new Vector2(200, 50);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.localPosition = Vector3.zero;

            // Set the background
            Image image = buttonObj.GetComponent<Image>();
            image.color = Color.gray; // Button background color

            // Add click handler
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(onClick);

            // Create the text
            GameObject textObj = new GameObject("ButtonText", typeof(RectTransform), typeof(TMP_Text));
            RectTransform textRect = textObj.GetComponent<RectTransform>();

            // Set default properties
            textRect.sizeDelta = new Vector2(200, 50);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.pivot = new Vector2(0.5f, 0.5f);

            // Set the text
            TMP_Text text = textObj.GetComponent<TMP_Text>();
            text.text = buttonName;
            text.fontSize = 18;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            // Set the parent
            textObj.transform.SetParent(buttonObj.transform, false);
            buttonObj.transform.SetParent(parent.transform, false);
            
            // Return the button
            return button;
        }

        /// <summary>
        /// Creates a divider
        /// </summary>
        /// <param name="parent">The parent object of the divider.</param>
        /// <returns>The created divider.</returns>
        public GameObject CreateDivider(GameObject parent)
        {   
            // Create the divider
            GameObject divider = new GameObject("Divider", typeof(RectTransform), typeof(Image));
            RectTransform rect = divider.GetComponent<RectTransform>();

            // Set default properties
            rect.sizeDelta = new Vector2(380, 2);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);

            // Set the background
            Image image = divider.GetComponent<Image>();
            image.color = Color.gray;

            // Set the parent
            divider.transform.SetParent(parent.transform, false);
            return divider;
        }

    }
}
