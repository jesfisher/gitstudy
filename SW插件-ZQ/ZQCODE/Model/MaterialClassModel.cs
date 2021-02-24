using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZQCode
{
    public class MaterialClassModel
    {
        public int MaterialClassId { get; set; }
        public int ParentId { get; set; }
        public string MaterialClassName { get; set; }
        public string FactoryCode { get; set; }
        public int IsPublic { get; set; }
        public string ReMark { get; set; }
        public string UpdateId { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateInfo { get; set; }
        //扩展属性（添加Add，修改Modify）
        public string ActionType { get; set; }
    }
}
