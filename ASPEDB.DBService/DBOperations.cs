using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPEDB.DBService
{
    public class DBOperations : IDBOperations
    {
        public string Hello()
        {
            return "hello";
        }


        public IList<DTO.EncryptedPoint> Search(DTO.Query.EncryptedQuery query)
        {
            throw new NotImplementedException();
        }

        public bool Update(DTO.Query.EncryptedQuery query, DTO.EncryptedPoint newPoint)
        {
            throw new NotImplementedException();
        }

        public bool Delete(DTO.Query.EncryptedQuery query)
        {
            throw new NotImplementedException();
        }

        public bool Insert(DTO.EncryptedPoint point)
        {
            throw new NotImplementedException();
        }
    }
}
