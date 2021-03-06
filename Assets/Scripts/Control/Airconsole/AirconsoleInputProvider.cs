﻿using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Control.Airconsole
{
    public class AirconsoleInputProvider : MonoBehaviour, IInputProvider
    {
        private readonly Dictionary<string, bool> _buttonDown = new Dictionary<string, bool>
        {
            {"Jump",false},
            {"Attack",false},
            {"Skill",false},
        };

        private float _horizontal;
        private List<string> _buttonNames;
        private int _playerId;

        void Awake()
        {
            AirConsole.instance.onMessage += OnMessage;
            _buttonNames = _buttonDown.Keys.ToList();
        }

        public void Initialize(int playerId)
        {
            _playerId = playerId;
        }

        void OnMessage(int deviceId, JToken data)
        {
            var airConsolePlayerNumber = AirConsole.instance.ConvertDeviceIdToPlayerNumber(deviceId);
            int activePlayer = airConsolePlayerNumber + 1;

          //  Debug.Log("On Message - active Player: " + activePlayer);
            if (activePlayer == _playerId)
            {
            //    Debug.Log("Data: " + data);
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
    
            _horizontal = direction == "right" ? 1 : -1;
        }

        private void HandleButtonDown(JToken data,string buttonName)
        {
            if(data[buttonName] == null) return;

            var isPressed = (bool)data[buttonName]["pressed"];

            _buttonDown[buttonName] = isPressed;
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
