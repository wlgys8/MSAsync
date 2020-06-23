using System.Runtime.CompilerServices;
using UnityEngine;
using System;

namespace MS.Async.CompilerServices{
    public struct AssetBundleRequestAwaiter: ICriticalNotifyCompletion
    {
        private AssetBundleRequest _request;
        public AssetBundleRequestAwaiter(AssetBundleRequest request){
            _request = request;
        }

        public UnityEngine.Object GetResult(){
            return _request.asset;
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
