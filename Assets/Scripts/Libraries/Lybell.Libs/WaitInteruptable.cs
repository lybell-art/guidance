using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lybell.Libs
{
    public class WaitInteruptable : IEnumerator
    {
        private IEnumerator wait1;
        private IEnumerator wait2;
        private System.Func<bool> condBase;
        private ConditionShrinker condition = null;
        private bool isTriggered = false;

        public WaitInteruptable(IEnumerator wait1, IEnumerator wait2, System.Func<bool> condition)
        {
            this.wait1 = wait1;
            this.wait2 = wait2;
            this.condBase = condition;
        }

        public object Current
        {
            get
            {
                if(isTriggered) return this.wait2.Current;
                return this.wait1.Current;
            }
        }

        public bool MoveNext()
        {
            if(!isTriggered && (condition == null ? false : condition.Get()) ) isTriggered = true;
            if(!isTriggered) condition = ConditionShrinker.Watch(condBase);
            
            if(isTriggered) return this.wait2.MoveNext();
            else return this.wait1.MoveNext();
        }

        public void Reset()
        {
            isTriggered = false;
            condition?.Get();
            condition = null;
        }
    }
}
