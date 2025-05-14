[System.Serializable]
public enum SoundType
{
    // UI 1000 번 시작
    ButtonClick = 1000,
    StartMonsterWave = 1001,

    // Player 10000번 시작

     PlayerHandAttack = 10000,
     PlayerWeaponAttack = 10001,
     PlayerHit = 10002,
     PlayerDeath = 10003,
     PlayerLevelUp = 10004,

    // 11000 몬스터
    MonsterHit = 11000,

    // 20000 건물
    BulidingHit = 20000,

    // 30000 gaher
    GatherTree = 30000,
    GatherStone = 30001,
}