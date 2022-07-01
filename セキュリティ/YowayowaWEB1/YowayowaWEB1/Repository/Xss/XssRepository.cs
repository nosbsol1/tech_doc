using Dapper;
using Web.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Repository
{
    public interface IXssRepository
    {
        (string Sql, List<BoardModel> Result) GetBoard();
        int InsertBoard(BoardModel model);
    }

    public class XssRepository : CommonRepository, IXssRepository
    {
        public XssRepository(DbConnection connection)
            : base(connection)
        {
        }

        public (string Sql, List<BoardModel> Result) GetBoard()
        {

            string sql =
$@"
SELECT * FROM board_xss
 WHERE deleted_flag = 0
 ORDER BY created_at desc
";

            var result = Connection.Query<BoardModel>(sql).ToList();
            return (sql, result);
        }

        public int InsertBoard(BoardModel model)
        {

            string sql =
$@"
INSERT INTO board_xss (name, comment) VALUES (@name, @comment);
";

            var result = Connection.Execute(sql, model);
            return result;
        }


    }
}
