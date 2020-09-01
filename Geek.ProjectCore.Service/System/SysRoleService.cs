using Geek.Project.Repository.System;
using Geek.Project.Service.DTO.System.SysRole;
using Geek.ProjectCore.DTO.Output;
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
    }
}
