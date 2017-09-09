using System.Linq;
using UnityEngine;

namespace LifeSystem
{
    public class TeamPointSystem : MonoBehaviour
    {
        private int _pointsForWin = 5;

        private int _matchPointAmount = 2;

        private int _teamCount=2;

        private int[] _teamPointCounter;

        private void Start()
        {
            _teamPointCounter = new int[_teamCount];
        }

        public void ScorePoint(int teamId)
        {
            _teamPointCounter[teamId]++;
            Debug.Log("Score team " + teamId +" : " + _teamPointCounter[teamId]);
            CheckWin();
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
                ScorePoint(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ScorePoint(1);
            }
        }
    }
}