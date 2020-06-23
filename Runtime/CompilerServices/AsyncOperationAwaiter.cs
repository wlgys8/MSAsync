using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace MS.Async.CompilerServices{
    public struct AsyncOperationAwaiter : ICriticalNotifyCompletion
    {
        private AsyncOperation _operation;
        public AsyncOperationAwaiter(AsyncOperation operation){
            _operation = operation;
        }

        public void GetResult(){
        }

        public bool IsCompleted{
            get{
                return _operation.isDone;
            }
        }


        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if(IsCompleted){
                continuation();
            }else{
                _operation.completed += (op)=>{
                    continuation();
                };
            }
        }
    }
}
