using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BoardModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string comment { get; set; }
        public int deleted_flag { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
