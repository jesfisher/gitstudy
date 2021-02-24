using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;

namespace ZQCode
{
    public static class swAppHelper
    {
        public static SldWorks swApp;

        #region 启动/退出SW程序

        /// <summary>
        /// 启动SW程序
        /// </summary>
        public static void StartSW()
        {
            swApp = new SldWorks();
            swApp.Visible = true;
        }
        /// <summary>
        /// 退出SW程序
        /// </summary>
        public static void ExitSW()
        {
            if (swApp != null)
            {
                swApp.ExitApp();
                swApp = null;
            }
        }

        #endregion

        #region 打开/关闭SW文件

        /// <summary>
        /// 打开SW文件(指定的文件名)
        /// </summary>
        public static ModelDoc2 OpenDoc(string fileName)
        {
            StartSW();
            string swDocTypes = fileName.Substring(fileName.Length - 6, 6);
            int intType = 0;
            //区分后缀的大小写
            switch (swDocTypes)
            {
                case "sldprt":
                    intType = 1;
                    break;
                case "SLDPRT":
                    intType = 1;
                    break;
                case "sldasm":
                    intType = 2;
                    break;
                case "SLDASM":
                    intType = 2;
                    break;
                case "slddrw":
                    intType = 3;
                    break;
                case "SLDDRW":
                    intType = 3;
                    break;
                default:
                    intType = 0;
                    break;
            }
            ModelDoc2 swDoc = swApp.OpenDoc6(fileName, intType, 1, "", 0, 0);
            swDoc.Visible = true;
            //swApp.OpenDoc(fileName, intType); 
            return swDoc;
        }
        /// <summary>
        /// 关闭SW文件（指定的文件名）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CloseDoc(string fileName)
        {
            if (swApp != null)
            {
                swApp.CloseDoc(fileName);
            }
        }

        public static void CloseAllDocuments()
        {
            if (swApp != null)
            {
                swApp.CloseAllDocuments(true);
            }
        }

        #endregion

        #region 当前激活文档/激活指定文档

        /// <summary>
        /// 当前激活文档
        /// </summary>
        /// <returns></returns>
        public static ModelDoc2 ActiveDoc()
        {
            StartSW();
            ModelDoc2 swDoc = swApp.ActiveDoc;
            return swDoc;
        }

        /// <summary>
        /// 激活指定文档
        /// </summary>
        /// <returns></returns>
        public static ModelDoc2 ActiveDoc(string filename)
        {
            StartSW();
            ModelDoc2 swDoc = swApp.ActivateDoc(filename);
            return swDoc;
        }
        /// <summary>
        /// 获取当前激活文档的文件名
        /// </summary>
        /// <returns></returns>
        public static string GetActiveDocName()
        {
            StartSW();
            ModelDoc2 swDoc = swApp.ActiveDoc;
            if (swDoc == null)
            {
                return "error";
            }
            string fileName = swDoc.GetPathName();
            return fileName;
        }

        #endregion

        #region 新建SW文件（零件、装配体、工程图）
        public static ModelDoc2 NewDoc()
        {
            StartSW();
            ModelDoc2 swDoc = swApp.NewAssembly();
            return swDoc;
        }

        #endregion

        #region 获取当前文件的Drawing属性

        //获取当前文件的Drawing属性
        public static DrawingModel GetDrawingInfo()
        {
            DrawingModel objDrawing = new DrawingModel();
            //【1】打开指定的SW文件，并获取文件名
            ModelDoc2 swDoc = swAppHelper.ActiveDoc();
            string fileName = swDoc.GetPathName();//文件名
            string configName = swDoc.ConfigurationManager.ActiveConfiguration.Name;//当前激活配置名
            if (fileName == null || fileName.Length == 0)
            {
                Msg.ShowError("无法获取文件名，请保存文件后重试！");
                return null;
            }

            //【2】从模型中获取主要属性值
            string drawingcode = swDoc.GetCustomInfoValue(configName, "图号");
            string materialname = swDoc.GetCustomInfoValue(configName, "名称");
            string materialspec = swDoc.GetCustomInfoValue(configName, "规格型号");
            string weight = swDoc.GetCustomInfoValue(configName, "重量");
            string unit = swDoc.GetCustomInfoValue(configName, "单位");
            string purchasetypeid = swDoc.GetCustomInfoValue(configName, "采购类型");
            string selectiontypeid = swDoc.GetCustomInfoValue(configName, "选型分类");
            string surfacetreatment = swDoc.GetCustomInfoValue(configName, "表面处理");
            string heattreatment = swDoc.GetCustomInfoValue(configName, "热处理");
            string brand = swDoc.GetCustomInfoValue(configName, "品牌");
            string remark = swDoc.GetCustomInfoValue(configName, "备注");
            //【3】封装图号实体类对象属性
            objDrawing.FileName = fileName;
            objDrawing.ConfigName = configName;
            objDrawing.DrawingCode = drawingcode;
            objDrawing.MaterialName = materialname;
            objDrawing.MaterialSpec = materialspec;//XorG：规格型号
            objDrawing.Weight = weight;
            objDrawing.Unit = unit;
            objDrawing.PurchaseTypeId = purchasetypeid;
            objDrawing.SelectionTypeId = selectiontypeid;
            objDrawing.SurfaceTreatment = surfacetreatment;
            objDrawing.HeatTreatment = heattreatment;
            objDrawing.Brand = brand;
            objDrawing.ReMark = remark;
            return objDrawing;
        }

