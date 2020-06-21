using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;


namespace MS.Async.CompilerServices{

    public class ConditionAwaiter : ICriticalNotifyCompletion
    {
        private static Action<object> _invokeContinuation = (obj)=>{
            Action continuation = obj as Action;
            continuation();
        };

        private Func<bool> _condition;
        public ConditionAwaiter(Func<bool> condition){
            _condition = condition;
        }
        public void GetResult(){
            
        }
        public bool IsCompleted{
            get{
                return _condition();
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            UnityLoops.ScheduleConditionalTask(new UnityLoops.ConditionalTask(){
                condition = _condition,
                action = _invokeContinuation,
                state = continuation
            });
        }       
    }
}
