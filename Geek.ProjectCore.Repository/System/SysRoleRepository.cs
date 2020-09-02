using FreeSql;
using Geek.Project.Domain.System;
using Project.Core.Domain.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geek.Project.Repository.System
{
    public class SysRoleRepository : RepositoryBase<SysRole, string>, ISysRoleRepository
    {
        public SysRoleRepository(UnitOfWorkManager uowm) : base(uowm)
        {

        }

        public async Task<IEnumerable<SysRole>> GetRoles(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var sql = "SELECT * FROM SYSROLE WHERE Id=@Id";
                var data = await base.Orm.Ado.QueryAsync<SysRole>(sql, new { Id = key });
                return data;
            }
            return null;
        }
    }
}
