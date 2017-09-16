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

        void Awake()
        {
            AirConsole.instance.onMessage += OnMessage;
            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
            StatusText.text = "Awake";
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
                HandleJump(data);
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

        private void HandleJump(JToken data)
        {
            if(data["jump"] == null) return;

            _jumpButtonDown = (bool)data["jump"]["pressed"];
        }

        void StartGame()
        {
            AirConsole.instance.SetActivePlayers(1);
            _gameIsRunning = false;
        }

        private void StopGame()
        {
            _gameIsRunning = false;
        }

        private float _horizontal;
        private bool _hitButtonDown;
        private bool _jumpButtonDown;


        public float GetAxis(string axisName)
        {
            return _horizontal;
        }

        public bool GetButtonDown(string buttonName)
        {
            //todo make generic
            return _jumpButtonDown;
        }

        public bool GetButtonUp(string buttonName)
        {
            return false;
        }
    }
}
