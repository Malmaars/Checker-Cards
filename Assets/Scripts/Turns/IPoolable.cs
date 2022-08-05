using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    bool active { get; set; }
    void OnEnableObject();
    void OnDisableObject();
}
