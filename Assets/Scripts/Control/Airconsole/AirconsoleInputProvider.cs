using System.Collections.Generic;
using System.Linq;
using Control;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Airconsole
{
    public class AirconsoleInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField]
        public Text StatusText;

        private bool _gameIsRunning=false;

        private readonly Dictionary<string, bool> _buttonDown = new Dictionary<string, bool>
        {
            {"Jump",false},
            {"Attack",false},
            {"Skill",false},
        };

        private float _horizontal;
        private List<string> _buttonNames;

        void Awake()
        {
            AirConsole.instance.onMessage += OnMessage;
            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
            StatusText = GameObject.FindGameObjectWithTag("AirconsoleDebug").GetComponent<Text>();
            StatusText.text = "Awake";

            _buttonNames = _buttonDown.Keys.ToList();
        }

        /// <summary>
        /// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
        ///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
        ///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
        /// </summary>
        /// <param name="deviceId">The device_id that connected</param>
        void OnConnect(int deviceId)
        {
            if (!_gameIsRunning)
            {
                StatusText.text = "Starting game";
                StartGame();
            }
            else
            {
                StatusText.text = "No Players registered";
            }
        }

        void OnDisconnect(int deviceId)
        {
            int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);

            StatusText.text = "Active Player left: " + activePlayer;
            StopGame();
        }

        void OnMessage(int deviceId, JToken data)
        {
            int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);
            if (activePlayer != -1)
            {
                Debug.Log("Data: " + data);
                HandleHorizontalMovement(data);
                foreach (var button in _buttonNames)
                {
                    HandleButtonDown(data,button);
                }
                
            }
        }

        private void HandleHorizontalMovement(JToken data)
        {
            var dpadLeftData = data["dpad-left"];
            if(dpadLeftData == null) return;

            var pressed = (bool) dpadLeftData["pressed"];
            if (!pressed)
            {
                _horizontal = 0;
                return;
            }

            var direction = (string) dpadLeftData["message"]["direction"];
            Debug.Log("Direction:" + direction);

            _horizontal = direction == "right" ? 1 : -1;
        }

        private void HandleButtonDown(JToken data,string buttonName)
        {
            if(data[buttonName] == null) return;

            var isPressed = (bool)data[buttonName]["pressed"];

            _buttonDown[buttonName] = isPressed;
        }

        void StartGame()
        {
            AirConsole.instance.SetActivePlayers(1);
            _gameIsRunning = true;
        }

        private void StopGame()
        {
            _gameIsRunning = false;
        }

        public float GetAxis(string axisName)
        {
            return _horizontal;
        }

        public bool GetButtonDown(string buttonName)
        {
            return _buttonDown[buttonName];
        }

        public bool GetButtonUp(string buttonName)
        {
            return !_buttonDown[buttonName];
        }
    }
}
