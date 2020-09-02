using Geek.Project.Domain.System;
using Geek.ProjectCore.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geek.Project.Repository.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISysRoleRepository : IRepositoryBase<SysRole, string>
    {
        /// <summary>
        /// 根据主键查询角色信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IEnumerable<SysRole>> GetRoles(string key);
    }
}
