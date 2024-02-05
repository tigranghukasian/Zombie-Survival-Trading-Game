using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public int PoolableId { get; set; }
    public GameObject GameObject { get; set; }

    public void OnPoolableObjectGet();

    public void OnPoolableObjectRelease();

}
