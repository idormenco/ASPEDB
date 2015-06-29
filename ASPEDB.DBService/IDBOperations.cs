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
        IList<EncryptedDBPoint> Search(EncryptedDBQuery query);

        [OperationContract]
        bool Update(EncryptedDBQuery query, EncryptedDBPoint newPoint);

        [OperationContract]
        bool Delete(EncryptedDBQuery query);

        [OperationContract]
        DBOperationResponse Insert(EncryptedDBPoint dbPoint);
    }
}
