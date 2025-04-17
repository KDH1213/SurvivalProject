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
        // ���ο� ������� �߰�, ������ �ִ� ������� ��ġ ����
        if (debuffs.ContainsKey(debuff))
        {
            debuffs.Add(debuff, percent);
        }
        debuffs[debuff] = percent;
    }

    public PlayerStatData CalulateStat()
    {
        // ������� ���� ���� ��ȭ �ݿ�
        // �Ű������δ� ����� ���� ���� Ȥ�� ����� ��ü�� �޾ƿͼ� ���ȿ� �ݿ�
        // �ݿ��� ���� ����
        return null;
    }
}
