using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guidance.Ending
{
    public interface INextEndingSceneController
    {
        public IEnumerator Run();
    }
}