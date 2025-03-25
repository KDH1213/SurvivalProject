using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileable
{
    void Shot(GameObject owner, GameObject target);
}
