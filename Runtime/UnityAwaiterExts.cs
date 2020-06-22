using UnityEngine.Events;
using UnityEngine;
using System;
namespace MS.Async{
    using CompilerServices;
    public static class UnityAwaiterExts
    {
        public static SourceAwaiter GetAwaiter(this UnityEvent evt){
            return new SourceAwaiter(UnityEventSource.Request(evt));
        }

        public static SourceAwaiter<T> GetAwaiter<T>(this UnityEvent<T> evt){
            var source = UnityEventSource<T>.Request(evt);
            return new SourceAwaiter<T>(source);
        }
        public static YieldableAwaiter GetAwaiter(this YieldInstruction instruction){
            return new YieldableAwaiter(instruction);
        }

        public static ConditionAwaiter GetAwaiter(this Func<bool> condition){
            return new ConditionAwaiter(condition);
        }

    }
}
