using Project;
using Project.Enums;
using Project.Helper;
using Project.AppSetting;
using Project.ProjectCtrl;
using Project.WorkerService;
using Newtonsoft.Json;
using NLog;

//�ϥ� default �� NLog.config
var nlog = LogManager.GetLogger("Startup");

try {
    IHost host = Host.CreateDefaultBuilder(args)
    // ���� nlog �ϥ� appsettings.json �Ӱ��]�w�A�H�ΨϥΪ`�J���覡�A
    // �аѦ�: https://stackoverflow.com/questions/69594537/nlog-setup-for-net5-worker-service-template
    // ���� ILogger/LoggerName�A���g�]�ܦn: https://stackoverflow.com/questions/67274039/nlog-write-to-multiple-logs-in-net-core-3-1    

    // �ϥ� nlog �`�J, �۰�Ū�� appsetting.json �� "NLog" Section <= ���O���@�w�n�ΡA�]��ı�o�ܳ·СA
    //.ConfigureLogging((hostContext, logging) => {
    //    logging.ClearProviders();
    //    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    //    logging.AddNLog(hostContext.Configuration, new NLog.Extensions.Logging.NLogProviderOptions() {
    //        LoggingConfigurationSectionName = "NLog"
    //    });
    //})

    // service �]�w
    .ConfigureServices((hostContext, services) => {        

        // ����ʰw�� NewtonSoft �� Json �ǦC�ƪ��󪺳]�w
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
            //Formatting = Formatting.Indented,
            //TypeNameHandling = TypeNameHandling.Objects,
            //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy/MM/dd HH:mm:ss.ffffff"
        };

        // Ū��
        nlog.Info("");
        nlog.Info("");
        nlog.Info("***********************************************");
        nlog.Info("********** program.ConfigureServices **********");
        nlog.Info($"\t {GlobalVar.ProjectName} starting .... version = {GlobalVar.CurrentVersion}");

        #region ���J AppSettings �� GlobalVar
        // AppSettings �� Project �Ϊ`�J���i�H�s��
        IConfiguration configuration = hostContext.Configuration;

        //AppSettings ��J Project.AppSettings
        nlog.Info("\t Project.SetConfiguration ...");
        GlobalVar.SetConfiguration(configuration);
        #endregion

        #region �`�JAppSettings
        nlog.Info("\t AppSetting �`�J ...");
        AppSettings settings = configuration.GetSection("AppSettings").Get<AppSettings>();
        // AppSettings�`�J
        services.AddSingleton(settings);
        nlog.Info($"\t\t ServerID={settings.ServerID}");
        nlog.Info($"\t\t AppID={settings.AppID}");
        //nlog.Info($"\t\t Loggings: \r\n{JsonConvert.SerializeObject(settings.Logging, Formatting.Indented)}");
        //nlog.Info($"\t\t Recording: \r\n{JsonConvert.SerializeObject(settings.Recording, Formatting.Indented)}");
        #endregion

        #region ���UIHttpClientFactory
        //���UIHttpClientFactory����{��DI�e��        
        nlog.Info("\t IHttpClientFactory �`�J ...");
        services.AddHttpClient(); // <= WEB ���Υ[�A�� console/service �n�[�A���M AddTransient �| error

        // �o�@�q�O�b�B�z¶�L SSL ��ñ�����D�A���٦����D... ����A�ݬݡC
        //services.AddHttpClient("myApi")0
        //        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
        //            ServerCertificateCustomValidationCallback = delegate { return true; }
        //        });

        services.AddTransient<HttpClientHelper>();
        #endregion


        #region ���UDemoWorker
        // �u���� worker service
        nlog.Info("\t �}�l���U�A�� ...");
        services.AddHostedService<MainWorker>();
        //services.AddHostedService<CommandWorker>();        
        #endregion
    })

    // �ϥ� Windows Service ���U
    .UseWindowsService()
    .Build();
    await host.RunAsync();
}
catch(Exception ex) {
    nlog.Error($"�t��(CreateDefaultBuilder)�Ұʿ��~: {ex.Message}");
}


