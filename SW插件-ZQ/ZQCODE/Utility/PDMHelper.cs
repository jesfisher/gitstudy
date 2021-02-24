using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPDM.Interop.epdm;
using EPDM.Interop.EPDMResultCode;
using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.IO;


namespace ZQCode
{
    public static class PDMHelper
    {
        public static IEdmVault5 vault = null;//声明包含登录对象
        public static string RootPath = string.Empty;//提取PDM根目录
        static string VaultName = Globals.VaultName;
        static string LogName = Globals.LogName;
        static string PWD = Globals.PWD;

        #region 视图与登录（Login）
        /// <summary>
        /// 获取当前的视图集合
        /// </summary>
        /// <param name="_dictionaryName">字典名称，例如：SWClient</param>
        /// <param name="_keyName">键名称，例如：SWAddinsClientVersion</param>
        /// <returns>视图集合</returns>
        public static string GetDictionary(string _dictionaryName, string _keyName)
        {
            IEdmVault5 vault5 = new EdmVault5();
            try
            {
                vault5.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            catch (Exception e)
            {
                throw e;
            }
            IEdmDictionary5 dic = vault5.GetDictionary(_dictionaryName, false);
            string value;
            dic.StringGetAt(_keyName, out value);
            //修改SOLIDWORKS插件版本信息
            //dic.StringSetAt(_keyName, "1.3.0.2");
            return value;
        }

        /// <summary>
        /// 【1】判断PDM是否登录,并返回根目录
        /// </summary>
        public static string LoginPDM()
        {
            EdmVault5 vault5 = new EdmVault5();
            if (!vault5.IsLoggedIn)
            {
                vault5.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            if (!vault5.IsLoggedIn)
            {
                MessageBox.Show("请先登录PDM系统！", "提示信息");
            }
            return RootPath = vault5.RootFolderPath.ToString();
        }

        //判断PDM是否登录,返回布尔值True/False
        public static bool IsLoginPDM()
        {
            //判断PDM是否登录
            try
            {
                EdmVault5 vault5 = new EdmVault5();
                if (!vault5.IsLoggedIn)
                {
                    vault5.LoginAuto(VaultName, new Control().Handle.ToInt32());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("PDM未登录或已注销，请重新登录！" + "\r\n" + ex.Message);
            }
        }
        /// <summary>
        /// 设置文件卡自由变量的值（文件ID）
        /// </summary>
        /// <param name="_filePath">文件ID</param>
        /// <param name="_value">自由变量名称，变量值</param>
        /// <returns></returns>
        public static bool SetFileCardVariableValue(int _fileID, Dictionary<string, string> _value)
        {
            try
            {
                IEdmVault5 vault5 = new EdmVault5();
                IEdmVault7 vault7 = (IEdmVault7)vault5;
                if (!vault5.IsLoggedIn)
                {
                    vault5.Login(LogName, PWD, VaultName);
                }
                IEdmBatchUpdate2 update = default(IEdmBatchUpdate2);
                update = (IEdmBatchUpdate2)vault7.CreateUtility(EdmUtility.EdmUtil_BatchUpdate);
                IEdmVariableMgr5 variableMgr5 = (IEdmVariableMgr5)vault5;
                foreach (var key in _value.Keys)
                {
                    //文件ID、变量ID、值、配置名称、         
                    update.SetVar(_fileID, variableMgr5.GetVariable(key).ID, _value[key], "@", (int)EdmBatchFlags.EdmBatch_AllConfigs);
                }
                EdmBatchError2[] Errors = null;
                update.CommitUpdate(out Errors, null);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 用户相关操作（User）

        //获取用户ID
        public static string GetLoginID()
        {
            IEdmVault5 vault5 = new EdmVault5();
            if (!vault5.IsLoggedIn)
            {
                vault5.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            IEdmVault11 valut11 = (IEdmVault11)vault5;
            return valut11.GetLoggedInWindowsUserID(VaultName).ToString();
        }

        //获取用户列表
        public static DataTable GetUserList()
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("UserName");
                dt.Columns.Add("UserID");
                dt.Columns.Add("IsLoggedIn");
                dt.Columns.Add("Vault");
                dt.Columns.Add("FullName");
                dt.Columns.Add("UserData");
                dt.Columns.Add("Initials");
                dt.Columns.Add("ObjectType");
                dt.Columns.Add("Email");
            }
            DataRow dr = null;
            IEdmVault5 vault = new EdmVault5();
            try
            {
                vault.Login(LogName, PWD, VaultName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            IEdmUserMgr5 UserMgr = default(IEdmUserMgr5);
            UserMgr = (IEdmUserMgr5)vault;
            IEdmPos5 UserPos = default(IEdmPos5);
            UserPos = UserMgr.GetFirstUserPosition();
            while (!UserPos.IsNull)
            {
                dr = dt.NewRow();
                IEdmUser5 user5 = UserMgr.GetNextUser(UserPos);
                IEdmUser6 user6 = (IEdmUser6)user5;
                IEdmUser7 user7 = (IEdmUser7)user5;
                IEdmUser8 user8 = (IEdmUser8)user5;
                IEdmUser9 user9 = (IEdmUser9)user5;
                IEdmUser10 user10 = (IEdmUser10)user5;

                dr["UserName"] = user5.Name;
                dr["UserID"] = user5.ID;
                dr["IsLoggedIn"] = user5.IsLoggedIn;
                dr["Vault"] = user5.Vault.Name;
                dr["FullName"] = user6.FullName;
                dr["UserData"] = user6.UserData;
                dr["Initials"] = user6.Initials;
                dr["ObjectType"] = user6.ObjectType;
                dr["Email"] = user10.Email;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //获取用户名BySQL（FullName）
        public static string GetLoginFullName(string PDMDbName)
        {
            SQLHelper.connString = Globals.connStrPDM;
            string loginId = PDMHelper.GetLoginID();
            string sql = string.Format("select fullname from [{0}].dbo.Users where UserId='{1}'", PDMDbName, loginId);
            return SQLHelper.GetSingleResult(sql).ToString();
        }

        /// <summary>
        /// 获取用户名ByPDM（FullName）
        /// </summary>
        /// <returns></returns>
        public static String GetLoginFullName()
        {
            string loginId = PDMHelper.GetLoginID();
            DataTable dt = PDMHelper.GetUserList();
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format(" UserId='{0}' ", loginId);
            return dv.ToTable("default").Rows[0]["FullName"].ToString();
        }

        /// <summary>
        /// 获取登录名ByPDM（UserName）
        /// </summary>
        /// <returns></returns>
        public static String GetLoginUserName()
        {
            string a = PDMHelper.GetLoginID();
            DataTable dt = PDMHelper.GetUserList();
            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format(" UserId='{0}' ", a);
            return dv.ToTable("default").Rows[0]["UserName"].ToString();
        }

        #endregion

        #region 文件相关操作（File）
        /// <summary>
        /// 【2】判断文件是PDM文件夹内文件
        /// </summary>
        /// <param name="FilePath"文件绝对路径</param>
        /// <returns></returns>
        public static bool IsPDMFiles(string FilePath)
        {
            bool ispdmfile = true;
            if (FilePath.Contains(RootPath))
            {
                ispdmfile = true;
            }
            else
            {
                ispdmfile = false;
            }
            return ispdmfile;
        }
        /// <summary>
        /// 【2.1】根据路径获取ID
        /// </summary>
        /// <param name="filepath">文件路径（包含文件名）</param>
        /// <returns></returns>
        public static int GetFileID(string filepath)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile11 aFile = default(IEdmFile11);
            IEdmFolder5 ppoRetParentFolder;
            if (filepath.Length > 0)
            {
                aFile = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
                //aFile.UnlockFile(new System.Windows.Forms.Control().Handle.ToInt32(), "");
            }
            return aFile.ID;
        }
        /// <summary>
        /// 【3】将文件添加到库中指定位置——导入,并返回pdm中文件全路径（包含文件名）
        /// </summary>
        /// <param name="FullName">要复制库外文件路径和名称</param>
        /// <param name="path">库中根目录下具体位置，如“\\03标准规范\\05技术通知单”</param>
        /// <returns></returns>
        public static string AddFile(string FullName, string path)
        {
            IEdmVault5 vault1 = new EdmVault5();
            IEdmVault7 vault2 = (IEdmVault7)vault1;
            vault1.Login(LogName, PWD, VaultName);//使用管理员登录每个用户不同
            // Add selected file to the root folder of the vault
            IEdmFolder5 Folder = default(IEdmFolder8);
            Folder = (IEdmFolder8)vault2.RootFolder;
            Folder = vault2.GetFolderFromPath(Folder.LocalPath + path);
            string name = FullName.Substring(FullName.LastIndexOf("\\") + 1);
            if (File.Exists(Folder.LocalPath + "\\" + name))
            {
                MessageBox.Show("文件已经存在,请更改文件名称！", "提示信息");
                return "";
            }
            else
            {
                Folder.AddFile(new System.Windows.Forms.Control().Handle.ToInt32(), FullName, "", 0);
            }
            return Folder.LocalPath + "\\" + name;
        }

        /// <summary>
        /// 【4.1】捡入指定文件
        /// </summary>
        /// <param name="filepath">文件全名（包含路径）</param>
        public static void CheckIn(string filepath)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile11 aFile = default(IEdmFile11);
            IEdmFolder5 ppoRetParentFolder;
            if (filepath.Length > 0)
            {
                aFile = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
                aFile.UnlockFile(new System.Windows.Forms.Control().Handle.ToInt32(), "");
            }
        }

        /// <summary>
        /// 【4.2】捡出指定文件
        /// </summary>
        /// <param name="filepath">文件全名（包含路径）</param>
        public static void CheckOut(string filepath)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile11 aFile = default(IEdmFile11);
            IEdmFolder5 ppoRetParentFolder;
            if (filepath.Length > 0)
            {
                aFile = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
                aFile.LockFile(ppoRetParentFolder.ID, new System.Windows.Forms.Control().Handle.ToInt32());
            }
        }
        /// <summary>
        /// 【4.3】取消捡出指定文件
        /// </summary>
        /// <param name="filepath">文件全名（包含路径）</param>
        public static void CancelCheckOut(string filepath)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile11 aFile = default(IEdmFile11);
            IEdmFolder5 ppoRetParentFolder;
            if (filepath.Length > 0)
            {
                aFile = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
                aFile.UndoLockFile(new System.Windows.Forms.Control().Handle.ToInt32());
            }
        }
        /// <summary>
        /// 【4.4】文件是否被检入,true为检出状态。false为检入状态
        /// </summary>
        /// <param name="filepath">文件全名（包含路径）</param>
        /// <returns></returns>
        public static bool IsLock(string filepath)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile11 aFile = default(IEdmFile11);
            IEdmFolder5 ppoRetParentFolder;
            if (filepath.Length > 0)
            {
                aFile = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
            }
            return aFile.IsLocked;
        }
        /// <summary>
        /// 【5】为文件重命名
        /// </summary>
        /// <param name="filepath">文件全名（包含路径）</param>
        /// <param name="newName">新名称</param>
        public static void ReName(string filepath, string newName)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile10 file = default(IEdmFile10);
            IEdmFolder5 ppoRetParentFolder;
            file = (IEdmFile11)vault2.GetFileFromPath(filepath, out ppoRetParentFolder);
            file.Rename(new Control().Handle.ToInt32(), newName);
        }

        /// <summary>
        /// 【6】pdm库内复制文件，返回pdm中文件全路径（包含文件名）
        /// </summary>
        /// <param name="oldFile">旧文件位置</param>
        /// <param name="newFolder">新文件夹位置</param>
        public static string CopyFile(string oldFile, string newFolder)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmFile10 file = default(IEdmFile10);
            IEdmFolder5 ppoRetParentFolder;
            file = (IEdmFile11)vault2.GetFileFromPath(oldFile, out ppoRetParentFolder);
            IEdmFolder5 destFolder1 = default(IEdmFolder5);
            destFolder1 = vault2.GetFolderFromPath(newFolder);//新文件夹位置
            IEdmFolder8 destFolder = (IEdmFolder8)destFolder1;
            int FileID = 0;
            int copyFileStatus;
            FileID = destFolder.CopyFile2(file.ID, ppoRetParentFolder.ID, new Control().Handle.ToInt32(), out copyFileStatus, "", (int)EdmCopyFlag.EdmCpy_Simple);
            return destFolder1.LocalPath + "\\" + file.Name.Substring(file.Name.LastIndexOf('\\') + 1);
        }

        /// <summary>
        /// 【7】将文件复制到指定路径(硬盘)——导出
        /// </summary>
        /// <param name="PathInVault">库文件夹根目录下相对路径</param>
        /// <param name="PathOutVault">本地具体路径</param>
        public static void CheckoutAndCopy(string PathInVault, string PathOutVault)
        {
            try
            {
                EdmVault5 vault = new EdmVault5();
                vault.Login(LogName, PWD, VaultName);
                IEdmFile5 file = default(IEdmFile5);
                IEdmFolder5 folder = null;
                file = vault.GetFileFromPath(vault.RootFolderPath + PathInVault, out folder);//读取指定文件的位置，PathInVault库文件夹根目录下相对路径
                file.LockFile(folder.ID, new System.Windows.Forms.Control().Handle.ToInt32());
                file.UndoLockFile(new System.Windows.Forms.Control().Handle.ToInt32(), true);
                //Copy the file
                IEdmEnumeratorVersion5 verEnum = default(IEdmEnumeratorVersion5);
                verEnum = (IEdmEnumeratorVersion5)file;
                int Version = 0;
                Version = file.GetLocalVersionNo(folder.ID);
                IEdmVersion5 ver = default(IEdmVersion5);
                ver = verEnum.GetVersion(Version);
                ver.GetFileCopy(new System.Windows.Forms.Control().Handle.ToInt32(), PathOutVault);//PathOutVault本地具体路径
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode.ToString() == "-2147220949")
                {
                    MessageBox.Show("The selected file is not located in a file vault.");
                }
                else
                {
                    MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 【8】根据文件名称搜索路径,返回全部路径的集合，如果未搜索到返回null
        /// </summary>
        /// <param name="fileName">文件名称（不包含路径）</param>
        public static List<string> SearchPart(string fileName)
        {
            IEdmVault5 vault1 = new EdmVault5();
            vault1.Login(LogName, PWD, VaultName);
            IEdmVault7 vault2 = null;
            vault2 = (IEdmVault7)vault1;
            IEdmSearch5 Search = (IEdmSearch5)vault2.CreateUtility(EdmUtility.EdmUtil_Search);
            Search.FileName = "%" + fileName + "%";//模糊搜索
            //Search.FileName = fileName;//命名搜索
            IEdmSearchResult5 Result = Search.GetFirstResult();//搜索第一个
            List<string> lst = new List<string>();
            //string filePath = string.Empty;
            while (Result != null)
            {
                lst.Add(Result.Path);
                Result = Search.GetNextResult();//搜索下一个
            }
            return lst;
        }

        /// <summary>
        /// 【10】获取文件版本
        /// </summary>
        /// <param name="path">文件在PDM中路径</param>
        /// <returns></returns>
        public static int GetDrawVersion(string path)
        {

            if (vault == null)
            {
                vault = new EdmVault5();
            }
            if (!vault.IsLoggedIn)
            {
                vault.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            IEdmFolder5 retFolder = default(IEdmFolder5);
            //aFile = vault1.GetFileFromPath(FileName, out retFolder);
            IEdmFile5 file = null;
            file = vault.GetFileFromPath(path, out retFolder);
            IEdmEnumeratorVersion5 enumVersion;
            enumVersion = (IEdmEnumeratorVersion5)file;
            return enumVersion.VersionCount;
        }
        #endregion

        #region 组相关操作（Group）
        /// <summary>
        /// 【11】获取组名
        /// </summary>
        /// <returns>共有2列，Name和Code</returns>
        public static DataTable GetGroup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
            IEdmUserMgr7 UsrMgr;
            IEdmUserGroup8 mngmtGroup;
            if (vault == null)
            {
                vault = new EdmVault5();
            }
            if (!vault.IsLoggedIn)
            {
                vault.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            UsrMgr = (IEdmUserMgr7)vault;

            //Traverse groups
            //string Groups = "";
            IEdmPos5 UserGroupPos = default(IEdmPos5);
            UserGroupPos = UsrMgr.GetFirstUserGroupPosition();
            while (!UserGroupPos.IsNull)
            {
                mngmtGroup = (IEdmUserGroup8)UsrMgr.GetNextUserGroup(UserGroupPos);
                //Groups = Groups + mngmtGroup.Name + "\n";
                DataRow dr = dt.NewRow();
                dr["Name"] = mngmtGroup.Name;
                dr["Code"] = mngmtGroup.ID;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 【11.1】获取组成员
        /// </summary>
        /// <param name="GroupID">组ID</param>
        /// <returns>共有2列，ID/Name/FullName</returns>
        public static DataTable GetGroupMember(int GroupID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("FullName");
            IEdmUserMgr7 UsrMgr;
            IEdmUserGroup8 mngmtGroup;
            IEdmUser9 user;
            IEdmPos5 UserPos = default(IEdmPos5);
            if (vault == null)
            {
                vault = new EdmVault5();
            }
            if (!vault.IsLoggedIn)
            {
                vault.LoginAuto(VaultName, new Control().Handle.ToInt32());
            }
            UsrMgr = (IEdmUserMgr7)vault;

            IEdmPos5 UserGroupPos = default(IEdmPos5);
            UserGroupPos = UsrMgr.GetFirstUserGroupPosition();
            while (!UserGroupPos.IsNull)
            {
                mngmtGroup = (IEdmUserGroup8)UsrMgr.GetNextUserGroup(UserGroupPos);
                if (mngmtGroup.ID == GroupID)
                {

                    UserPos = mngmtGroup.GetFirstUserPosition();
                    while (!UserPos.IsNull)
                    {
                        user = (IEdmUser9)mngmtGroup.GetNextUser(UserPos);
                        //Users = Users + " " + user.Name + "\n";
                        DataRow dr = dt.NewRow();
                        dr["ID"] = user.ID;
                        dr["Name"] = user.Name;
                        dr["FullName"] = user.FullName;
                        dt.Rows.Add(dr);
                    }
                }


            }
            return dt;
        }
        #endregion

        #region 字符串截取
        /// <summary>
        /// 从文件路径获取独立的文件名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNameFromPath(string str)
        {
            //如：从"D:\123\name.sldprt"获得"name"
            string[] strArray = str.Remove(str.LastIndexOf('.')).Split('\\');
            return strArray[strArray.Count() - 1];
        }
        /// <summary>
        /// 从用户信息组合中获取用户名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNameFromString(string str)
        {
            //如：从"李小明-001"获得"李小明"
            string[] strArray = str.Split('-');
            return strArray[0];
        }
        /// <summary>
        /// 获取字符串后几位数
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }
        #endregion
    }
}
