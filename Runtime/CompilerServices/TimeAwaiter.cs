using System.Runtime.CompilerServices;
using System;

namespace MS.Async.CompilerServices{
    public struct TimeAwaiter : ICriticalNotifyCompletion
    {
        private static Action<object> _invokeContinuation = (p)=>{
            Action continuation = p as Action;
            continuation();
        };

        
        private DateTime _targetTime;
        public TimeAwaiter(System.DateTime targetTime){
            _targetTime = targetTime;
        }

        public void GetResult(){

        }

        public bool IsCompleted{
            get{
                return DateTime.Now >= _targetTime;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            UnityLoops.ScheduleTimeTask(_targetTime,_invokeContinuation,continuation);
        }
    }
}
