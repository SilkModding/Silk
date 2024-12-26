using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Silk.API {
    class UI {
        public static UI instance;

        public void Initialize() { 
            instance = this;
        }
        public GameObject CreatePanel()
        {
            GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(400, 600); // Set default size
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localPosition = Vector3.zero;

            Image bg = panel.GetComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.8f); // Semi-transparent background

            return panel;
        }

        public TMP_Text CreateText(string text, GameObject parent, int fontSize = 20, TextAlignmentOptions alignment = TextAlignmentOptions.Center, Color? color = null)
        {
            GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TMP_Text));
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(380, 40);
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);

            TMP_Text tmpText = textObj.GetComponent<TMP_Text>();
            tmpText.text = text;
            tmpText.fontSize = fontSize;
            tmpText.alignment = alignment;
            tmpText.color = color ?? Color.white;

            textObj.transform.SetParent(parent.transform, false);
            return tmpText;
        }

        public Button CreateButton(string buttonName, GameObject parent, UnityAction onClick)
        {
            GameObject buttonObj = new GameObject("Button", typeof(RectTransform), typeof(Button), typeof(Image));
            RectTransform rect = buttonObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 50);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.localPosition = Vector3.zero;

            Image image = buttonObj.GetComponent<Image>();
            image.color = Color.gray; // Button background color

            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(onClick);

            GameObject textObj = new GameObject("ButtonText", typeof(RectTransform), typeof(TMP_Text));
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(200, 50);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.pivot = new Vector2(0.5f, 0.5f);

            TMP_Text text = textObj.GetComponent<TMP_Text>();
            text.text = buttonName;
            text.fontSize = 18;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            textObj.transform.SetParent(buttonObj.transform, false);
            buttonObj.transform.SetParent(parent.transform, false);

            return button;
        }

        public GameObject CreateDivider(GameObject parent)
        {
            GameObject divider = new GameObject("Divider", typeof(RectTransform), typeof(Image));
            RectTransform rect = divider.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(380, 2);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);

            Image image = divider.GetComponent<Image>();
            image.color = Color.gray;

            divider.transform.SetParent(parent.transform, false);
            return divider;
        }

    }
}