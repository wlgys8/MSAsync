using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MS.Async.CompilerServices{
    public struct ResourceRequestAwaiter<T> : ICriticalNotifyCompletion where T:UnityEngine.Object
    {

        private ResourceRequest _request;
        public ResourceRequestAwaiter(ResourceRequest request){
            _request = request;
        }
 
        public T GetResult(){
            return (_request.asset as T);
        }

        public bool IsCompleted{
            get{
                return _request.isDone;
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
                _request.completed += (op)=>{
                    continuation();
                };
            }
        }
    }
}
