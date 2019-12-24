using Assets.Scripts.Gui;
using System;
using System.Collections.Generic;

public class PlayerPanelFrame : GuiFrame
{
    private int STATS_POOL_COUNT = 5;
    private int BUFFS_POOL_COUNT = 5;

    public Action<int> OnAttackButtonClick = null;

    private PlayerData data = null;
    public PlayerData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            UpdateData();
        }
    }

    private PlayerPanelViewFrame view = null;
    private List<PlayerStatFrame> playerStatsFrameList = new List<PlayerStatFrame>();
    private List<PlayerStatFrame> playerBuffFrameList = new List<PlayerStatFrame>();

    public override string PrefabName
    {
        get { return "PlayerPanelFrame"; }
    }

    protected override void InitFormControls()
    {
        view = GetView<PlayerPanelViewFrame>();

        for (int i = 0; i < STATS_POOL_COUNT; i++)
        {
            var frame = AddFrame<PlayerStatFrame>(view.PlayerStatsHolder);
            frame.Enable();

            playerStatsFrameList.Add(frame);
        }

        for (int i = 0; i < BUFFS_POOL_COUNT; i++)
        {
            var frame = AddFrame<PlayerStatFrame>(view.PlayerStatsHolder);
            frame.Enable();

            playerBuffFrameList.Add(frame);
        }

        view.AttackButton.onClick.AddListener(onAttackButtonClick);
    }

    private void onAttackButtonClick()
    {
        OnAttackButtonClick?.Invoke(data.Id);
    }

    public void UpdateData()
    {
        if (data == null)
            return;

        UpdateStats(data.StatList);

        UpdateBuffs(data.BuffList);
    }

    public void UpdateStats(List<Stat> playerStats)
    {
        // Устанавливаем статы персонажа
        int frameCounter = 0;
        for (int i = 0; i < playerStats.Count; i++)
        {
            if (i >= playerStatsFrameList.Count)
            {
                var frame = AddFrame<PlayerStatFrame>(view.PlayerStatsHolder);
                playerStatsFrameList.Add(frame);
            }

            playerStatsFrameList[i].Enable();
            playerStatsFrameList[i].SetData(playerStats[i]);
            frameCounter++;
        }
        for (int j = frameCounter; j < playerStatsFrameList.Count; j++)
        {
            playerStatsFrameList[j].Disable();
        }
    }

    public void UpdateBuffs(List<Buff> playerBuffs)
    {
        // Устанавливаем активные баффы
        int frameCounter = 0;
        for (int i = 0; i < playerBuffs.Count; i++)
        {
            if (i >= playerBuffFrameList.Count)
            {
                var frame = AddFrame<PlayerStatFrame>(view.PlayerStatsHolder);
                frame.Enable();
                playerBuffFrameList.Add(frame);
            }

            playerBuffFrameList[i].Enable();
            playerBuffFrameList[i].SetData(playerBuffs[i]);
            frameCounter++;
        }
        for (int j = frameCounter; j < playerBuffFrameList.Count; j++)
        {
            playerBuffFrameList[j].Disable();
        }
    }
}
