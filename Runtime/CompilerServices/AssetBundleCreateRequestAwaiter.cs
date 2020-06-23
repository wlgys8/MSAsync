using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

namespace MS.Async.CompilerServices{
    public struct AssetBundleCreateRequestAwaiter: ICriticalNotifyCompletion
    {
        private AssetBundleCreateRequest _request;
        public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest request){
            _request = request;
        }
 
        public AssetBundle GetResult(){
            return _request.assetBundle;
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
