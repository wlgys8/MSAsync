using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Async{
    using System;
    using CompilerServices;
    public class Locker:IValueSource
    {
        private List<Action> _continuations = new List<Action>();

        public uint Token => 0;
        private SourceStatus _status = SourceStatus.Succeeded;

        public void GetResult()
        {
        }

        public SourceStatus GetStatus()
        {
            return _status;
        }

        public void Lock(){
            _status = SourceStatus.Pending;
        }

        public void OnComplete(Action continuation)
        {
            _continuations.Add(continuation);
        }

        public void Release(){
            if(_continuations.Count == 0){
                _status = SourceStatus.Succeeded;
            }else{
                do{
                    var continuation = _continuations[0];
                    _continuations.RemoveAt(0);
                    continuation();
                }while(_status != SourceStatus.Pending && _continuations.Count > 0);
                if(_continuations.Count == 0){
                    _status = SourceStatus.Succeeded;
                }
            }
        }

        public SourceAwaiter GetAwaiter(){
            return new SourceAwaiter(this);
        }
    }
}
