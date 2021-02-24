using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZQCode
{
    /// <summary>
    /// 图号实体类
    /// </summary>
    public class DrawingModel
    {
        public string FileName { get; set; }//文件名
        public string ConfigName { get; set; }//配置名
        public string DrawingId { get; set; }
        public string DrawingClassId { get; set; }
        public string DrawingCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialSpec { get; set; }
        //public string MaterialDesc { get; set; }
        //public string Mquality { get; set; }
        public string Weight { get; set; }
        public string Unit { get; set; }
        public string HeatTreatment { get; set; }
        public string SurfaceTreatment { get; set; }
        public string Brand { get; set; }
        //public string SupplierId { get; set; }
        public string PurchaseTypeId { get; set; }
        public string SelectionTypeId { get; set; }
        public int DrawingStatusId { get; set; }//状态（默认为0）
        public int Revision { get; set; }//版本（默认为1）
        public int DocIdModel { get; set; }
        public string ReMark { get; set; }
        public string CreateFrom { get; set; }
        public string CreateId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateId { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateInfo { get; set; }
    }
}
