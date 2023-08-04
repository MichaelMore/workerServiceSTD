using Project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AppSetting {
    public class AppSettings {
        public string ServerID { get; set; }
        public string AppID { get; set; }
        public string LicenseFile { get; set; }
        public int UDPCommandPort { get; set; }
        public AppSettings_DBConnection DBConnection { get; set; }
        public AppSettings_WorkerOption WorkerOption { get; set; }
        public AppSettings_WebAPI WebAPI { get; set; }
    }

    public class AppSettings_DBConnection {
        public string DBName { get; set; }
        public string SchemaName { get; set; }
        public string MainDBConnStr { get; set; }
        public int DBConnectTimeout { get; set; } = 60;
        public bool SqlTrace { get; set; } = false;

        // Contructure
        public AppSettings_DBConnection() {
            DBConnectTimeout = 60;
        }
    }

    public class AppSettings_WorkerOption {
        public int DelayBefroreExecute { get; set; }
        public bool UpdateModuleStatus { get; set; } = false;
        public int UpdateModuleStatusIntervalMin { get; set; } = 5;
        public int LoopIntervalMSec { get; set; } = 100; // default = 100ms
        public int ProcessIntervalSec { get; set; } = 20; // default = 20s        
    }

    public class AppSettings_WebAPI {
        public string ApiAccount { get; set; } = "";
        public string ApiPassword { get; set; } = "";
        public string GetToken { get; set; } = "";
        public string UpdateChannelStatus { get; set; } = "";
        public string NotifyChannelStatus { get; set; } = "";
        public string WriteRecData { get; set; } = "";
        public string WriteSystemLog { get; set; } = "";
    }



}
