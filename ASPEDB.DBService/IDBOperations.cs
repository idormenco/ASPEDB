using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ASPEDB.DBService
{
    [ServiceContract]
    public interface IDBOperations
    {
        [OperationContract]
        string Hello();
    }
}
