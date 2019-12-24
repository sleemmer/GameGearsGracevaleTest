using Assets.Scripts.Gui;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GameType
    {
        WithBuffs,
        WithoutBuffs,
    }

    public enum StatType
    {
        Health,
        Armor,
        Damage,
        Vampire,
    }

    public class BattleManager : IGameManager
    {
        private const int PLAYERS_MAX_COUNT = 2;

        public Action<GameType, PlayerData, PlayerData> OnGameStarted = null;
        public Action<int, PlayerData, float> OnPlayerStatChange = null;

        private static BattleManager instance = null;

        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BattleManager();
                }
                return instance;
            }
        }

        private LevelBehaviour levelBehaiour = null;
        private Data gameSettings = null;
        private GameType gameType = GameType.WithoutBuffs;

        private Dictionary<int, Player> playerList = new Dictionary<int, Player>();

        public override void Start()
        {
            base.Start();

            GameObject levelObject = GameObject.Find("LocationRoot");
            levelBehaiour = levelObject.GetComponent<LevelBehaviour>();

            var jsonTextFile = Resources.Load<TextAsset>("data");

            gameSettings = JsonUtility.FromJson<Data>(jsonTextFile.text);

            levelBehaiour.CameraController.CameraSettings = gameSettings.cameraSettings;
        }

        public void StartGame(GameType gameType)
        {
            this.gameType = gameType;

            // Создаем данные игроков
            playerList.Clear();
            for (int i = 0; i < PLAYERS_MAX_COUNT; i++)
            {
                playerList.Add(i, CreatePlayer(i));
                playerList[i].Behaviour.Idle();
            }

            if (OnGameStarted != null)
            {
                OnGameStarted(gameType, playerList[0].Data, playerList[1].Data);
            }
        }

        private Player CreatePlayer(int playerId)
        {
            Player player = new Player();

            PlayerData playerData = new PlayerData(playerId);
            List<Stat> playerStats = new List<Stat>();
            for (int i = 0; i < gameSettings.stats.Length; i++)
            {
                Stat stat = new Stat();
                stat.id = gameSettings.stats[i].id;
                stat.title = string.Copy(gameSettings.stats[i].title);
                stat.icon = string.Copy(gameSettings.stats[i].icon);
                stat.value = gameSettings.stats[i].value;
                stat.maxValue = gameSettings.stats[i].value;

                playerStats.Add(stat);
            }

            playerData.SetStats(playerStats);

            List<Buff> playerBuffs = new List<Buff>();
            if (gameType == GameType.WithBuffs)
            {
                // Добавляем игрокам баффы
                int buffsCount = UnityEngine.Random.Range(1, gameSettings.buffs.Length + 1);
                for (int i = 0; i < buffsCount; i++)
                {
                    int buffIndex = UnityEngine.Random.Range(0, gameSettings.buffs.Length);
                    playerBuffs.Add(gameSettings.buffs[buffIndex]);

                    // добавляем баффы к статам
                    for (int j = 0; j < playerBuffs[i].stats.Length; j++)
                    {
                        int statId = playerBuffs[i].stats[j].statId;
                        playerStats[statId].value += playerBuffs[i].stats[j].value;
                    }
                }

                playerData.SetBuffs(playerBuffs);
            }


            player.Data = playerData;
            player.Behaviour = levelBehaiour.Players[playerId];

            return player;
        }

        public void Attack(int attackerId)
        {
            playerList[attackerId].Behaviour.Attack();
            if (attackerId == 0)
            {
                ReceiveDamage(attackerId, 1);
            }
            else
            {
                ReceiveDamage(attackerId, 0);
            }
        }

        private void ReceiveDamage(int attackerId, int targetId)
        {
            //75% * damage / 100%
            float attackerDamage = playerList[attackerId].Data.StatList[(int)StatType.Damage].value;
            float targeArmor = playerList[targetId].Data.StatList[(int)StatType.Armor].value;

            float receivedDamage = (100 - targeArmor) * attackerDamage / 100;

            // отнимаем здоровье у цели
            playerList[targetId].Data.StatList[(int)StatType.Health].value -= receivedDamage;
            if (playerList[targetId].Data.StatList[(int)StatType.Health].value <= 0)
            {
                // игрок убит
                playerList[targetId].Behaviour.Death();

                playerList[targetId].Data.StatList[(int)StatType.Health].value = 0;
            }
            OnPlayerStatChange?.Invoke(targetId, playerList[targetId].Data, -receivedDamage);

            // восстанавливаем здоровье у атакующего за счет свойства вампиризм
            float attackerVampire = playerList[attackerId].Data.StatList[(int)StatType.Vampire].value;
            float restoredHealth = receivedDamage * attackerVampire / 100;
            OnPlayerStatChange?.Invoke(attackerId, playerList[attackerId].Data, restoredHealth);
        }

        public Player GetPlayer(int playerId)
        {
            return playerList[playerId];
        }
    }
}