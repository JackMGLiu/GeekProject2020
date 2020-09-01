using FreeSql.DataAnnotations;
using Geek.ProjectCore.Domain.Model;
using System;

namespace Geek.Project.Domain.System
{
    /// <summary>
    /// 角色实体
    /// </summary>
    [Table(Name = "SysRole")]
    public class SysRole : EntityBase<string>
    {
        [Column(StringLength = 50)]
        public string RoleName { get; set; }
        public int? SortCode { get; set; }
        public int? Status { get; set; }
        public int? DelFlag { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUserId { get; set; }
        [Column(StringLength = 255)]
        public string Remark { get; set; }
    }
}
