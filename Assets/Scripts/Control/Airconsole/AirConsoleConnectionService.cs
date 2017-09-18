using NDream.AirConsole;
using UnityEngine;

namespace Control.Airconsole
{
    public class AirConsoleConnectionService : MonoBehaviour
    {
        private int _activePlayers;

        void Awake()
        {
            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
        }

        /// <summary>
        /// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
        ///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
        ///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
        /// </summary>
        /// <param name="deviceId">The device_id that connected</param>
        void OnConnect(int deviceId)
        {
            ChangeMaxPlayerCount(+1);
        }

        void OnDisconnect(int deviceId)
        {
            ChangeMaxPlayerCount(-1);
        }

        private void ChangeMaxPlayerCount(int change)
        {
            _activePlayers += change;

            Debug.Log("MaxPlayer: " + _activePlayers);

            AirConsole.instance.SetActivePlayers(_activePlayers);
        }

    }
}