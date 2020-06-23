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

        public static ConditionAwaiter GetAwaiter(this Func<bool> condition){
            return new ConditionAwaiter(condition);
        }

        public static YieldableAwaiter GetAwaiter(this YieldInstruction instruction){
            return new YieldableAwaiter(instruction);
        }


        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation operation){
            return new AsyncOperationAwaiter(operation);
        }

        public static ResourceRequestAwaiter<UnityEngine.Object> GetAwaiter(this ResourceRequest request){
            return new ResourceRequestAwaiter<UnityEngine.Object>(request);
        }

        public static AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest request){
            return new AssetBundleCreateRequestAwaiter(request);
        }
        public static AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest request){
            return new AssetBundleRequestAwaiter(request);
        }


    }
}
