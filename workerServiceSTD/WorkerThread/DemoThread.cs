using Newtonsoft.Json;
using NLog;
using Project.AppSetting;
using Project.Enums;
using Project.Helpers;
using Project.Models;
using Project.ProjectCtrl;
using WorkerThread;

namespace ThreadWorker {
    public class DemoThread: _IWorker
    {
        // protect
        protected volatile bool _shouldStop;
        protected volatile bool _shouldPause;

        // private
        private string _tag;
        private Thread _myThread;
        private NLog.Logger _log;        

        // public        
        public WorkerState State { get; internal set; }               

        public DemoThread() {            
            _tag = "Demo";
            _log = LogManager.GetLogger($"{_tag}-XXX");
        }

        public void StartThread() {
            _myThread = new Thread(this.DoWork) {
                IsBackground = true,
                Name = _tag
            };
            State = WorkerState.Starting;
            _myThread.Start();
        }

        public void StopThread() {
            RequestStop(); // stopping ...
            _log.Info($"{_tag} is waiting to stop(join) ...");
            _myThread.Join();
            State = WorkerState.Stopped; // stopped !!!
        }


        // 這個 Thread 只有 1 個，專門處理封包
        public virtual void DoWork(object anObject) {
            _log.Info("");
            _log.Info($"********** {_tag} Thread is now starting ... **********");
            State = WorkerState.Running;
            while (!_shouldStop) {       
                // 此處不要使用 thread.sleep...，要使用 WaitOne 的方法，以提升程式效能
                //GlobalVar.WaitPacketComing.WaitOne(); // <= 注意這裡的寫法

                //// 關於 GetDispatchPacket:
                ////      1. 如果Queue沒有封包，WaitPacketComing 會 reset，這裡的 while loop 會卡在 WaitOne()
                ////      2. 如果Queue有封包，WaitPacketComing 會 set，WaitOne() 會跳出、往下跑
                //var packetInfo = GlobalVar.GetDispatchPacket(); 
                //if (packetInfo != null) {
                //    ProcessPacketInfo(packetInfo);
                //}
            }
            _log.Info($"========== {_tag} Thread terminated. ==========");
            State = WorkerState.Stopped;
        }        
        

        public void RequestStop() {
            _log.Info($"{_tag} is requested to stop ...");
            State = WorkerState.Stopping;
            _shouldStop = true;
        }       

        
    }

}
