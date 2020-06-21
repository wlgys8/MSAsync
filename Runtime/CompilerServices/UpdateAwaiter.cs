using System.Runtime.CompilerServices;
using System;


namespace MS.Async.CompilerServices{
    public struct UpdateAwaiter : ICriticalNotifyCompletion
    {

        public void GetResult(){
            
        }
        public bool IsCompleted{
            get{
                return false;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            UnityLoops.ScheduleUpdate(continuation);
        }
    }
}
