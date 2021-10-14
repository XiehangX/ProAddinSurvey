using ArcGIS.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.Models
{
    public class AttributeTableEntity
    {
        public string 序号 { get; set; }
        public string 字段名称 { get; set; }
        public string 字段代码 { get; set; }
        public string 字段类型 { get; set; }
        public string 字段长度 { get; set; }
        public string 小数位数 { get; set; }
        public string 值域 { get; set; }
        public string 约束条件 { get; set; }
        public string 模式 { get; set; }
        public string 计算方法 { get; set; }
        public string 数据来源 { get; set; }
        public string 对应字段 { get; set; }
        public string 描述 { get; set; }

        /// <summary>
        /// 转为 AddField 的字段描述
        /// AddField_management(in_table, field_name, field_type, { field_precision}, { field_scale}, { field_length}, { field_alias}, { field_is_nullable}, { field_is_required}, { field_domain})
        /// </summary>
        /// <returns></returns>
        public List<object> ToAddFieldDesc()
        {
            string field_name = 字段代码;
            string field_type = 字段类型;
            object field_precision = null;
            object field_scale = null;

            if (!Enum.TryParse(field_type, out FieldType _))
            {
                field_type = "Text";
            }
            string field_alias = 字段名称;
            string fieldLengthStr = 字段长度;
            if (string.IsNullOrEmpty(fieldLengthStr))
                fieldLengthStr = "1000";

            int? field_length = Convert.ToInt32(fieldLengthStr);

            string isNullable = "NULABLE";//: "NON_NULLABLE"

            List<object> arguments = new List<object>
              {
                field_name,
                field_type,
                field_precision,
                field_scale,
                field_length,
                field_alias,
                isNullable
              };
            return arguments;
        }

        /// <summary>
        /// 转为 AddFields 的字段描述
        /// AddFields(in_table, field_description)
        /// field_description [[Field Name, Field Type, {Field Alias}, {Field Length}, { Default Value}, { Field Domain}],...]
        /// </summary>
        /// <returns></returns>
        public string ToAddFieldsDesc()
        {
            string field_name = 字段代码;
            string field_type = 字段类型;

            if (!Enum.TryParse(field_type, out FieldType _))
            {
                field_type = "Text";
            }
            string field_alias = 字段名称;
            string fieldLengthStr = 字段长度;
            if (string.IsNullOrEmpty(fieldLengthStr))
                fieldLengthStr = "1000";

            int? field_length = Convert.ToInt32(fieldLengthStr);

            object defaultValue = "#";
            string domains = "#";

            List<object> arguments = new List<object>
              {
                field_name,
                field_type,
                field_alias,
                field_length,
                defaultValue,
                domains
              };
            var result = string.Join(" ", arguments.ToArray());

            return result;
        }

    }
}
