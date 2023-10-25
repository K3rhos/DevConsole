using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RedSnail
{
    public class Console : MonoBehaviour
    {
        private bool isOpen = false;

        private Animation anim;
        private bool hasAnimation = false;

        [SerializeField]
        private GameObject uIRoot;

        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private TextMeshProUGUI textArea;

        [SerializeField]
        private bool openOnAwake = false;

        [SerializeField]
        private bool useAnimations = true;



        private void Awake()
        {
            hasAnimation = TryGetComponent(out anim);

            isOpen = openOnAwake;

            inputField.onSubmit.AddListener(OnSubmit);

            Application.logMessageReceived += LogMessageReceived;

            if (openOnAwake)
            {
                OpenConsole();
            }
            else
            {
                if (uIRoot.activeSelf)
                {
                    CloseConsole(true);
                }
            }
        }



        private void LogMessageReceived(string _condition, string _stackTrace, LogType _type)
        {
            Dictionary<LogType, Color> colors = new Dictionary<LogType, Color>
            {
                { LogType.Error, Color.red },
                { LogType.Assert, Color.gray },
                { LogType.Warning, Color.yellow },
                { LogType.Log, Color.white },
                { LogType.Exception, Color.red }
            };

            string color = ColorUtility.ToHtmlStringRGB(colors[_type]);

            string currentTime = DateTime.Now.ToShortTimeString();

            textArea.text += $"<color=#{color}>[{currentTime}] [{_type}] {_condition}</color>\n";
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                isOpen ^= true;

                if (isOpen)
                {
                    OpenConsole();
                }
                else
                {
                    CloseConsole();
                }
            }
        }



        private void OpenConsole()
        {
            if (hasAnimation && useAnimations)
            {
                StartCoroutine(Coroutine_OpenConsole());
            }
            else
            {
                uIRoot.SetActive(true);

                inputField.ActivateInputField();
            }
        }



        private void CloseConsole(bool _noAnimation = false)
        {
            if (_noAnimation)
            {
                inputField.DeactivateInputField();

                uIRoot.SetActive(false);

                return;
            }

            if (hasAnimation && useAnimations)
            {
                StartCoroutine(Coroutine_CloseConsole());
            }
            else
            {
                inputField.DeactivateInputField();

                uIRoot.SetActive(false);
            }
        }



        private IEnumerator Coroutine_OpenConsole()
        {
            anim.Play("RedSnail_DevConsole_Open");

            while (anim.isPlaying)
            {
                yield return null;
            }

            inputField.ActivateInputField();
        }



        private IEnumerator Coroutine_CloseConsole()
        {
            anim.Play("RedSnail_DevConsole_Close");

            while (anim.isPlaying)
            {
                yield return null;
            }

            inputField.DeactivateInputField();
        }



        private void OnSubmit(string _text)
        {
            // TODO: Send command here...
            Debug.LogWarning("Command has been sent !");

            // Empty the text field
            inputField.text = null;

            // Re-Activate the text field
            inputField.ActivateInputField();
        }
    }
}