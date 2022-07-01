using Dapper;
using Web.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Repository
{
    public interface ISqlInjectionRepository
    {
        (string Sql, List<MembersModel> Result) GetMembers(string familyName);
    }

    public class SqlInjectionRepository : CommonRepository, ISqlInjectionRepository
    {
        public SqlInjectionRepository(DbConnection connection)
            : base(connection)
        {
        }

        public (string Sql, List<MembersModel> Result) GetMembers(string familyName)
        {

            string sql =
$@"
SELECT name, deleted_flag FROM members
 WHERE deleted_flag = 0
 {(string.IsNullOrEmpty(familyName) ? "" : $@" AND name LIKE '%{familyName}%' ")}
";

            var result = Connection.Query<MembersModel>(sql).ToList();
            return (sql, result);
        }
    }
}
