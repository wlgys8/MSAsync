using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using System.Threading;

namespace MS.Async.CompilerServices{

    public static class UnityThreadContextCapture{

        private static SynchronizationContext _context;
        private static int _unityThreadId;
        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Launch(){
            _context = SynchronizationContext.Current;
            _unityThreadId = Thread.CurrentThread.ManagedThreadId;
            _initialized = true;
        }

        public static SynchronizationContext Context{
            get{
                AssertInitialized();
                return _context;
            }
        }

        private static void AssertInitialized(){
            if(!_initialized){
                throw new FieldAccessException("UnityThreadContextCapture has not been initialized. Please call UnityThreadContextCapture.Initialize in UnityThread first before you use await UnityThread()");
            }
        }

        public static bool IsUnityThread(){
            AssertInitialized();
            return Thread.CurrentThread.ManagedThreadId == _unityThreadId;
        }

        
    }
    public struct UnityThreadAwaiter : ICriticalNotifyCompletion
    {
        
        private static SendOrPostCallback _callback = (object state)=>{
            var continuation = state as Action;
            continuation();
        };
        
        public void GetResult(){
            
        }
        public bool IsCompleted{
            get{
                return UnityThreadContextCapture.IsUnityThread();
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            UnityThreadContextCapture.Context.Send(_callback,continuation);
        }
    }
}
