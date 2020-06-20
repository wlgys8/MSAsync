using System.Runtime.CompilerServices;
using System;

namespace MS.Async.CompilerServices{

    public struct SourceAwaiter:ICriticalNotifyCompletion{
        private IValueSource _source;
        private uint _token;

        public SourceAwaiter(IValueSource source){
            _source = source;
            _token = source.Token;
        }
        public void GetResult(){
            _source.GetResult();
        }

        public bool IsCompleted{
            get{
                return _source.GetStatus() != SourceStatus.Pending;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            _source.OnComplete(continuation);
        }
    }


    public interface IValueSource{


        void OnComplete(Action continuation);

        void GetResult();

        SourceStatus GetStatus();

        uint Token{
            get;
        }

    }

    public enum SourceStatus{
        Pending,
        Success,
        Canceled,
        Faulted,
    }
}
