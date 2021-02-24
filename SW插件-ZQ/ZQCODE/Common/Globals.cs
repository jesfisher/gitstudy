using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Data;
using System.Net;
using System.Linq;

namespace ZQCode
{
    /// <summary>
    /// 公共单元
    /// </summary>
    public class Globals
    {
        #region 全局变量
        //图号主要属性值
        public static DrawingModel CurrentDrawing = new DrawingModel();//当前图号对象
        public static MaterialModel CurrentMaterial = new MaterialModel();//当前物料编码对象
        public static string FileName = string.Empty;//文件名称（含路径）  
        public static string FileID = string.Empty;//文件ID
        public static string ConfigName = string.Empty;//当前激活配置名
        public static string FileIDDrawing = string.Empty;//工程图文件ID
        public static string MaterialID = string.Empty;//物料编码ID
        public static int RequestStyle = 0;//RequestStyle（1-新图号生成；2-旧图号保持）
        //【暂时不用】集合中包含3个值：DrawingId，DrawingClassId，RequestStyle（1-新图号生成；2-旧图号保持；3-新规格生成）
        //public static List<string> RequestList = new List<string>();//综合查询及判断返回值
        public static string PDM_UserID = string.Empty;// PDMHelper.GetLoginID();//用户ID
        public static string PDM_UserAccount = string.Empty;// PDMHelper.GetLoginUserName();//登录账号
        public static string PDM_UserFullName = string.Empty;// PDMHelper.GetLoginFullName();//用户全名（FullName）
        public static string DEF_CreateId = string.Empty;// PDMHelper.GetLoginUserName();//创建者ID(登录账号)
        public static string DEF_CreateUser = string.Empty;// PDMHelper.GetLoginFullName();//创建者名字(全名)
        public static string DEF_CreateInfo = GetLoginPCInfo();//创建补充信息

        //插件版本
        public static string swCodeVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //【本机】数据库连接字符串+pdm账号
        //public const string DbServerIP = "127.0.0.1";
        //public const string connStrPDM = "Server=.;Database=PDM;Uid=sa;Pwd=jesinfor";
        //public const string connStrGroupDBName = "Server=.;Database=OBOT_Data;Uid=sa;Pwd=jesinfor";
        //public const string connStrFactoryDBName = "Server=.;Database=OBOT_XKNC;Uid=sa;Pwd=jesinfor";
        //public const string FactoryCode = "1000";
        //public const string VaultName = "PDM";
        //public const string LogName = "admin";
        //public const string PWD = "";
        //【ZQ】数据库连接字符串+pdm账号
        public const string DbServerIP = "10.10.10.33";
        public const string connStrPDM = "Server=10.10.10.33;Database=ZQPDM;Uid=sa;Pwd=xknc@2017";
        public const string connStrGroupDBName = "Server=10.10.1.30;Database=OBOT_Data;Uid=sa;Pwd=Xknc@2017";
        public const string connStrFactoryDBName = "Server=10.10.1.30;Database=OBOT_ZQ;Uid=sa;Pwd=Xknc@2017";
        public const string FactoryCode = "1000";
        public const string VaultName = "ZQPDM";
        public const string LogName = "adm";
        public const string PWD = "123";
        #endregion

        public const string DEF_PROGRAM_VERSION = "V4.0-20201203";
        public const string DEF_PROGRAM_NAME = "图号申请插件【0114】" + DEF_PROGRAM_VERSION;
        public const string DEF_DATE_FORMAT = "yyyy-MM-dd";// "dd/MM/yyyy";
        public const string DEF_LONE_DATE_FORMAT = "yyyy-MM-dd hh:mm:ss";//"dd/MM/yyyy hh:mm:ss";
        public const string DEF_YYYYMMDD = "yyyyMMdd";
        public const string DEF_YYYYMMDD_LONG = "yyyy-MM-dd";//"yyyy/MM/dd";
        public const string DEF_DATE_LONG_FORMAT = "yyyy-MM-dd hh:mm:ss ";//"yyyy/MM/dd hh:mm:ss";                
        public const string DEF_NULL_DATETIME = "1900-1-1";
        public const string DEF_NULL_VALUE = "NULL";
        public const string DEF_CURRENCY = "RMB";//预设货币
        public const string DEF_DECIMAL_FORMAT = "0.00"; //输出格式        
        public const string DEF_NO_TEXT = "*自动生成*";

        /// <summary>
        /// 系统数据库名。开发框架2.2版支持多帐套管理，帐套表定义在系统数据库。
        /// 打开登录窗体时加载帐套数据给用户选择。        
        /// 
        /// 帐套数据由系统管理员在后台配置，不提供配置窗体。
        /// </summary>
        public const string DEF_SYSTEM_DB = "GF_System";
        public const string DEF_MASTER_DB = "master";

        public const int DEF_DECIMAL_ROUND = 2;//四舍五入小数位

        /// <summary>
        /// 加载Debug\Images目录下的的图片
        /// </summary>
        /// <param name="imgFileName">文件名</param>
        /// <returns></returns>
        public static Image LoadImage(string imgFileName)
        {
            string file = Application.StartupPath + @"\images\" + imgFileName;
            if (File.Exists(file))
                return Image.FromFile(file);
            else
                return null;
        }

        /// <summary>
        /// 加载Debug\Images目录下的的图片
        /// </summary>
        /// <param name="imgFileName">文件名</param>
        /// <returns></returns>
        public static Bitmap LoadBitmap(string imgFileName)
        {
            string file = Application.StartupPath + @"\images\" + imgFileName;

            if (File.Exists(file))
                return new Bitmap(Bitmap.FromFile(file));
            else
                return null;
        }

        /// <summary>
        /// 移除SQL注入非法字符
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <param name="returnMaxLength">返回的长度，0长度为不处理．</param>
        /// <returns></returns>
        public static string RemoveInjection(string content, int returnMaxLength)
        {
            string replaced = content.Replace("'", "").Replace("@", "").Replace("0x", "");
            if (returnMaxLength == 0)
                return replaced;
            else
                return replaced.Substring(0, replaced.Length < returnMaxLength ? replaced.Length : returnMaxLength);
        }

        /// <summary>
        /// 获取登录计算机信息
        /// </summary>
        /// <returns>计算机名-IP地址</returns>
        public static string GetLoginPCInfo()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];
            return string.Format("{0}-{1}", hostName, localaddr.ToString());
        }

        /// <summary>
        /// 最大流水码获取的方法，自动补齐缺位（如列表为1,2,4，则流水码为3）
        /// </summary>
        /// <param name="flowNoList">历史流水码整数集合</param>
        /// <param name="startNo">起始码0或1</param>
        /// <param name="bitNum">流水码位数</param>
        /// <returns></returns>
        public static string GetFlowNo(List<int> flowNoList, int startNo = 1, int bitNum = 4)
        {
            string flowNo = string.Empty;
            int n = startNo;
            if (flowNoList.Count > 0)
            {
                //默认是最大值 +1
                n = flowNoList.Max() + 1;
                for (int i = startNo; i < flowNoList.Max(); i++)
                {
                    //如果有中断的码，则赋值给n，并退出循环
                    if (!flowNoList.Contains(i))
                    {
                        n = i;
                        break;
                    }
                }
            }
            //一句话的代码（4位数，前面不足的用'0'补齐，如"01"→"0001"）
            flowNo = n.ToString().PadLeft(bitNum, '0');
            return flowNo;
        }
    }
}
