[System.Serializable]
public enum GatherType
{
    None,
    Bush,
    Branch,
    Tree,
    Rock,
    Stone,
    Box,        

    End
}

[System.Serializable]
[System.Flags]
public enum GatherTypeMask
{
    None = 1 << GatherType.None,
    Bush = 1 << GatherType.Bush,
    Branch = 1 << GatherType.Branch,
    Tree = 1 << GatherType.Tree,
    Rock = 1 << GatherType.Rock,
    Stone = 1 << GatherType.Stone,
    Box = 1 << GatherType.Box,

}