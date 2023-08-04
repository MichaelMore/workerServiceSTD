using Project.Database;
using Project.Helper;
using Project.Models;
using Project.ProjectCtrl;
using NLog;
using ThreadWorker;
using WorkerThread;
using Project.Lib;
using Project.Enums;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Project.AppSetting;
using System.Text;

namespace Project.WorkerService {
    class MainWorker : BaseWorker {

        public override string className => GetType().Name;
        //protected DemoDb db => new DemoDb();        

        public MainWorker(HttpClientHelper httpClientHelper, IHostApplicationLifetime hostLifeTime) : base(httpClientHelper, hostLifeTime) {
            //初始化nlog
            nlog = LogManager.GetLogger("System");
        }
        

        //TODO: 在 API 提供增加/停止分機錄音的功能，方便變更系統設定，而不需要將錄音核心重啟!!!

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            //必須給定前置時間給 Batch 檔啟動服務,不然console會判定service啟動錯誤
            await Task.Delay(GlobalVar.AppSettings.WorkerOption.DelayBefroreExecute * 1000, stoppingToken);

            if (!CheckSystemSettings(out string errMsg)) {
                nlog.Error($"\t {errMsg}");                
                return;
            }   
            
            nlog.Info($"{GlobalVar.ProjectName} MainWorker.ExecuteAsync starting ...");                       

            //DateTime checkTime = DateTime.MinValue; // 故意讓流程一開始要先跑            
            var demoStr = GlobalVar.LicenseModel.DemoExpired.HasValue ? $"Demo版本:{GlobalVar.LicenseModel.DemoExpired.Value.ToString("yyyy-MM-dd")}" : "正式版";            
                        
            while (!stoppingToken.IsCancellationRequested) {
                /*
                 * 此處可以每隔一段時間進行檢查或處理 ..... 等。
                 * 例如: 監測、計算、reset、...
                 */                
                await Task.Delay(1000, stoppingToken);                
            }

            // 通知 ReadThread 結束
            //foreach (var thd in listReadThread) {
            //    thd.RequestStop();
            //}

            //// 通知 ParseThread 結束
            //foreach (var thd in listParseThread) {
            //    thd.RequestStop();
            //}

            // 此處也必須等待所有的 Tnread 都離開。
        }        

        private bool CheckSystemSettings(out string checkErr) {
            checkErr = "";
            if (!GlobalVar.CheckLicense(out string licErr)) {
                checkErr = $"{licErr}, 服務停止!";                                
                CreateLicenceKey();
                return false;
            }

            if (GlobalVar.LicenseModel.SynipPort == 0) {
                checkErr = $"無錄音授權, 服務停止!";                
                CreateLicenceKey();
                return false;
            }

            #region 檢查 appsettings，設定有錯誤則 ExecuteAsync return，服務不繼續，應該會停止
            //if (!CheckAndCreateRecFolder(out string err)) {
            //    checkErr = $"{err}, 服務停止!";                
            //    return false;
            //}

            //if (lib_misc.IsNullOrEmpty(GlobalVar.AppSettings.Monitor.Device)) {
            //    checkErr = $"監控設備(appsettings.Monitor.Device)設定錯誤, 服務停止!";             
            //    return false;
            //}
            //var sipProto = new List<string>() { "tcp", "udp" };
            //if (!sipProto.Contains(GlobalVar.AppSettings.Monitor.SipProtocol.ToLower())) {
            //    checkErr = $"監控設備(appsettings.Monitor.SipProtocol)設定錯誤(tcp/udp), 服務停止!";                
            //    return false;
            //}
            //var monType = new List<string>() { "ip", "mac" };
            //if (!monType.Contains(GlobalVar.AppSettings.Monitor.MonType.ToLower())) {
            //    checkErr = $"監控設備(appsettings.Monitor.MonType)設定錯誤(ip/mac), 服務停止!";                
            //    return false;
            //}            

            //#region 矯正 AppSettings 中 MAC 的格式, 從 PCAP 封包取得的 MAC Address = 6C5E3B87C0BD，而且大寫
            //foreach (var mon in GlobalVar.AppSettings.Monitor.Device) {
            //    if (!string.IsNullOrEmpty(mon.MacAddr)) {
            //        mon.MacAddr = mon.MacAddr.Replace("-", "").Replace(":", "").ToUpper(); // 去除 - : 並大寫
            //    }
            //}
            #endregion

            return true;

        }

        private void CreateLicenceKey() {

            var key = lib_license.GetLicenceKey(out var err);
            if (string.IsNullOrEmpty(key)) {
                nlog.Error($"failed to create license key: {err}");
                return;
            }
            
            var fileName = Path.Combine(GlobalVar.CurrentExePath, "ivrMichelle.key");
            try {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName)) {
                    file.WriteLine(key); 
                }
            }
            catch (Exception ex) {
                nlog.Error($"*** 無法產生 license key 檔案({fileName}): {ex.Message}");
            }
            return;

        }


    }
}
