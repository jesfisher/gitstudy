using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZQCode
{
    public class CodeLibraryModel
    {
        public Guid ID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
