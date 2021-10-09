using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.Models
{
    /// <summary>
    /// 数据源设置
    /// </summary>
    public class DataSourceSetting
    {
        /// <summary>
        /// 市级行政区划
        /// </summary>
        public string AdminArea_City { get; set; }

        /// <summary>
        /// 县级行政区划
        /// </summary>
        public string AdminArea_Country { get; set; }

        /// <summary>
        /// 乡镇级行政区划
        /// </summary>
        public string AdminArea_Town { get; set; }

        /// <summary>
        /// 地类图斑
        /// </summary>
        public string DLTB { get; set; }

        /// <summary>
        /// 基本农田
        /// </summary>
        public string JBNT { get; set; }

        /// <summary>
        /// 生态红线
        /// </summary>
        public string STHX { get; set; }
    }
}
