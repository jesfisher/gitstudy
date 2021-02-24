using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZQCode
{
    /// <summary>
    /// 图号编码数据访问层
    /// </summary>
    public class DrawingService
    {
        //统一使用编码数据库连接字符串：conStrCODE
        public List<string> GetDrawingRequestStyle(string fileId, string materialSpec)
        {
            //集合中包含3个值：DrawingId，DrawingClassId，RequestStyle（1-新图号生成；2-旧图号保持；3-新规格生成）
            string drawingId = string.Empty;
            string drawingClassid = string.Empty;
            int requestStyle = 0;
            List<string> list = new List<string>();
            DataTable dt = null;
            //【1】第1次判断：是否新图号生成
            dt = GetDrawingInfoByDocumentId(fileId);//仅判断文件ID
            if (dt.Rows.Count == 0)
            {
                requestStyle = 1;//1-新图号生成
            }
            else
            {
                drawingId = dt.Rows[0]["DrawingId"].ToString();
                drawingClassid = dt.Rows[0]["DrawingClassId"].ToString();
                //【2】第2次判断：是否X/G
                if (drawingClassid == "X" || drawingClassid == "G")
                {
                    //【3】第3次判断：是否新规格生成
                    dt = GetDrawingInfoByDocumentId(fileId, materialSpec);//判断文件ID+规格
                    if (dt.Rows.Count == 0)
                    {
                        requestStyle = 3;//3-新规格生成
                    }
                    else
                    {
                        drawingId = dt.Rows[0]["DrawingId"].ToString();
                        drawingClassid = dt.Rows[0]["DrawingClassId"].ToString();
                        requestStyle = 2;//2-旧图号保持
                    }
                }
                else
                {
                    requestStyle = 2;//2-旧图号保持
                }
            }
            //给结果集合list赋值并返回
            list.Add(drawingId);
            list.Add(drawingClassid);
            list.Add(requestStyle.ToString());
            return list;
        }


        #region 三个判断（是否申请新图号/是否是系列件XorG/是否XorG添加新规格）
        /// <summary>
        /// 【1】是否申请新图号(返回null值，则是新图号申请)
        /// </summary>
        /// <returns></returns>
        public string IsNewDrawingCode(string fileId)
        {
            string drawingId = GetDrawingIdByDocumentId(fileId);
            if (drawingId.Length > 0)
                return drawingId;
            else
                return null;
        }

        /// <summary>
        /// 【2】是否是系列件XorG(图号分类是X或G)
        /// </summary>
        /// <returns></returns>
        public bool IsXorG(DrawingModel objDrawing)
        {
            if (objDrawing.DrawingClassId == "X" || objDrawing.DrawingClassId == "G")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 【3】是否XorG添加新规格(添加新规格或更改原规格属性)
        /// </summary>
        /// <returns></returns>
        public bool IsNewMaterialSpec(string drawingCode, string materialSpec)
        {
            //不存在或修改，即新增
            return !IsExistDrawingCodeAndMaterialSpec(drawingCode, materialSpec);
        }
        #endregion

        #region 两个重要的重复性验证（图号；图号+规格[配置]）

        //判断图号是否存在(DrawingCode)(新增/修改)
        public bool IsExistDrawingCode(string drawingCode, string drawingId = null)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select count(*) from code_Drawing where DrawingCode='{0}'", drawingCode);
            if (drawingId != null)
            {
                sql += (" and DrawingId <> '" + drawingId + "'");
            }
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            return result != 0;
        }

        /// <summary>
        /// 判断图号+规格型号是否存在（DrawingCode+MaterialSpec）【规格型号=配置名】
        /// </summary>
        /// <param name="drawingCode">图号</param>
        /// <param name="materialSpec">规格型号</param>
        /// <returns></returns>
        public bool IsExistDrawingCodeAndMaterialSpec(string drawingCode, string materialSpec)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select count(*) from code_Drawing where DrawingCode='{0}' and MaterialSpec='{1}'", drawingCode, materialSpec);
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            return result != 0;
        }

        #endregion

        #region 两个获取图号所有属性的办法
        //从模型获取图号属性
        public DrawingModel GetDrawingFromSW()
        {
            return swAppHelper.GetDrawingInfo();
        }

        //从数据库获取图号属性(DrawingId)
        public DrawingModel GetDrawingFromSQL(string drawingId)
        {
            return GetInfoByDrawingId(drawingId);
        }

        #endregion

        #region 新增图号或修改图号

        /// <summary>
        /// 新增图号
        /// </summary>
        /// <param name="objDrawing"></param>
        /// <returns></returns>
        public bool AddDrawing(DrawingModel objDrawing)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "insert into code_Drawing(DrawingClassId, DrawingCode, MaterialName, MaterialSpec, Weight, Unit, HeatTreatment, SurfaceTreatment, Brand, PurchaseTypeId, SelectionTypeId, DrawingStatusId, Revision, DocIdModel, ReMark, CreateFrom, CreateId, CreateUser, CreateDate, CreateInfo) values(@DrawingClassId, @DrawingCode, @MaterialName, @MaterialSpec, @Weight, @Unit, @HeatTreatment, @SurfaceTreatment, @Brand, @PurchaseTypeId, @SelectionTypeId, @DrawingStatusId, @Revision, @DocIdModel, @ReMark, @CreateFrom, @CreateId, @CreateUser, @CreateDate, @CreateInfo)";
            SqlParameter[] param = new SqlParameter[]
            {
                //只封装一部分主要的参数
                new SqlParameter("@DrawingClassId",objDrawing.DrawingClassId),
                new SqlParameter("@DrawingCode",objDrawing.DrawingCode),
                new SqlParameter("@MaterialName",objDrawing.MaterialName),
                new SqlParameter("@MaterialSpec",objDrawing.MaterialSpec),
                new SqlParameter("@Weight",objDrawing.Weight),
                new SqlParameter("@Unit",objDrawing.Unit),
                new SqlParameter("@HeatTreatment",objDrawing.HeatTreatment),
                new SqlParameter("@SurfaceTreatment",objDrawing.SurfaceTreatment),
                new SqlParameter("@Brand",objDrawing.Brand),
                new SqlParameter("@PurchaseTypeId",objDrawing.PurchaseTypeId),                
                new SqlParameter("@SelectionTypeId",objDrawing.SelectionTypeId),
                new SqlParameter("@DrawingStatusId",objDrawing.DrawingStatusId),
                new SqlParameter("@Revision",objDrawing.Revision),
                new SqlParameter("@DocIdModel",objDrawing.DocIdModel),
                new SqlParameter("@ReMark",objDrawing.ReMark),
                new SqlParameter("@CreateFrom",objDrawing.CreateFrom),
                new SqlParameter("@CreateId",objDrawing.CreateId),
                new SqlParameter("@CreateUser",objDrawing.CreateUser),
                new SqlParameter("@CreateDate",objDrawing.CreateDate),
                new SqlParameter("@CreateInfo",objDrawing.CreateInfo)
            };
            int result = SQLHelper.Update(sql, param, false);
            return result != 0;
        }

        /// <summary>
        /// 修改图号
        /// </summary>
        /// <param name="objDrawing"></param>
        /// <returns></returns>
        public bool ModifyMaterial(DrawingModel objDrawing)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            //string sql = "update code_Drawing set DrawingClassId=@DrawingClassId, DrawingCode=@DrawingCode, MaterialName=@MaterialName, MaterialSpec=@MaterialSpec, Weight=@Weight, Unit=@Unit, HeatTreatment=@HeatTreatment, SurfaceTreatment=@SurfaceTreatment, Brand=@Brand, PurchaseTypeId=@PurchaseTypeId, SelectionTypeId=@SelectionTypeId, DrawingStatusId=@DrawingStatusId, Revision=@Revision, DocIdModel=@DocIdModel, ReMark=@ReMark, UpdateId=@UpdateId, UpdateUser=@UpdateUser, UpdateDate=@UpdateDate, UpdateInfo=@UpdateInfo where DrawingId=@DrawingId";
            string sql = "update code_Drawing set DrawingClassId=@DrawingClassId, DrawingCode=@DrawingCode, MaterialName=@MaterialName, MaterialSpec=@MaterialSpec, Weight=@Weight, Unit=@Unit, HeatTreatment=@HeatTreatment, SurfaceTreatment=@SurfaceTreatment, Brand=@Brand, PurchaseTypeId=@PurchaseTypeId, SelectionTypeId=@SelectionTypeId, ReMark=@ReMark, UpdateId=@UpdateId, UpdateUser=@UpdateUser, UpdateDate=@UpdateDate, UpdateInfo=@UpdateInfo where DrawingId=@DrawingId";
            SqlParameter[] param = new SqlParameter[]
            {
                //只封装一部分主要的参数
                new SqlParameter("@DrawingId",objDrawing.DrawingId),
                new SqlParameter("@DrawingClassId",objDrawing.DrawingClassId),
                new SqlParameter("@DrawingCode",objDrawing.DrawingCode),
                new SqlParameter("@MaterialName",objDrawing.MaterialName),
                new SqlParameter("@MaterialSpec",objDrawing.MaterialSpec),
                new SqlParameter("@Weight",objDrawing.Weight),
                new SqlParameter("@Unit",objDrawing.Unit),
                new SqlParameter("@HeatTreatment",objDrawing.HeatTreatment),
                new SqlParameter("@SurfaceTreatment",objDrawing.SurfaceTreatment),
                new SqlParameter("@Brand",objDrawing.Brand),
                new SqlParameter("@PurchaseTypeId",objDrawing.PurchaseTypeId),                
                new SqlParameter("@SelectionTypeId",objDrawing.SelectionTypeId),
                //new SqlParameter("@DrawingStatusId",objDrawing.DrawingStatusId),
                //new SqlParameter("@Revision",objDrawing.Revision),
                //new SqlParameter("@DocIdModel",objDrawing.DocIdModel),
                new SqlParameter("@ReMark",objDrawing.ReMark),
                new SqlParameter("@UpdateId",objDrawing.UpdateId),
                new SqlParameter("@UpdateUser",objDrawing.UpdateUser),
                new SqlParameter("@UpdateDate",objDrawing.UpdateDate),
                new SqlParameter("@UpdateInfo",objDrawing.UpdateInfo)
            };
            int result = SQLHelper.Update(sql, param, false);
            return result != 0;
        }
        #endregion

        #region 获取带流水码的新图号

        /// <summary>
        /// BLL获取新的图号编码（6位流水码：T123456）
        /// </summary>
        /// <param name="MyPrefix">前缀字母</param>
        /// <returns></returns>
        public string GetDrawingCode(string MyPrefix)
        {
            //string MyPrefix = "T";//前缀输入参数
            string MyNolen = "6";//流水号长度
            string MyListna = "DrawingCode";//流水单号对应的列名
            string MyTablena = "code_Drawing";//表名
            //流水码（返回值）
            string MyRunningNum = GetNewDrawingCode(MyPrefix, MyNolen, MyListna, MyTablena);
            return MyRunningNum;
        }

        /// <summary>
        /// DAL获取新的图号编码（6位流水码：T123456）
        /// </summary>
        /// <param name="myPrefix">前缀字母</param>
        /// <param name="myNolen">流水号长度</param>
        /// <param name="myListna">流水单号对应的列名</param>
        /// <param name="myTablena">表名</param>
        /// <returns></returns>
        public string GetNewDrawingCode(string myPrefix, string myNolen, string myListna, string myTablena)
        {
            try
            {
                SQLHelper.connString = Globals.connStrGroupDBName;
                //【1】自定义存储过程
                string sql = "usp_AutoFlowNum";
                //【2】存储过程的参数(5个变量)
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@prefix",  myPrefix),//前缀字母
                    new SqlParameter("@nolen", myNolen),//流水号长度
                    new SqlParameter("@listna", myListna),//流水单号对应的列名
                    new SqlParameter("@tablena",  myTablena),//表名
                    new SqlParameter("@runningnum", string.Empty)//生成流水码
                };
                //【3】执行并返回新的图号编码
                return SQLHelper.GetSingleResult(sql, param, true).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 从数据库获取列表信息（图号分类、单位、采购分类、选型分类）
        //获取所有图号分类数据
        public List<DrawingClassModel> GetDrawingClass()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select DrawingClassId,DrawingClassName from code_DrawingClass";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<DrawingClassModel> list = new List<DrawingClassModel>();
            while (objReader.Read())
            {
                list.Add(new DrawingClassModel()
                {
                    DrawingClassId = objReader["DrawingClassId"].ToString(),
                    DrawingClassName = objReader["DrawingClassName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //获取所有单位数据
        public List<UnitModel> GetUnit()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select UnitId,UnitCode,UnitName from code_Unit";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<UnitModel> list = new List<UnitModel>();
            while (objReader.Read())
            {
                list.Add(new UnitModel()
                {
                    //UnitId = Convert.ToInt32(objReader["UnitId"]),
                    Unit = objReader["UnitCode"].ToString(),
                    UnitName = objReader["UnitName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //获取采购分类数据
        public List<MachiningPropertyModel> GetPurchaseType()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select PurchaseTypeId,PurchaseTypeName from code_PurchaseType";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<MachiningPropertyModel> list = new List<MachiningPropertyModel>();
            while (objReader.Read())
            {
                list.Add(new MachiningPropertyModel()
                {
                    MachiningPropertyId = objReader["PurchaseTypeId"].ToString(),
                    MachiningPropertyName = objReader["PurchaseTypeName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //获取选型分类数据
        public List<SelectionTypeModel> GetSelectionType()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select SelectionTypeId,SelectionTypeName from code_SelectionType";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<SelectionTypeModel> list = new List<SelectionTypeModel>();
            while (objReader.Read())
            {
                list.Add(new SelectionTypeModel()
                {
                    SelectionTypeId = objReader["SelectionTypeId"].ToString(),
                    SelectionTypeName = objReader["SelectionTypeName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        #endregion

        #region 根据DocIdModel查找DrawingId
        //根据DocIdModel查找DrawingId
        public string GetDrawingIdByDocumentId(string fileId)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select DrawingId,DrawingClassId from code_Drawing where DrawingStatusId in (0,1,2,3) and DocIdModel='{0}'", fileId);
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            string result = null;
            if (objReader.Read())
            {
                result = objReader["DrawingId"].ToString();
            }
            objReader.Close();
            return result;
        }

        /// <summary>
        /// 根据DocIdModel查找图号相关信息(可能是多条记录)
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="materialSpec">规格型号(配置名)</param>
        /// <returns>返回dt表(DrawingId，DrawingClassId)</returns>
        public DataTable GetDrawingInfoByDocumentId(string fileId, string materialSpec = null)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            StringBuilder sb = new StringBuilder();
            sb.Append("select DrawingId,DrawingClassId from code_Drawing where 1=1 ");
            if (String.IsNullOrEmpty(fileId) == false)
            {
                sb.Append(" AND DocIdModel='" + fileId + "'");
            }
            if (String.IsNullOrEmpty(materialSpec) == false)
            {
                sb.Append(" AND MaterialSpec='" + materialSpec + "'");
            }
            string sql = sb.ToString();
            return SQLHelper.GetDataSet(sql).Tables[0];
        }

        #endregion

        #region 根据DocIdModel获取图号编码信息
        //如果通过文件ID可以查询到图号编码等信息(图号状态是1)，则返回4个字符串，否则返回空的List
        public DrawingModel GetFileDrawingInfo(string fileID)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from code_Drawing");
            strSql.Append(" where DocIdModel={0} and DrawingStatusId=1");
            strSql.Append(" ORDER BY DrawingId DESC");
            string sql = string.Format(strSql.ToString(), fileID);
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            DrawingModel objDrawing = null;
            if (dt.Rows.Count >= 1)
            {
                objDrawing = new DrawingModel()
               {
                   DrawingCode = dt.Rows[0]["DrawingCode"].ToString(),
                   MaterialName = dt.Rows[0]["MaterialName"].ToString(),
                   MaterialSpec = dt.Rows[0]["MaterialSpec"].ToString(),
                   DocIdModel = Convert.ToInt32(dt.Rows[0]["DocIdModel"])
               };
            }
            return objDrawing;
        }

        #endregion

        #region 根据DrawingId获取图号编码信息
        //如果通过文件ID可以查询到图号编码等信息(图号状态是1)，则返回4个字符串，否则返回空的List
        public DrawingModel GetInfoByDrawingId(string drawingId)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from code_Drawing where DrawingId='{0}'");
            strSql.Append(" ORDER BY DrawingId DESC");
            string sql = string.Format(strSql.ToString(), drawingId);
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            DrawingModel objDrawing = null;
            if (dt.Rows.Count >= 1)
            {
                objDrawing = new DrawingModel()
                {
                    DrawingId = dt.Rows[0]["DrawingId"].ToString(),
                    DrawingClassId = dt.Rows[0]["DrawingClassId"].ToString(),
                    DrawingCode = dt.Rows[0]["DrawingCode"].ToString(),
                    MaterialName = dt.Rows[0]["MaterialName"].ToString(),
                    MaterialSpec = dt.Rows[0]["MaterialSpec"].ToString(),
                    Unit = dt.Rows[0]["Unit"].ToString(),
                    PurchaseTypeId = dt.Rows[0]["PurchaseTypeId"].ToString(),
                    SelectionTypeId = dt.Rows[0]["SelectionTypeId"].ToString(),
                    Weight = dt.Rows[0]["Weight"].ToString(),
                    SurfaceTreatment = dt.Rows[0]["SurfaceTreatment"].ToString(),
                    HeatTreatment = dt.Rows[0]["HeatTreatment"].ToString(),
                    Brand = dt.Rows[0]["Brand"].ToString(),
                    ReMark = dt.Rows[0]["ReMark"].ToString(),
                    DocIdModel = Convert.ToInt32(dt.Rows[0]["DocIdModel"])
                };
            }
            return objDrawing;
        }

        #endregion

        #region 基础验证（插件版本、登录名、服务器时间）
        //获取插件版本SWCodeVersion
        public string GetSWCodeVersion(string programname)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select top 1 VersionNumber from sys_ProgramVersion where ProgramName='{0}' order by progid DESC", programname);
            return SQLHelper.GetSingleResult(sql).ToString();
        }

        //查询PDM登录名是否与CODE程序登录名相同
        public bool IsExistUserName(string username)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select count(*) from tb_MyUser where Account='{0}'", username);
            //return SQLHelper.Update(strSql.ToString(), SqlPara, false) > 0 ? true : false;
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result > 0) return true;
            else return false;
        }

        //获取服务器时间
        public DateTime GetDBServerTime()
        {
            return SQLHelper.GetDBServerTime();
        }
        #endregion
    }
}
