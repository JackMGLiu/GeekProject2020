using System.Threading.Tasks;
using Geek.Project.Service.System;
using Geek.ProjectCore.DTO.Output;
using Microsoft.AspNetCore.Mvc;

namespace Geek.Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ISysRoleService _sysRoleService;

        public RoleController(ISysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _sysRoleService.GetAsync(id);
        }

        /// <summary>
        /// 查询多条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IResponseOutput> GetList(string name)
        {
            return await _sysRoleService.GetAsyncByWhere(name);
        }
    }
}
