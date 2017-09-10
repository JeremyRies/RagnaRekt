using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Control;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LifeSystem
{
    public class TeamPointSystem : MonoBehaviour
    {
        public Text[] TeamTexts;

        private Team _team1;
        private Team _team2;

        private int _pointsForWin = 5;

        private int _matchPointAmount = 2;

        private Dictionary<Team, int> _teamPointCounter = new Dictionary<Team, int>();

        private IInputProvider _inputProvider;

        private Dictionary<Team, Text> _teamPointCounterDisplay = new Dictionary<Team, Text>();
        [SerializeField] private GameObject _hatiWinPrefab;
        [SerializeField] private GameObject _skalliWinPrefab;

        private void Start()
        {
            UpdateDisplay();
            _inputProvider = new UnityInputProvider(1);
        }

        public void AddTeams(Team team1, Team team2)
        {
            _team1 = team1;
            _team2 = team2;

            _teamPointCounter.Add(team1, 0);
            _teamPointCounterDisplay.Add(team1, TeamTexts[0]);
            
            _teamPointCounter.Add(team2, 0);
            _teamPointCounterDisplay.Add(team2, TeamTexts[1]);
        }

        public void ScorePoint(Team team)
        {
            Debug.Log(team.TeamId);
            _teamPointCounter[team]++;
            Debug.Log("Score team " + team.TeamId +" : " + _teamPointCounter[team]);
            UpdateDisplay();
            CheckWin();
        }

        private void UpdateDisplay()
        {
            foreach (var team in _teamPointCounter.Keys)
            {
                _teamPointCounterDisplay[team].text = _teamPointCounter[team].ToString();
            }
        }

        private void CheckWin()
        {
            foreach (var team in _teamPointCounter.Keys)
            {
                var points = _teamPointCounter[team];
                if (points >= _pointsForWin && IsMatchPointsAheadOfOtherTeam(team, points))
                {
                    Win(team);
                }
            }
        }

        private void Win(Team team)
        { 
            Debug.Log("Team: " + team.TeamId + " wins!");

            foreach (var player in _team1.GetPlayers())
            {
                player.GetPlayerController().DisableInput();
            }
            foreach (var player in _team2.GetPlayers())
            {
                player.GetPlayerController().DisableInput();
            }

            if (team.TeamId == 1)
            {
                Instantiate(_skalliWinPrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Instantiate(_hatiWinPrefab, Vector3.zero, Quaternion.identity);
            }

            Observable.EveryUpdate().Where(_ => _inputProvider.GetButtonDown("Jump")).Subscribe(_ =>  LevelController.GetInstance().LoadMenuScene()).AddTo(gameObject);
        }

        private bool IsMatchPointsAheadOfOtherTeam(Team team, int points)
        {
            return PointsOfOtherTeam(team) + _matchPointAmount <= points;
        }

        private int PointsOfOtherTeam(Team team)
        {
            return _teamPointCounter[ GetOtherTeam(team)];
        }

        public Team GetOtherTeam(Team team)
        {
            return _teamPointCounter.First(x => x.Key != team).Key;
        }
    }
}