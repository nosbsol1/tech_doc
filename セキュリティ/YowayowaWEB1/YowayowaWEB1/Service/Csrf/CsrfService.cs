using Web.Models;
using Repository;
using System.Collections.Generic;
using System.Transactions;

namespace Service
{
    public class CsrfService
    {

        private readonly ICsrfRepository repository;

        public CsrfService(ICsrfRepository repository)
        {
            this.repository = repository;
        }

        public (string Sql, List<BoardModel> Result) GetBoard()
        {
            return repository.GetBoard();
        }

        public int InsertBoard(BoardModel model)
        {
            using (var transaction = new TransactionScope())
            {
                var result = repository.InsertBoard(model);
                if(result == 1)
                    transaction.Complete();

                return result;
            }
        }
        

    }
}
