using System.Collections.Generic;
using UnityEngine.Events;
using System.Runtime.CompilerServices;
using System;

namespace MS.Async.CompilerServices{

    internal class UnityEventContinuationInvoker:IDisposable{

        private UnityAction _action;
        private Action _continuation;
        private UnityEvent _event;
        public UnityEventContinuationInvoker(){
            _action = ()=>{
                try{
                    _event.RemoveListener(_action);
                    _continuation();
                }finally{
                    this.Dispose();
                }
            };
        }

        public void OnContinuation(UnityEvent evt, Action continuation){
            _event = evt;
            _continuation = continuation;
            _event.AddListener(_action);
        }

        public void Dispose()
        {
            _event = null;
            _continuation = null;
            _invokerPool.Push(this);
        }

        private static Stack<UnityEventContinuationInvoker> _invokerPool = new Stack<UnityEventContinuationInvoker>();

        public static UnityEventContinuationInvoker Request(){
            if(_invokerPool.Count == 0){
                return new UnityEventContinuationInvoker();
            }else{
                return _invokerPool.Pop();
            }
        }

    }

    /// <summary>
    /// await UnityEvent会产生少量的GC Alloc,其来源在于UnityEvent.AddListener和UnityEvent.RemoveListener两个接口的内部实现.
    /// </summary>
    public struct UnityEventAwaiter : ICriticalNotifyCompletion
    {
        private UnityEvent _unityEvent;

        public UnityEventAwaiter(UnityEvent evt){
            _unityEvent = evt;
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
            //UnityEventContinuationInvoker will be disposed automatically when event invoked.
            UnityEventContinuationInvoker.Request().OnContinuation(_unityEvent,continuation);
        }
    }

}
