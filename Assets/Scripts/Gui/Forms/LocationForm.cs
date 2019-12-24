using Assets.Scripts;
using Assets.Scripts.Gui;
using System.Collections.Generic;
using UnityEngine;

public class LocationForm : GuiForm
{
    private LocationViewForm view = null;

    private PlayerPanelFrame playerPanelLeftFrame = null;
    private PlayerPanelFrame playerPanelRightFrame = null;

    private List<HealthbarFrame> healthbarFrameList = new List<HealthbarFrame>();

    public override void Init(GameObject formRoot)
    {
        base.Init(formRoot);

        view = GetView<LocationViewForm>();

        view.PlayWithoutBuffButton.onClick.AddListener(onPlayWithoutBuffClick);
        view.PlayWithBuffButton.onClick.AddListener(onPlayWithBuffClick);

        playerPanelLeftFrame = AddFrame<PlayerPanelFrame>(view.PlayerPanelLeftHolder);
        playerPanelLeftFrame.OnAttackButtonClick += onAttackButtonClick;
        playerPanelLeftFrame.Enable();

        playerPanelRightFrame = AddFrame<PlayerPanelFrame>(view.PlayerPanelRightHolder);
        playerPanelRightFrame.OnAttackButtonClick += onAttackButtonClick;
        playerPanelRightFrame.Enable();

        for (int i = 0; i < 2; i++)
        {
            HealthbarFrame frame = AddFrame<HealthbarFrame>(view.HealthbarHolder);
            frame.Enable();
            healthbarFrameList.Add(frame);
        }

        BattleManager.Instance.OnGameStarted += onGameStarted;
        BattleManager.Instance.OnPlayerStatChange += onPlayerStatChange;
    }

    public override void Show()
    {
        base.Show();

        // по дефолтну начинаем игру без баффов
        BattleManager.Instance.StartGame(GameType.WithoutBuffs);
    }

    private void onPlayWithoutBuffClick()
    {
        BattleManager.Instance.StartGame(GameType.WithoutBuffs);
    }

    private void onPlayWithBuffClick()
    {
        BattleManager.Instance.StartGame(GameType.WithBuffs);
    }

    private void onGameStarted(GameType gameType, PlayerData player1, PlayerData player2)
    {
        // заполняем статы игроков
        playerPanelLeftFrame.Data = player1;
        healthbarFrameList[0].SetHealth(player1.StatList[(int)StatType.Health].value, player1.StatList[(int)StatType.Health].maxValue, 0);

        playerPanelRightFrame.Data = player2;
        healthbarFrameList[1].SetHealth(player2.StatList[(int)StatType.Health].value, player2.StatList[(int)StatType.Health].maxValue, 0);
    }

    private void onAttackButtonClick(int attackerId)
    {
        BattleManager.Instance.Attack(attackerId);
    }

    private void onPlayerStatChange(int playerId, PlayerData playerData, float receivedHealthCount)
    {
        Debug.Log("fsv playerId == " + playerId + "  healthCount == " + receivedHealthCount);
        healthbarFrameList[playerId].SetHealth(playerData.StatList[(int)StatType.Health].value, playerData.StatList[(int)StatType.Health].maxValue, receivedHealthCount);

        if (playerId == 0)
        {
            // обновляем статы игроков
            playerPanelLeftFrame.Data = playerData;
        }
        else
        {
            playerPanelRightFrame.Data = playerData;
        }
    }

    public void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 healthbarHolderPos = BattleManager.Instance.GetPlayer(i).Behaviour.GetHealthbarHolderPosition() + new Vector3(0, 1.3f, 0);
            healthbarFrameList[i].SetPosition(healthbarHolderPos);
        }
    }
}
