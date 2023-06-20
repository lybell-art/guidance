using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBarrier : IDynamicPlatform
{
    bool isVertical{get;}
    bool isBroken{get;}
    void Enable();
    void Disable();
}
