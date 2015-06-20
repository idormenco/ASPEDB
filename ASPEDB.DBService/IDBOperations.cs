using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ASPEDB.DTO;
using ASPEDB.DTO.Query;

namespace ASPEDB.DBService
{
    [ServiceContract]
    public interface IDBOperations
    {
        [OperationContract]
        string Hello();

        [OperationContract]
        IList<EncryptedPoint> Search(EncryptedQuery query);

        [OperationContract]
        bool Update(EncryptedQuery query, EncryptedPoint newPoint);

        [OperationContract]
        bool Delete(EncryptedQuery query);

        [OperationContract]
        bool Insert(EncryptedPoint point);
    }
}
