using UnityEngine.Events;
using UnityEngine;
using System;
namespace MS.Async{
    using CompilerServices;
    public static class UnityAwaiterExts
    {
        public static UnityEventAwaiter GetAwaiter(this UnityEvent evt){
            return new UnityEventAwaiter(evt);
        }
        public static YieldableAwaiter GetAwaiter(this YieldInstruction instruction){
            return new YieldableAwaiter(instruction);
        }

        public static ConditionAwaiter GetAwaiter(this Func<bool> condition){
            return new ConditionAwaiter(condition);
        }
    }
}
