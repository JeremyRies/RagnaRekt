using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LifeSystem
{
    public class TeamPointSystem : MonoBehaviour
    {
        private int _pointsForWin = 5;

        private int _matchPointAmount = 2;

        private int _teamCount=2;

        private int[] _teamPointCounter;

        [SerializeField] private Text[] _teamPointCounterDisplay;

        private void Start()
        {
            _teamPointCounter = new int[_teamCount];
            UpdateDisplay();
        }

        public void ScorePoint(int teamId)
        {
            Debug.Log(teamId);
            _teamPointCounter[teamId-1]++;
            Debug.Log("Score team " + teamId +" : " + _teamPointCounter[teamId-1]);
            UpdateDisplay();
            CheckWin();
        }

        private void UpdateDisplay()
        {
            for (int teamIndex = 0; teamIndex < _teamCount; teamIndex++)
            {
                _teamPointCounterDisplay[teamIndex].text = _teamPointCounter[teamIndex].ToString();
            }
        }

        private void CheckWin()
        {
            for (int teamIndex = 0; teamIndex < _teamCount; teamIndex++)
            {
                var points = _teamPointCounter[teamIndex];
                if (points >= _pointsForWin && IsMatchPointsAheadOfOtherTeam(teamIndex,points))
                {
                    Win(teamIndex);
                }
            }
        }

        private void Win(int teamIndex)
        { 
            Debug.Log("Team: " + teamIndex + " wins!");

            foreach (var VARIABLE in COLLECTION)
            {
                
            }

            LevelController.GetInstance().LoadMenuScene();
        }

        private bool IsMatchPointsAheadOfOtherTeam(int teamIndex, int points)
        {
            return PointsOfOtherTeam(teamIndex) + _matchPointAmount <= points;
        }

        private int PointsOfOtherTeam(int teamIndex)
        {
            return _teamPointCounter[teamIndex == 0 ? 1 : 0];
        }

        private void Update()
        {
            //only for debug

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ScorePoint(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ScorePoint(2);
            }
        }
    }
}