using System.Collections.Generic;
using System.ServiceModel;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;

namespace ASPEDB.DBService
{
    [ServiceContract]
    public interface IDBOperations
    {
        [OperationContract]
        string Hello();

        [OperationContract]
        IList<EncryptedDBPoint> Search(EncryptedQuery query);

        [OperationContract]
        bool Update(EncryptedQuery query, EncryptedPoint newPoint);

        [OperationContract]
        bool Delete(EncryptedQuery query);

        [OperationContract]
        DBOperationResponse Insert(EncryptedDBPoint dbPoint);
    }
}
