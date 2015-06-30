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
        DBOperationResponse Update(EncryptedDBQuery query, EncryptedDBValue newValue);

        [OperationContract]
        DBOperationResponse Delete(EncryptedDBQuery query);

        [OperationContract]
        DBOperationResponse Insert(EncryptedDBPoint dbPoint);
    }
}
