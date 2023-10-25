using TMPro;
using UnityEngine;



namespace RedSnail
{
    public class ContentResizer : MonoBehaviour
    {
        private RectTransform content;

        [SerializeField]
        private TextMeshProUGUI textArea;



        private void Awake()
        {
            content = transform as RectTransform;
        }



        private void Update()
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, textArea.textBounds.size.y);
        }
    }
}