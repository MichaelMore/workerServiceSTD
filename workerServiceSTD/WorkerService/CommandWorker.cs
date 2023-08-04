using Project.Database;
using Project.Helper;
using Project.AppSetting;
using Project.ProjectCtrl;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Fluent;
using Project.Lib;
using System.Net.Sockets;
using System.Net;
using WorkerThread;
using Project.Models;
using ThreadWorker;

namespace Project.WorkerService {
    class CommandWorker : BaseWorker {

        private UdpListener _udpServer = null;
        private IPEndPoint _serverEndPoint = null;
        private Socket _sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public override string className => GetType().Name;

        public CommandWorker(HttpClientHelper httpClientHelper, IHostApplicationLifetime hostLifeTime) : base(httpClientHelper, hostLifeTime) {
            //初始化nlog
            nlog = LogManager.GetLogger(className);
        }

        private bool Init() {
            nlog.Info($"\t try to bind Command(UDP) listening ...(port={GlobalVar.AppSettings.UDPCommandPort})");
            var ret = true;
            _serverEndPoint = lib_misc.GetIpEndPoint(IPAddress.Any, GlobalVar.AppSettings.UDPCommandPort, out string err);
            if (_serverEndPoint == null) {
                nlog.Info($"\t\t IPEndPoint error: {err}");
                ret = false;
            }
            else {
                try {
                    _udpServer = new UdpListener(_serverEndPoint);
                }
                catch (Exception ex) {
                    nlog.Info($"\t\t Command(UDP) listener raised an exception: {ex.Message}");
                    ret = false;
                }
            }
            if (ret)
                nlog.Info($"\t Command(UDP) server bind ok({_serverEndPoint})");

            return ret;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            //必須給定前置時間給 Batch 檔啟動服務,不然console會判定service啟動錯誤
            await Task.Delay(GlobalVar.AppSettings.WorkerOption.DelayBefroreExecute * 1000, stoppingToken);

            nlog.Info($"{className} ExecuteAsync starting ...");
            if (!Init()) {
                nlog.Info("********** process init failed. Job terminated. **********");                
                return;
            }

            //DateTime checkTime = DateTime.MinValue; // 故意讓流程一開始要先跑
            DateTime checkTime = DateTime.Now;            

            while (!stoppingToken.IsCancellationRequested) {                

                // 等待時間是否已到
                if (!TimeIsUp(1, ref checkTime)) {
                    await Task.Delay(50, stoppingToken);
                    continue;
                }
                nlog.Info($"");
                nlog.Info($"");
                try {
                    // worker 會停在此處等 UDP 封包
                    // TODO: 可以改成 Task 的方式
                    #region Do your work
                    await DoJob(); 
                    #endregion
                }
                catch (Exception ex) {
                    nlog.Info($"執行工作發生錯誤：{ex.Message}");
                }                
                await Task.Delay(100, stoppingToken);
            }
        }

        private async Task DoJob() {
            try {
                var received = await _udpServer.Receive();
                nlog.Info("\r\n\r\n");
                nlog.Info($"received data from {received.Sender}, length={received.DataLen}");
                if (string.IsNullOrEmpty(received.Message.Trim()))
                    return;

                // 這邊的 LoggerCommandModel 只是舉例 ...                
                //var model = GetCommandModel<LoggerCommandModel>(received.Message);
                //if (model == null)
                //    return;

                //nlog.Info($"===> Get command:\r\n{JsonConvert.SerializeObject(model, Formatting.Indented)}");                
                //ProcessCommand(model);                
            }
            catch (Exception ex) {
                nlog.Info($"process udp packet raise an exception: {ex.Message}");                
            }
        }


        // 暫時假設是 LoggerCommandModel
        private T GetCommandModel<T>(string jsonStr) {

            T model = default;
            try {
                model = JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch (Exception ex) {                
                nlog.Info($"\t json to model<T> parsing exception: {ex.Message}");
            }
            return model;
        }

        
        private void ProcessCommand<T>(T model) {

            //if (string.IsNullOrEmpty(model.ExtNo))
            //    return;
            
            try {
                // 根據 model.Command 內容處理...
            }
            catch (Exception ex) {
                nlog.Info($"ProcessCommand raised exception: {ex.Message}");
            }            
        }
    }
}
