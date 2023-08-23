using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._GAME.Scripts.Game
{
    public class GameDriverPanel : MonoBehaviour
    {
        public Button HackMoney;

        private void Awake()
        {
            HackMoney?.onClick.AddListener(GameDriver.I.Deposit100Coin);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}