        //获取当前文件的Material属性
        public static MaterialModel GetMaterialInfo()
        {

            //【1】打开指定的SW文件，并获取文件名
            ModelDoc2 swDoc = swAppHelper.ActiveDoc();
            string fileName = swDoc.GetPathName();//文件名

            if (fileName == null || fileName.Length == 0)
            {
                Msg.ShowError("无法获取文件名，请保存文件后重试！");
                return null;
            }
            //【2】从模型中获取属性并封装到物料实体类中
            //2.1-配置特定属性
            //string configName = swDoc.ConfigurationManager.ActiveConfiguration.Name;//当前激活配置名            
            //2.2-自定义属性
            string configName = "";//特别说明：获取自定义属性时，只需要将配置名设置为空白""即可
            MaterialModel objMaterial = new MaterialModel()
            {
                //DrawingCode = swDoc.GetCustomInfoValue(configName, "图号"),
                //MaterialName = swDoc.GetCustomInfoValue(configName, "名称"),
                //DrawingCode = swDoc.GetCustomInfoValue(configName, "Identification Nb"),
                MaterialName = swDoc.GetCustomInfoValue(configName, "Description"),
                MaterialSpec = swDoc.GetCustomInfoValue(configName, "规格"),
                MaterialType = swDoc.GetCustomInfoValue(configName, "型号"),
                Unit = swDoc.GetCustomInfoValue(configName, "单位"),
                Mquality = swDoc.GetCustomInfoValue(configName, "Material"),
                Weight = swDoc.GetCustomInfoValue(configName, "Weight"),
                MachiningPropertyId = swDoc.GetCustomInfoValue(configName, "加工属性"),
                PaintingColor = swDoc.GetCustomInfoValue(configName, "涂装颜色"),
                BrandId = swDoc.GetCustomInfoValue(configName, "品牌")
            };
            return objMaterial;
        }

        //获取当前文件的配置名
        public static string GetConfigName()
        {
            //【1】打开指定的SW文件，并获取文件名
            ModelDoc2 swDoc = swAppHelper.ActiveDoc();
            string fileName = swDoc.GetPathName();//文件名
            string configName = swDoc.ConfigurationManager.ActiveConfiguration.Name;//当前激活配置名
            return configName;
        }

        //获取当前文件的文件名
        public static string GetFileName()
        {
            string fileName = string.Empty;
            //【1】打开指定的SW文件，并获取文件名
            ModelDoc2 swDoc = swAppHelper.ActiveDoc();
            if (swDoc != null)
            {
                fileName = swDoc.GetPathName();//文件名
            }
            else
            {
                fileName = null;
            }
            return fileName;
        }
        #endregion

        #region  封装所有的SolidWorks属性（配置）
        public static void UpdateProperty(MaterialModel objMaterial)
        {
            //1.打开指定的SW文件
            ModelDoc2 swDoc = swAppHelper.ActiveDoc(Globals.FileName);
            //2.获取当前激活配置名
            string configName = swDoc.ConfigurationManager.ActiveConfiguration.Name;
            #region 封装所有的SolidWorks属性（配置+自定义）
            //获取属性值：图号、名称、型号规格、单位、热处理、表面处理、品牌、血液
            Dictionary<string, string> MyCustominfos = new Dictionary<string, string>();
            MyCustominfos.Add("Identification Nb", objMaterial.MaterialCode);
            MyCustominfos.Add("Description", objMaterial.MaterialName);
            MyCustominfos.Add("规格", objMaterial.MaterialSpec);
            MyCustominfos.Add("型号", objMaterial.MaterialType);
            MyCustominfos.Add("单位", objMaterial.Unit);
            MyCustominfos.Add("加工属性", objMaterial.MachiningPropertyId);
            MyCustominfos.Add("涂装颜色", objMaterial.PaintingColor);
            MyCustominfos.Add("品牌", objMaterial.BrandId);
            //遍历每个属性并赋值
            foreach (string key in MyCustominfos.Keys)
            {
                //删除，添加配置特定的属性
                swDoc.DeleteCustomInfo2(configName, key);
                swDoc.AddCustomInfo3(configName, key, (int)swCustomInfoType_e.swCustomInfoText, MyCustominfos[key]);
                //删除，添加自定义的属性
                swDoc.DeleteCustomInfo(key);
                swDoc.AddCustomInfo2(key, (int)swCustomInfoType_e.swCustomInfoText, MyCustominfos[key]);
            }
            #endregion

            #region 重建模型
            swDoc.ClearSelection();//清除选择              
            swDoc.ForceRebuild3(true);//重建模型Ctrl+Q
            //swDoc.ShowNamedView2("*等轴测", 7);//等轴测显示
            //swDoc.ViewZoomtofit2();//全屏显示
            //swDoc.Save();
            //swAppHelper.CloseDoc(objDoc.OriginalFileName);
            #endregion
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