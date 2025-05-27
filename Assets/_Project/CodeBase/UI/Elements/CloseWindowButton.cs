using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class CloseWindowButton : MonoBehaviour
    {
        public Button Button;
        
        private void Awake()
        {
            if (Button == null)
                Button = GetComponent<Button>();

            Button.onClick.AddListener(CloseWindow);
        }
        private void CloseWindow()
        {
            MinigameUISinglton.Instance.CloseWindow();
        }
    }
}