[System.Serializable]
public enum GatherType
{
    None,
    Tree,
    Branch,
    Rock,
    Stone,
    Bush,
    Box,        

    End
}

[System.Serializable]
[System.Flags]
public enum GatherTypeMask
{
    None = 1 << GatherType.None,
    Tree = 1 << GatherType.Tree,
    Branch = 1 << GatherType.Branch,
    Rock = 1 << GatherType.Rock,
    Stone = 1 << GatherType.Stone,
    Bush = 1 << GatherType.Bush,
    Box = 1 << GatherType.Box,

}