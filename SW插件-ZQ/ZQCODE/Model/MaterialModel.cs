using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZQCode
{
    /// <summary>
    /// 物料编码实体类
    /// </summary>
    public class MaterialModel
    {
        public string MaterialId { get; set; }
        public string MaterialClassId { get; set; }
        public string MaterialCode { get; set; }
        public string FactoryCode { get; set; }
        public int StatusId { get; set; }
        public int Revision { get; set; }
        public string MaterialCategoryId { get; set; }
        public string DrawingCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialSpec { get; set; }
        public string MaterialType { get; set; }
        public string Mquality { get; set; }
        public string Weight { get; set; }
        //public Decimal Weight { get; set; }
        public string Unit { get; set; }
        public string SubUnit { get; set; }
        public string ImportanceGrade { get; set; }
        public string DraftFeatureId { get; set; }
        public string MachiningPropertyId { get; set; }
        public string PaintingColor { get; set; }
        public string HeatTreatment { get; set; }
        public string SurfaceTreatment { get; set; }
        public string BrandId { get; set; }
        public int DocIdModel { get; set; }
        public int DocIdDrawing { get; set; }
        public int DocIdProcess { get; set; }
        public int DocIdOther { get; set; }
        public string Image { get; set; }
        public string ReMark { get; set; }
        public bool IsPublic { get; set; }
        public bool IsPackage { get; set; }
        public bool Deleted { get; set; }
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyProperty3 { get; set; }
        public string MyProperty4 { get; set; }
        public string MyProperty5 { get; set; }
        public string MyProperty6 { get; set; }
        public string MyProperty7 { get; set; }
        public string MyProperty8 { get; set; }
        public string MyProperty9 { get; set; }
        public string CreateFrom { get; set; }
        public string CreateId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateInfo { get; set; }
        public string AppFlag { get; set; }
        public string AppId { get; set; }
        public string AppUser { get; set; }
        public DateTime AppDate { get; set; }
        public string AppInfo { get; set; }
    }
}
