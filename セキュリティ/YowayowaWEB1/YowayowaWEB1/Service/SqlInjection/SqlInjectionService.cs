using Web.Models;
using Repository;
using System.Collections.Generic;

namespace Service
{
    public class SqlInjectionService
    {

        private readonly ISqlInjectionRepository repository;

        public SqlInjectionService(ISqlInjectionRepository repository)
        {
            this.repository = repository;
        }

        public (string Sql, List<MembersModel> Result) GetMembers(string familyName)
        {
            return repository.GetMembers(familyName);
        }

    }
}
