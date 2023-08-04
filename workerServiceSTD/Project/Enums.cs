using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Project.Enums {

    public enum ENUM_PayloadType {
        PT_PCMU = 0,
        PT_GSM = 3,
        PT_G723 = 4,
        PT_LPC = 7,
        PT_PCMA = 8,
        PT_G722 = 9,        
        PT_L16_ST = 10,
        PT_L16_MONO = 11,
        PT_G729 = 18
    }

    // IP 的傳送位置
    public enum ENUM_IPDir {
        [Description("未知")]
        Unknown = 0,

        [Description("發送端")]
        Send = 1,   

        [Description("接收端")]
        Recv = 2      
    }

    // IP通訊類型
    public enum ENUM_IPType {        
        TCP = 1,
        UDP = 2
    }    
    
    public enum ENUM_CallDirection {
        Unknown = -1,
        Outbound = 0,
        Inbound = 1
    }

    public enum ENUM_SipStatus {
        Idle = 0,
        Ivite = 1,
        Ringing = 2,
        Talking = 3,
        Hold = 4
    }
        
    public enum ENUM_LineStatus {
        [Description("線路錯誤")]
        Failed = -1,

        [Description("閒置")]
        Idle = 0,

        [Description("響鈴")]
        Ring = 1,

        [Description("撥出")]
        Inbound = 2,

        [Description("外撥")]
        Outbound = 3,

        [Description("內線通話")]
        Intercom = 4
    }

    public enum ENUM_SqlLogType : int {
        [Description("SqlTrace")]
        Trace = 0,

        [Description("SqlError")]
        Error = 1
    }

    public enum ENUM_LogType {
        [Description("訊息")]
        Info = 1,
        [Description("告警")]
        Alarm = 2,
        [Description("錯誤")]
        Error = 3,
        [Description("嚴重")]
        Fatal = 4
    }  
        
}