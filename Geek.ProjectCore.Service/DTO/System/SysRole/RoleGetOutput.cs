using System;

namespace Geek.Project.Service.DTO.System.SysRole
{
    public class RoleGetOutput
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public int? SortCode { get; set; }
        public int? Status { get; set; }
        public int? DelFlag { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUserId { get; set; }
        public string Remark { get; set; }
    }
}
