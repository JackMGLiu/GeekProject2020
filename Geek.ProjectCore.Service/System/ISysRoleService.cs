﻿using Geek.ProjectCore.DTO.Output;
using System.Threading.Tasks;

namespace Geek.Project.Service.System
{
    public interface ISysRoleService
    {
        Task<IResponseOutput> GetAsync(string id);
        Task<IResponseOutput> GetDataByKey(string key);

        Task<IResponseOutput> GetAsyncByWhere(string roleName);
    }
}
