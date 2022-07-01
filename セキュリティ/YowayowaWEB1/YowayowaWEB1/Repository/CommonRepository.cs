using MySql.Data.MySqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using Web.Models;
using Dapper;
using System.Data.Common;
using System.Data;

namespace Repository
{
    public class CommonRepository
    {
        const int DefaultCommandTimeOut = 300;


        DbConnection _connection;
        /// <summary>
        /// DB接続用のIDbConnectionを取得・設定する。
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="connection">IDbConnectionを外部から渡したい場合に使用。</param>
        public CommonRepository(DbConnection connection)
        {
            _connection = connection;
        }


    }
}
