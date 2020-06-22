using System.Runtime.CompilerServices;
using System;
using System.Threading.Tasks;

namespace MS.Async.CompilerServices{
    public struct BackgroundThreadAwaiter : ICriticalNotifyCompletion
    {

        private BackgroundThreadType _type;
        public BackgroundThreadAwaiter(BackgroundThreadType type){
            _type = type;
        }

        public void GetResult(){
        }

        public bool IsCompleted{
            get{
                if(_type == BackgroundThreadType.SwitchAnyway){
                    return false;
                }else if(_type == BackgroundThreadType.SwitchOnlyInUnityThread){
                    return !UnityThreadContextCapture.IsUnityThread();
                }else{
                    throw new ArgumentException();
                }
            }
        }

        public void OnCompleted (Action continuation){
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted (Action continuation){
            if(_type == BackgroundThreadType.SwitchAnyway){
                Task.Run(continuation);
            }else if(_type == BackgroundThreadType.SwitchOnlyInUnityThread){
                if(!UnityThreadContextCapture.IsUnityThread()){
                    continuation();
                }else{
                    Task.Run(continuation);
                }
            }else{
                throw new ArgumentException();
            }
        }     
    }
}
