using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public Dictionary<string, Component> ObjectPoolTable { get; private set; } = new Dictionary<string, Component>();

}
