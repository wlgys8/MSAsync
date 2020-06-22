using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MS.Async{
    using System;
    using CompilerServices;

    internal class  UnityEventSource : IValueSource
    {
        private static Stack<UnityEventSource> _pool = new Stack<UnityEventSource>();

        internal static UnityEventSource Request(UnityEvent unityEvent){
            UnityEventSource source = null;
            if(_pool.Count == 0){
                source = new UnityEventSource();
            }else{
                source =  _pool.Pop();
            }
            source.Initialize(unityEvent);
            return source;
        }

        private static uint _globalToken = 0;
        private static uint AllocateToken(){
            do{
                _globalToken ++;
            }while(_globalToken == 0);
            return _globalToken;
        }

        public uint Token{
            get;private set;
        }

        private SourceStatus _status;
        private UnityEvent _unityEvent;
        private Action _continuation;
        private UnityAction _eventListener;

        private bool _isListenerRegisted = false;

        private UnityEventSource(){
            _eventListener = ()=>{
                _status = SourceStatus.Success;
                UnregisterListener();
                _continuation();
            };
        }

        private void UnregisterListener(){
            if(_isListenerRegisted && _unityEvent != null){
                _unityEvent.RemoveListener(_eventListener);
                _isListenerRegisted = false;
            }            
        }
        public void Initialize(UnityEvent unityEvent){
            _status = SourceStatus.Pending;
            _unityEvent = unityEvent;
            Token = AllocateToken();
        }

        private void Dispose(){
            if(Token == 0){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            UnregisterListener();
            Token = 0;
            _continuation = null;
            _pool.Push(this);
        }


        public void GetResult()
        {
            Dispose();
        }

        public SourceStatus GetStatus()
        {
            return _status;
        }

        public void OnComplete(Action continuation)
        {
            _continuation = continuation;
            _unityEvent.AddListener(_eventListener);
            _isListenerRegisted = true;
        }
    }


    internal class  UnityEventSource<T> : IValueSource<T>
    {
        private static Stack<UnityEventSource<T>> _pool = new Stack<UnityEventSource<T>>();

        internal static UnityEventSource<T> Request(UnityEvent<T> unityEvent){
            UnityEventSource<T> source = null;
            if(_pool.Count == 0){
                source = new UnityEventSource<T>();
            }else{
                source = _pool.Pop();
            }
            source.Initialize(unityEvent);
            return source;
        }

        private static uint _globalToken = 0;
        private static uint AllocateToken(){
            do{
                _globalToken ++;
            }while(_globalToken == 0);
            return _globalToken;
        }

        public uint Token{
            get;private set;
        }

        private SourceStatus _status;
        private UnityEvent<T> _unityEvent;
        private Action _continuation;
        private UnityAction<T> _eventListener;
        private T _result;

        private bool _isListenerRegisted = false;

        private UnityEventSource(){
            _eventListener = (T result)=>{
                _status = SourceStatus.Success;
                _result = result;
                UnregisterListener();
                _continuation();
            };
        }

        private void UnregisterListener(){
            if(_isListenerRegisted && _unityEvent != null){
                _unityEvent.RemoveListener(_eventListener);
                _isListenerRegisted = false;
            }            
        }
        public void Initialize(UnityEvent<T> unityEvent){
            _status = SourceStatus.Pending;
            _unityEvent = unityEvent;
            Token = AllocateToken();
        }

        private void Dispose(){
            if(Token == 0){
                throw new ObjectDisposedException(this.GetType().Name);
            }
            UnregisterListener();
            Token = 0;
            _continuation = null;
            _result = default(T);
            _pool.Push(this);
        }


        public T GetResult()
        {
            try{
                return _result;
            }finally{
                Dispose();
            }
        }

        public SourceStatus GetStatus()
        {
            return _status;
        }

        public void OnComplete(Action continuation)
        {
            _continuation = continuation;
            _unityEvent.AddListener(_eventListener);
            _isListenerRegisted = true;
        }
    }
}
