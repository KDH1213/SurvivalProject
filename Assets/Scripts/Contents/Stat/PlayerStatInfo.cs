using System.Collections.Generic;

[System.Serializable]
public enum Debuffs
{
    Hungry,
    Tired,
    Smell,
    Thirsty,
    Temperature
}
public class PlayerStatInfo
{
    private PlayerStatData stat;
    private Dictionary<Debuffs, float> debuffs = new Dictionary<Debuffs, float>();

    public void SetStat(PlayerStatData stat)
    {
        this.stat = stat;
    }

    public void SetDebuff(Debuffs debuff, float percent)
    {
        // 새로운 디버프면 추가, 기존에 있던 디버프면 수치 조절
        if (debuffs.ContainsKey(debuff))
        {
            debuffs.Add(debuff, percent);
        }
        debuffs[debuff] = percent;
    }

    public PlayerStatData CalulateStat()
    {
        // 디버프로 인한 스탯 변화 반영
        // 매개변수로는 디버프 종류 단일 혹은 디버프 전체를 받아와서 스탯에 반영
        // 반영된 스탯 리턴
        return null;
    }
}
