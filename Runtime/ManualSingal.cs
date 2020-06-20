using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace MS.Async{

    using CompilerServices;

    public class ManualSingal:IValueSource
    {   
        private List<Action> _completeActions = new List<Action>();
        private uint _token = 0;
        private SourceStatus _status;
        private Exception _exception;

        public ManualSingal(){
            Reset();
        }

        /// <summary>
        /// 重置信号量。所有处于等待改信号量的异步操作都会抛出OperationCanceledException异常
        /// </summary>
        public void Reset(){
            _status = SourceStatus.Pending;
            _token = AllocateId();
            _exception = null;
            Complete();
        }

        private void Complete(){
            try{
                foreach(var action in _completeActions){
                    action();
                }
            }finally{
                _completeActions.Clear();
            }
        }


        public uint Token => _token;
        public void GetResult()
        {
            
            if(_status == SourceStatus.Canceled){
                throw new OperationCanceledException();
            }else if(_status == SourceStatus.Faulted){
                throw new AggregateException(_exception);
            }
        }

        public SourceStatus GetStatus()
        {
            return _status;
        }

        private void AssertPending(){
            if(_status != SourceStatus.Pending){
                throw new InvalidOperationException();
            }
        }

        public void SetResult(){
            AssertPending();
            _status = SourceStatus.Success;
            Complete();
        }

        public void Cancel(){
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

        public SourceAwaiter GetAwaiter(){
            return new SourceAwaiter(this);
        }

        private static uint _allocateId = 0;
        
        private static uint AllocateId(){
            do{
                _allocateId ++;
            }while(_allocateId == 0);
            return _allocateId;
        }


    }


}
