using System.Collections.Generic;
using System;

namespace MS.Async{
    using CompilerServices;



    public class AutoResetSingal<T> : IValueSource<T>
    {
        private List<Action> _completeActions = new List<Action>();
        private SourceStatus _status;
        private Exception _exception;
        private T _result;

        public AutoResetSingal(){
            Reset();
        }

        private void Reset(){
            _status = SourceStatus.Pending;
            _exception = null;
            _result = default(T);
        }

        private void Complete(){
            try{
                var count = _completeActions.Count;
                var index = 0;
                while(index < count){
                    var action = _completeActions[index];
                    _completeActions.RemoveAt(index);
                    action();
                    index ++;
                }
            }finally{
                Reset();
            }
        }


        public uint Token => 0;

        public T GetResult()
        {
            if(_status == SourceStatus.Canceled){
                throw new OperationCanceledException();
            }else if(_status == SourceStatus.Faulted){
                throw new AggregateException(_exception);
            }
            return _result;
        }

        /// <summary>
        /// Pending forever
        /// </summary>
        public SourceStatus GetStatus()
        {
            return SourceStatus.Pending;
        }

        private void AssertPending(){
            if(_status != SourceStatus.Pending){
                throw new InvalidOperationException();
            }
        }

        public void SetResult(T result){
            AssertPending();
            _status = SourceStatus.Succeeded;
            _result = result;
            Complete();
        }

        public void SetCanceled(){
            AssertPending();
            _status = SourceStatus.Canceled;
            Complete();
        }

        public void SetException(Exception e){
            AssertPending();
            _status = SourceStatus.Faulted;
            _exception = e;
            Complete();
        }

        public void OnComplete(Action action){
            _completeActions.Add(action);
        }

        public SourceAwaiter<T> GetAwaiter(){
            return new SourceAwaiter<T>(this);
        }
 
    }
}
