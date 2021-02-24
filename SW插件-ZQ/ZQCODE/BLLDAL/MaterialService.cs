using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZQCode
{
    /// <summary>
    /// 物料编码编码数据访问层
    /// </summary>
    public class MaterialService
    {
        #region 从数据库获取列表信息（物料分类、料件类别、单位、加工属性表等）
        //【事业部申请物料时使用】查询事业部(工厂)物料分类
        public DataTable GetFactoryMaterialClass()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("SELECT * FROM code_MaterialClass WHERE IsPublic=1 or (FactoryCode='{0}' and IsPublic=0)", Globals.FactoryCode);//FactoryCode=1000
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        //料件类别
        public List<MaterialCategoryModel> GetMaterialCategory()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_MaterialCategory";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<MaterialCategoryModel> list = new List<MaterialCategoryModel>();
            while (objReader.Read())
            {
                list.Add(new MaterialCategoryModel()
                {
                    MaterialCategoryId = objReader["MaterialCategoryId"].ToString(),
                    MaterialCategoryName = objReader["MaterialCategoryName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //单位
        public List<UnitModel> GetUnit()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_Unit";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<UnitModel> list = new List<UnitModel>();
            while (objReader.Read())
            {
                list.Add(new UnitModel()
                {
                    Unit = objReader["Unit"].ToString(),
                    UnitName = objReader["UnitName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //加工属性表
        public List<MachiningPropertyModel> GetMachiningProperty()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_MachiningProperty";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<MachiningPropertyModel> list = new List<MachiningPropertyModel>();
            while (objReader.Read())
            {
                list.Add(new MachiningPropertyModel()
                {
                    MachiningPropertyId = objReader["MachiningPropertyId"].ToString(),
                    MachiningPropertyName = objReader["MachiningPropertyName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //涂装颜色
        public List<PaintingColorModel> GetPaintingColor()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_PaintingColor";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<PaintingColorModel> list = new List<PaintingColorModel>();
            while (objReader.Read())
            {
                list.Add(new PaintingColorModel()
                {
                    //没有使用PaintingColorId，全部使用PaintingColor
                    PaintingColorId = objReader["PaintingColor"].ToString(),
                    PaintingColor = objReader["PaintingColor"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //图样特征代号
        public List<DraftFeatureModel> GetDraftFeature()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_DraftFeature";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<DraftFeatureModel> list = new List<DraftFeatureModel>();
            while (objReader.Read())
            {
                list.Add(new DraftFeatureModel()
                {
                    DraftFeatureId = objReader["DraftFeatureId"].ToString(),
                    DraftFeatureName = objReader["DraftFeatureName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }

        //品牌
        public List<BrandModel> GetBrand()
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "select * from code_Brand";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<BrandModel> list = new List<BrandModel>();
            while (objReader.Read())
            {
                list.Add(new BrandModel()
                {
                    BrandId = objReader["BrandId"].ToString(),
                    BrandName = objReader["BrandName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
        #endregion

        //从数据库获取选中节点的完整信息
        public DataRow GetMaterialClassById(string materialclassId)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select * from code_MaterialClass where MaterialClassId='{0}'", materialclassId);
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        //获取物料编码详细信息(判断该文件ID是否是新图纸)
        public DataRow GetMaterialInfo(string fileId, string factoryCode)
        {
            DataTable dt = GetDrawingInfoByDocumentId(fileId, factoryCode);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];//dr不为空————修改物料编码         
            }
            else
            {
                return null;//dr为空————新增物料编码
            }
        }

        /// <summary>
        /// 根据DocIdModel、FactoryCode查找图号相关信息(可能是多条记录)
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="factoryCode">工厂编码</param>
        /// <returns></returns>
        public DataTable GetDrawingInfoByDocumentId(string fileId, string factoryCode)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select * from code_Material where StatusId<4 and Deleted=0 and DocIdModel='{0}' and FactoryCode='{1}'", fileId, factoryCode);
            return SQLHelper.GetDataSet(sql).Tables[0];
        }


        #region 两个重要的重复性验证（图号；图号+规格[配置]）

        //判断图号是否存在(DrawingCode)(新增/修改)
        public bool IsExistDrawingCode(string drawingCode, string drawingId = null)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = string.Format("select count(*) from code_Material where StatusId<4 and Deleted=0 and DrawingCode='{0}'", drawingCode);
            if (drawingId != null)
            {
                sql += (" and MaterialId <> '" + drawingId + "'");
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

        //从模型获取物料属性
        public MaterialModel GetMaterialFromSW()
        {
            return swAppHelper.GetMaterialInfo();
        }

        //从数据库获取图号属性(DrawingId)
        public MaterialModel GetMaterialFromSQL()
        {
            DataRow dr = GetMaterialInfo(Globals.FileID, Globals.FactoryCode);
            if (dr == null) return null;
            MaterialModel objMaterial = new MaterialModel()
            {
                MaterialId = dr["MaterialId"].ToString(),
                IsPublic = Convert.ToBoolean(dr["IsPublic"]),
                MaterialCode = dr["MaterialCode"].ToString(),
                FactoryCode = Globals.FactoryCode,
                MaterialClassId = dr["MaterialClassId"].ToString(),
                MaterialCategoryId = dr["MaterialCategoryId"].ToString(),
                DrawingCode = dr["DrawingCode"].ToString(),
                MaterialName = dr["MaterialName"].ToString(),
                MaterialSpec = dr["MaterialSpec"].ToString(),
                MaterialType = dr["MaterialType"].ToString(),
                Unit = dr["Unit"].ToString(),
                Mquality = dr["Mquality"].ToString(),
                Weight = dr["Weight"].ToString(),
                MachiningPropertyId = dr["MachiningPropertyId"].ToString(),
                PaintingColor = dr["PaintingColor"].ToString(),
                BrandId = dr["BrandId"].ToString(),
                ImportanceGrade = dr["ImportanceGrade"].ToString(),
                DraftFeatureId = dr["DraftFeatureId"].ToString(),
                HeatTreatment = dr["HeatTreatment"].ToString(),
                SurfaceTreatment = dr["SurfaceTreatment"].ToString(),
                ReMark = dr["ReMark"].ToString()
            };
            return objMaterial;
        }

        //如果通过文件ID可以查询到图号编码等信息(图号状态是1)，则返回4个字符串，否则返回空的List
        public MaterialModel GetInfoByDrawingId(string materialId)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from code_Material where MaterialId='{0}'");
            strSql.Append(" ORDER BY DrawingId DESC");
            string sql = string.Format(strSql.ToString(), materialId);
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            MaterialModel objMaterial = null;
            if (dt.Rows.Count >= 1)
            {
                objMaterial = new MaterialModel()
                {
                    //DrawingId = dt.Rows[0]["DrawingId"].ToString(),
                    //DrawingClassId = dt.Rows[0]["DrawingClassId"].ToString(),
                    //DrawingCode = dt.Rows[0]["DrawingCode"].ToString(),
                    //MaterialName = dt.Rows[0]["MaterialName"].ToString(),
                    //MaterialSpec = dt.Rows[0]["MaterialSpec"].ToString(),
                    //Unit = dt.Rows[0]["Unit"].ToString(),
                    //PurchaseTypeId = dt.Rows[0]["PurchaseTypeId"].ToString(),
                    //SelectionTypeId = dt.Rows[0]["SelectionTypeId"].ToString(),
                    //Weight = dt.Rows[0]["Weight"].ToString(),
                    //SurfaceTreatment = dt.Rows[0]["SurfaceTreatment"].ToString(),
                    //HeatTreatment = dt.Rows[0]["HeatTreatment"].ToString(),
                    //Brand = dt.Rows[0]["Brand"].ToString(),
                    ReMark = dt.Rows[0]["ReMark"].ToString(),
                    DocIdModel = Convert.ToInt32(dt.Rows[0]["DocIdModel"])
                };
            }
            return objMaterial;
        }
        #endregion

        #region 新增图号或修改图号

        /// <summary>
        /// 新增物料编码
        /// </summary>
        /// <param name="objMaterial"></param>
        /// <returns></returns>
        public bool AddMaterial(MaterialModel objMaterial)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            string sql = "insert into code_Material(MaterialClassId, MaterialCode, FactoryCode, IsPublic, MaterialCategoryId, DrawingCode, MaterialName, MaterialSpec, MaterialType, Mquality, Weight, Unit, ImportanceGrade, DraftFeatureId, MachiningPropertyId, PaintingColor, HeatTreatment, SurfaceTreatment, BrandId, StatusId, Revision, DocIdModel, ReMark, CreateFrom, CreateId, CreateUser, CreateDate, CreateInfo) values(@MaterialClassId, @MaterialCode, @FactoryCode, @IsPublic, @MaterialCategoryId, @DrawingCode, @MaterialName, @MaterialSpec, @MaterialType, @Mquality, @Weight, @Unit, @ImportanceGrade, @DraftFeatureId, @MachiningPropertyId, @PaintingColor, @HeatTreatment, @SurfaceTreatment, @BrandId, @StatusId, @Revision, @DocIdModel, @ReMark, @CreateFrom, @CreateId, @CreateUser, @CreateDate, @CreateInfo)";
            SqlParameter[] param = new SqlParameter[]
            {
                //只封装一部分主要的参数
                new SqlParameter("@MaterialClassId",objMaterial.MaterialClassId),
                new SqlParameter("@MaterialCode",objMaterial.MaterialCode),
                new SqlParameter("@FactoryCode",objMaterial.FactoryCode),
                new SqlParameter("@IsPublic",objMaterial.IsPublic),
                new SqlParameter("@MaterialCategoryId",objMaterial.MaterialCategoryId),
                new SqlParameter("@DrawingCode",objMaterial.DrawingCode),
                new SqlParameter("@MaterialName",objMaterial.MaterialName),
                new SqlParameter("@MaterialSpec",objMaterial.MaterialSpec),
                new SqlParameter("@MaterialType",objMaterial.MaterialType),
                new SqlParameter("@Mquality",objMaterial.Mquality),
                new SqlParameter("@Weight",objMaterial.Weight),
                new SqlParameter("@Unit",objMaterial.Unit),
                new SqlParameter("@ImportanceGrade",ConvertEx.SqlNull(objMaterial.ImportanceGrade)),
                new SqlParameter("@DraftFeatureId",ConvertEx.SqlNull(objMaterial.DraftFeatureId)),
                new SqlParameter("@MachiningPropertyId",ConvertEx.SqlNull(objMaterial.MachiningPropertyId)),
                new SqlParameter("@PaintingColor",ConvertEx.SqlNull(objMaterial.PaintingColor)),
                new SqlParameter("@HeatTreatment",objMaterial.HeatTreatment),
                new SqlParameter("@SurfaceTreatment",objMaterial.SurfaceTreatment),
                new SqlParameter("@BrandId",ConvertEx.SqlNull(objMaterial.BrandId)),
                new SqlParameter("@StatusId",objMaterial.StatusId),
                new SqlParameter("@Revision",objMaterial.Revision),
                new SqlParameter("@DocIdModel",objMaterial.DocIdModel),
                new SqlParameter("@ReMark",objMaterial.ReMark),
                new SqlParameter("@CreateFrom",objMaterial.CreateFrom),
                new SqlParameter("@CreateId",objMaterial.CreateId),
                new SqlParameter("@CreateUser",objMaterial.CreateUser),
                new SqlParameter("@CreateDate",objMaterial.CreateDate),
                new SqlParameter("@CreateInfo",objMaterial.CreateInfo)
            };
            int result = SQLHelper.Update(sql, param, false);
            return result != 0;
        }

        /// <summary>
        /// 修改图号
        /// </summary>
        /// <param name="objDrawing"></param>
        /// <returns></returns>
        public bool ModifyMaterial(MaterialModel objMaterial)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            //string sql = "update code_Drawing set DrawingClassId=@DrawingClassId, DrawingCode=@DrawingCode, MaterialName=@MaterialName, MaterialSpec=@MaterialSpec, Weight=@Weight, Unit=@Unit, HeatTreatment=@HeatTreatment, SurfaceTreatment=@SurfaceTreatment, Brand=@Brand, PurchaseTypeId=@PurchaseTypeId, SelectionTypeId=@SelectionTypeId, DrawingStatusId=@DrawingStatusId, Revision=@Revision, DocIdModel=@DocIdModel, ReMark=@ReMark, UpdateId=@UpdateId, UpdateUser=@UpdateUser, UpdateDate=@UpdateDate, UpdateInfo=@UpdateInfo where DrawingId=@DrawingId";
            string sql = "update code_Material set MaterialName=@MaterialName, MaterialSpec=@MaterialSpec, MaterialType=@MaterialType, Mquality=@Mquality, Weight=@Weight, Unit=@Unit, ImportanceGrade=@ImportanceGrade, DraftFeatureId=@DraftFeatureId, MachiningPropertyId=@MachiningPropertyId, PaintingColor=@PaintingColor, HeatTreatment=@HeatTreatment, SurfaceTreatment=@SurfaceTreatment, BrandId=@BrandId, ReMark=@ReMark, UpdateId=@UpdateId, UpdateUser=@UpdateUser, UpdateDate=@UpdateDate, UpdateInfo=@UpdateInfo where MaterialId=@MaterialId";
            SqlParameter[] param = new SqlParameter[]
            {
                //只封装一部分主要的参数
                new SqlParameter("@MaterialId",objMaterial.MaterialId),
                //new SqlParameter("@MaterialClassId",objMaterial.MaterialClassId),
                //new SqlParameter("@MaterialCode",objMaterial.MaterialCode),
                //new SqlParameter("@FactoryCode",objMaterial.FactoryCode),
                //new SqlParameter("@IsPublic",objMaterial.IsPublic),
                //new SqlParameter("@MaterialCategoryId",objMaterial.MaterialCategoryId),
                //new SqlParameter("@DrawingCode",objMaterial.DrawingCode),
                new SqlParameter("@MaterialName",objMaterial.MaterialName),
                new SqlParameter("@MaterialSpec",objMaterial.MaterialSpec),
                new SqlParameter("@MaterialType",objMaterial.MaterialType),
                new SqlParameter("@Mquality",objMaterial.Mquality),
                new SqlParameter("@Weight",objMaterial.Weight),
                new SqlParameter("@Unit",objMaterial.Unit),
                new SqlParameter("@ImportanceGrade",ConvertEx.SqlNull(objMaterial.ImportanceGrade)),
                new SqlParameter("@DraftFeatureId",ConvertEx.SqlNull(objMaterial.DraftFeatureId)),
                new SqlParameter("@MachiningPropertyId",ConvertEx.SqlNull(objMaterial.MachiningPropertyId)),
                new SqlParameter("@PaintingColor",ConvertEx.SqlNull(objMaterial.PaintingColor)),
                new SqlParameter("@HeatTreatment",objMaterial.HeatTreatment),
                new SqlParameter("@SurfaceTreatment",objMaterial.SurfaceTreatment),
                new SqlParameter("@BrandId",ConvertEx.SqlNull(objMaterial.BrandId)),
                //new SqlParameter("@StatusId",objMaterial.StatusId),
                //new SqlParameter("@Revision",objMaterial.Revision),
                //new SqlParameter("@DocIdModel",objMaterial.DocIdModel),
                new SqlParameter("@ReMark",objMaterial.ReMark),
                new SqlParameter("@UpdateId",objMaterial.UpdateId),
                new SqlParameter("@UpdateUser",objMaterial.UpdateUser),
                new SqlParameter("@UpdateDate",objMaterial.UpdateDate),
                new SqlParameter("@UpdateInfo",objMaterial.UpdateInfo)
            };
            int result = SQLHelper.Update(sql, param, false);
            return result != 0;
        }
        #endregion

        #region 获取新的物料编码，主要是获取4位流水码

        /// <summary>
        /// 获取新的物料编码（首先检查历史空位置）
        /// 2021年1月5日 04:58:47
        /// </summary>
        /// <param name="materialClassId"></param>
        /// <returns></returns>
        public string GetNewMaterialCode(string materialClassId)
        {
            DataTable dt = GetMaterialByClassId(materialClassId);//包含禁用的物料(StatusId=4)
            string flowNo = string.Empty;
            List<int> flowNoList = new List<int>();
            //【1】根据父节点判断流水码位数
            int bitNum = 0;//流水码位数
            if (materialClassId.StartsWith("7"))
            {
                bitNum = 5;
            }
            else
            {
                bitNum = 4;
            }
            //【2】计算流水号（新方法，自动补位）
            if (dt.Rows.Count > 0)
            {
                string oldCode = string.Empty;
                int n = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    oldCode = dt.Rows[i]["MaterialCode"].ToString();
                    n = Convert.ToInt32(oldCode.Substring(oldCode.Length - bitNum));
                    flowNoList.Add(n);
                }
            }
            //【2021年1月5日 04:32:50】验证后结论：List不需要去重
            flowNo = Globals.GetFlowNo(flowNoList, 1, bitNum);//根据父节点判断流水码位数
            //if (flowNo == "10000") flowNo = "超出范围";
            flowNo = materialClassId + flowNo;
            return flowNo;
        }

        /// <summary>
        /// 根据物料分类获取已有物料编码
        /// 2021年1月5日 04:59:29
        /// </summary>
        /// <param name="materialClassId">物料分类</param>
        /// <returns></returns>
        public DataTable GetMaterialByClassId(string materialClassId)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            //包含状态=4的禁用物料
            string sql = string.Format("SELECT * FROM code_Material WHERE StatusId < 5 and MaterialClassId='{0}'", materialClassId);
            return SQLHelper.GetDataSet(sql).Tables[0];
        }

        #endregion

        #region 是否是唯一的物料（三个条件+自身MaterialId）

        //是否是唯一的物料（三个条件+自身MaterialId）
        public DataRow IsUniqueMaterial(string materialclassId, string parameter1, string parameter2, string materialId = null)
        {
            SQLHelper.connString = Globals.connStrGroupDBName;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM code_Material WHERE StatusId < 5 ");//默认不显示历史（5）【包含禁用（4）的数据】
            if (materialclassId.StartsWith("3"))//自制件3开头，图号DrawingCode+颜色PaintingColor
            {
                if (string.IsNullOrEmpty(parameter2))
                {
                    sb.Append(" AND DrawingCode='" + parameter1 + "' and (PaintingColor IS NULL OR PaintingColor='')");
                }
                else
                {
                    sb.Append(" AND DrawingCode='" + parameter1 + "' and PaintingColor='" + parameter2 + "' ");
                }
            }
            else if (materialclassId.StartsWith("50") || materialclassId.StartsWith("51"))//外购件50、51开头，型号MaterialType+品牌BrandId
            {
                if (string.IsNullOrEmpty(parameter2))
                {
                    sb.Append(" AND MaterialType='" + parameter1 + "' and (BrandId IS NULL OR BrandId='')");
                }
                else
                {
                    sb.Append(" AND MaterialType='" + parameter1 + "' and BrandId='" + parameter2 + "' ");
                }
            }
            else if (materialclassId.StartsWith("52"))//标准件52开头，型号MaterialType+名称MaterialName
            {
                sb.Append(" AND MaterialType='" + parameter1 + "' and MaterialName='" + parameter2 + "' ");
            }
            else//其他的，图号DrawingCode+名称MaterialName
            {
                sb.Append(" AND DrawingCode='" + parameter1 + "' and MaterialName='" + parameter2 + "' ");
            }
            if (String.IsNullOrEmpty(materialId) == false)
            {
                sb.Append(" AND MaterialId <> '" + materialId + "' ");
            }
            //DataTable dt = DataProvider.Instance.GetTable(_Loginer.GroupDBName, sb.ToString(), "IsUniqueMaterial");
            DataTable dt = SQLHelper.GetDataSet(sb.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 基础验证（插件版本、登录名、服务器时间）
        //获取插件版本SWCodeVersion
        public string GetSWCodeVersion(string programname)
        {
            try
            {
                SQLHelper.connString = Globals.connStrFactoryDBName;
                string sql = string.Format("select top 1 Revision from sys_ProgramInfo where ProgramName='{0}' order by progid DESC", programname);
                return SQLHelper.GetSingleResult(sql).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //通过查询获取编码系统的用户信息
        public DataRow GetUserInfo(string username)
        {
            SQLHelper.connString = Globals.connStrFactoryDBName;
            string sql = string.Format("select * from tb_MyUser where PDMAccount='{0}'", username);
            DataTable dt = SQLHelper.GetDataSet(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

        //获取服务器时间
        public DateTime GetDBServerTime()
        {
            return SQLHelper.GetDBServerTime();
        }
        #endregion
    }
}
