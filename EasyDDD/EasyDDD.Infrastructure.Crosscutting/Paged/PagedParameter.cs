using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDDD.Infrastructure.Crosscutting.Paged
{
    public class PagedParameter
    {
        /// <summary>
        /// 获取或设置页面大小。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置页码。
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 排序字段 格式：  +id,-name   按照id 升序，name降序排序
        /// </summary>
        public string OrderBy { get; set; }
    }
}
