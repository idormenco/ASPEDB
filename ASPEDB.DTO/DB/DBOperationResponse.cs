using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class DBOperationResponse
    {
        [DataMember]
        public bool IsOperationExecuted { get; set; }
        [DataMember]
        public string Message { get; set; }

        public DBOperationResponse(bool opExecuted, string message)
        {
            this.IsOperationExecuted = opExecuted;
            this.Message = message;
        }
    }
}
