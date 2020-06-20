using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;

namespace MS.Async.CompilerServices{
    public struct YieldableAwaiter : ICriticalNotifyCompletion
    {

        private static Action<object> _invokeContinuation = (p)=>{
            Action continuation = p as Action;
            continuation();
        };
        
        private YieldInstruction _yieldInstruction;


        public YieldableAwaiter(YieldInstruction instruction){
            _yieldInstruction = instruction;
        }

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
            UnityLoops.ScheduleYieldTask(new UnityLoops.YieldTask(){
                instruction = _yieldInstruction,
                callback = _invokeContinuation,
                state = continuation,
            });
        }
    }
}
