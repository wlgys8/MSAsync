using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace MS.Async{
    internal class UnityLoops : MonoBehaviour
    {
        private static List<YieldTask> _yieldTasks = new List<YieldTask>();
        private static List<TimeTask> _timeTasks = new List<TimeTask>();

        private static List<KeyInputTask> _keyInputTasks = new List<KeyInputTask>();

        private static List<Action> _updateActions = new List<Action>();

        private static IEnumerator WaitYieldInstruction(YieldInstruction instruction,Action<object> callback,object state){
            yield return instruction;
            callback(state);
        }


        public static void ScheduleYieldTask(YieldTask task){
            if(Instance != null){
                Instance.StartCoroutine(WaitYieldInstruction(task.instruction,task.callback,task.state));
            }else{
                _yieldTasks.Add(task);
            }
        }

        /// <summary>
        /// 在指定时间执行Action
        /// </summary>
        public static void ScheduleTimeTask(DateTime targetTime,Action<object> action,object state){
            if(DateTime.Now >= targetTime){
                action(state);
            }else{
                var task =  new TimeTask(){
                    targetTime = targetTime,
                    action = action,
                    state = state,
                };
                InsertTimeTaskOrdered(ref task);
            }
        }

        /// <summary>
        /// Schedule an action run in next update.
        /// 
        /// This method is thread safe
        /// </summary>
        public static void ScheduleUpdate(Action action){
            lock(_updateActions){
                _updateActions.Add(action);
            }
        }



        public static void ScheduleKeyInput(in KeyInputTask task){
            _keyInputTasks.Add(task);
        }

        private static void InsertTimeTaskOrdered(ref TimeTask task){
            if(_timeTasks.Count == 0){
                _timeTasks.Add(task);
                return;
            }
            var time = task.targetTime;
            var minTime = _timeTasks[0].targetTime;
            var maxTime = _timeTasks[_timeTasks.Count - 1].targetTime;
            if(time < minTime){
                _timeTasks.Insert(0,task);
            }else if(time >= maxTime){
                _timeTasks.Add(task);
            }else{
                var index = FindIndexToInsert(ref time,0,_timeTasks.Count);
                _timeTasks.Insert(index,task);
            }
        }

        /// <summary>
        /// when time >= minTime && time < maxTime, find the best index to insert
        /// </summary>
        private static int FindIndexToInsert(ref DateTime time,int minIndex,int maxIndex){
            var middleIndex = (minIndex + maxIndex) / 2;
            var middleTime = _timeTasks[minIndex].targetTime;
            if(time < middleTime){
                return FindIndexToInsert(ref time,minIndex,middleIndex);
            }else if(time > middleTime){
                if(middleIndex == minIndex){
                    return maxIndex;
                }
                return FindIndexToInsert(ref time,middleIndex,maxIndex);
            }else{
                //time == middleTime
                return middleIndex;
            }
        }

        public static int TimeTaskCount{
            get{
                return _timeTasks.Count;
            }
        }

        public static int YieldTaskCount{
            get{
                return _yieldTasks.Count;
            }
        }


        void Update(){

            lock(_updateActions){
                while(_updateActions.Count > 0){
                    var action = _updateActions[0];
                    try{
                        action();
                    }catch(System.Exception e){
                        Debug.LogException(e);
                    }
                    _updateActions.RemoveAt(0);
                }
            }

            var now = DateTime.Now;
            while(_timeTasks.Count > 0){
                var task = _timeTasks[0];
                if(now >= task.targetTime){
                    _timeTasks.RemoveAt(0);
                    task.action(task.state);
                }else{
                    break;
                }
            }

            if(Input.anyKey){
                var index = 0;
                while(index < _keyInputTasks.Count){
                     var task = _keyInputTasks[index];
                     if(task.IsActive){
                         task.action(task.state);
                         _keyInputTasks.RemoveAt(index);
                     }else{
                         index ++;
                     }
                }
            }

        }


        public struct YieldTask{
            public YieldInstruction instruction;
            public Action<object> callback;
            public object state;
        }

        public struct TimeTask{
            public DateTime targetTime;
            public Action<object> action;

            public object state;

        }

        public struct KeyInputTask{

            public KeyCode keyCode;
            public KeyInutType type;

            public Action<object> action;

            public object state;

            public bool IsActive{
                get{
                    switch(type){
                        case KeyInutType.Down:
                        return Input.GetKeyDown(keyCode);
                        case KeyInutType.Up:
                        return Input.GetKeyUp(keyCode);
                        case KeyInutType.Any:
                        return Input.GetKey(keyCode);
                    }
                    return false;
                }
            }
        }



        private static UnityLoops _instance;

        private static UnityLoops Instance{
            get{
                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Launch(){
            if(_instance == null){
                _instance = new GameObject("UnityLoops").AddComponent<UnityLoops>();
                UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
                foreach(var task in _yieldTasks){
                    ScheduleYieldTask(task);
                }
                _yieldTasks.Clear();
            }
        }
    }
}
