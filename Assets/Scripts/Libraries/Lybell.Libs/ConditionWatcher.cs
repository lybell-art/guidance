using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lybell.Libs
{
    public class ConditionShrinker
    {
        private System.Func<bool> condition;
        private bool status;
        private ConditionShrinker(System.Func<bool> condition)
        {
            status = false;
            this.condition = condition;
        }
        public static ConditionShrinker Watch(System.Func<bool> condition)
        {
            ConditionShrinker instance = new ConditionShrinker(condition);
            ConditionWatcher.Instance.Mount(instance);
            return instance;
        }
        public bool Get()
        {
            ConditionWatcher.Instance.Unmount(this);
            return status;
        }
        internal void Judge()
        {
            if(condition()) status = true;
        }
    }

    public class ConditionWatcher : SingletonMonoBehaviour<ConditionWatcher>
    {
        private List<ConditionShrinker> watchList = new List<ConditionShrinker>();
        internal void Mount(ConditionShrinker target)
        {
            watchList.Add(target);
        }
        internal void Unmount(ConditionShrinker target)
        {
            bool successful = watchList.Remove(target);
        }
        void Update()
        {
            foreach (ConditionShrinker target in watchList)
            {
                target.Judge();
            }
        }
    }
}
