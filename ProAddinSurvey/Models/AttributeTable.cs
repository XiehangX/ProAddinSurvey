using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.Models
{
    public class AttributeTable
    {
        public string 序号 { get; set; }
        public string 字段名称 { get; set; }
        public string 字段代码 { get; set; }
        public string 字段类型 { get; set; }
        public string 字段长度 { get; set; }
        public string 小数位数 { get; set; }
        public string 值域 { get; set; }
        public string 约束条件 { get; set; }
        public string 数据来源 { get; set; }
        public string 对应字段 { get; set; }
    }
}
