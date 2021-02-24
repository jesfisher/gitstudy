using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Configuration;
namespace SwCSharpAddin1
{
    public class ConfigSet
    {
        //private Configuration config = ConfigurationManager.OpenExeConfiguration(@"D:\0.Working\【01】客户项目\G.共享集团\code\01共享辅机编码\V2-20170726\01共享辅机编码\SWaddin\SwCSharpAddin1\bin\Debug\KMAcode.dll");
        private Configuration config = ConfigurationManager.OpenExeConfiguration(@"C:\KMA模板\KMAcode\KMAcode.dll");

        public List<string> GetConfig()
        {
            List<string> defaultCode = new List<string>();
            object o1 = config.AppSettings.Settings["ProductCode"].Value;
            object o2 = config.AppSettings.Settings["ImageCode"].Value;
            defaultCode.Add(o1 == null ? "" : o1.ToString());
            defaultCode.Add(o2 == null ? "" : o2.ToString());
            return defaultCode;
        }
        public void SetConfig(List<string> defaultCode)
        {
            config.AppSettings.Settings["ProductCode"].Value = defaultCode[0];
            config.AppSettings.Settings["ImageCode"].Value = defaultCode[1];
            config.Save();
        }
    }
}

