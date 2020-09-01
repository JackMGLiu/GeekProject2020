using Geek.Project.Repository.System;
using Geek.Project.Service.DTO.System.SysRole;
using Geek.ProjectCore.DTO.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geek.Project.Service.System
{
    public class SysRoleService : ISysRoleService
    {
        private readonly ISysRoleRepository sysRoleRepository;

        public SysRoleService(ISysRoleRepository sysRoleRepository)
        {
            this.sysRoleRepository = sysRoleRepository;
        }

        public async Task<IResponseOutput> GetAsync(string id)
        {
            var result = await sysRoleRepository.GetAsync<RoleGetOutput>(id);
            return ResponseOutput.Ok(result, "查询成功");
        }

        public async Task<IResponseOutput> GetAsyncByWhere(string roleName)
        {
            var result = await sysRoleRepository.Where(r => r.RoleName.Contains(roleName)).ToListAsync<RoleGetOutput>();
            return ResponseOutput.Ok(result, "查询成功");
        }
    }
}
