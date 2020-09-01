using FreeSql;
using Geek.Project.Domain.System;
using Project.Core.Domain.Repository;

namespace Geek.Project.Repository.System
{
    public class SysRoleRepository : RepositoryBase<SysRole, string>, ISysRoleRepository
    {
        public SysRoleRepository(UnitOfWorkManager uowm) : base(uowm)
        {

        }
    }
}
