using System.Collections.Generic;

/// <summary>
/// Класс, содержащий текущие статы и баффы игрока
/// </summary>
public class PlayerData
{
    public int Id = -1;
    public List<Stat> StatList = new List<Stat>();
    public List<Buff> BuffList = new List<Buff>();

    public PlayerData(int playerId)
    {
        Id = playerId;
    }

    public void SetStats(List<Stat> statList)
    {
        StatList = statList;
    }

    public void SetBuffs(List<Buff> buffList)
    {
        BuffList = buffList;
    }
}
