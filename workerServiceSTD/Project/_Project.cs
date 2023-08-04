using System.Reflection;
using System.Net.Sockets;
using System.Net;
using NLog;
using Project.Models;
using Project.AppSetting;
using SharpPcap;
using ThreadWorker;
using Project.Lib;
using System.Text;
using Project.Enums;

namespace Project.ProjectCtrl {

    //專案資訊
    public static class GlobalVar {

        //專案名稱
        public const string ServiceName = "ivrMichelle";
        public const string ServiceName_Ch = "語音互動核心Michelle";

        // 資料庫加密的 Key & iv
        public const string ProjectName = "ivrMichelle";
        public const string DBAesKey = "550102mkt" + "42751171@richpod.com.tw"; // 32 個英文或數字        
        public const string DBAesIV = "0955502123ASDzxc"; // 16 個英文或數字                        
        //        
        public static Logger nlog = LogManager.GetLogger("Startup");

        public static string LicenseFile { private set; get; } = "";
        public static string LocalIP { private set; get; } = "";
        public static AppSettings AppSettings { private set; get; }
        public static IConfiguration Configuration { private set; get; }
        
        
        public static string FFMpegExeFileName { private set; get; } = "";
        public static string SoxExeFileName { private set; get; } = "";

        internal static LicRegisterEx2Model LicenseModel = null;

        // 建構子
        static GlobalVar() {
            // 取得 FFMpegExeFileName 位置
            var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            FFMpegExeFileName = Path.Combine(exePath, "3rdParty","ffmpeg.exe");
            SoxExeFileName = Path.Combine(exePath, "3rdParty", "sox", "sox.exe");

            // to get local ip address string                        
            try {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                    socket.Connect("8.8.8.8", 65530);                    
                    IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                    LocalIP = (endPoint == null) ? "" : endPoint.Address.ToString();                                        
                }
            }
            catch (Exception ex) {
                nlog.Error($"failed to get LocalIP address: {ex.Message}");
                LocalIP = "";
            }
        }     

        public static void SetConfiguration(IConfiguration config) {
            Configuration = config;
            AppSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(AppSettings);
        }

        public static string CurrentExePath {
            get {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }


        public static string CurrentVersion {
            get {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                if (version != null) {
                    return $"{version.Major}.{version.Minor}.{version.Build} build {version.Revision}";
                }
                else {
                    return $"getVersionError";
                }                
            }
        }

        public static bool CheckLicense(out string err) {
            err = "";            
            if (LicenseModel == null || LicenseModel.SynipPort == 0) {
                err = "no license";
                return false;
            }
            if (GlobalVar.LicenseModel.DemoExpired.HasValue) {
                if ((DateTime.Now - GlobalVar.LicenseModel.DemoExpired.Value).TotalSeconds > 0) {
                    err = $"demo license expired: {GlobalVar.LicenseModel.DemoExpired.Value.ToString("yyyy-MM-dd")}";
                    return false;
                }
            }
            return true;
        }
        
    }
}
