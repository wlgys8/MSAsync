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

        private void AssertTokenNotExpired(){
            if(_token != _source.Token){
                throw new InvalidOperationException("Token expired");
            }
        }

        public void GetResult(){
            AssertTokenNotExpired();
            _source.GetResult();
        }

        public bool IsCompleted{
            get{
                AssertTokenNotExpired();
                return _source.GetStatus() != SourceStatus.Pending;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            AssertTokenNotExpired();
            _source.OnComplete(continuation);
        }
    }

    public struct SourceAwaiter<T>:ICriticalNotifyCompletion{
        private IValueSource<T> _source;
        private uint _token;

        public SourceAwaiter(IValueSource<T> source){
            _source = source;
            _token = source.Token;
        }
        private void AssertToken(){
            if(_token != _source.Token){
                throw new InvalidOperationException("Token expired");
            }
        }

        public T GetResult(){
            AssertToken();
            return _source.GetResult();
        }

        public bool IsCompleted{
            get{
                AssertToken();
                return _source.GetStatus() != SourceStatus.Pending;
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            AssertToken();
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

    public interface IValueSource<T>{

        void OnComplete(Action continuation);

        T GetResult();

        SourceStatus GetStatus();

        uint Token{
            get;
        }
    }

    public enum SourceStatus{
        Pending,
        Succeeded,
        Canceled,
        Faulted,
    }
}
