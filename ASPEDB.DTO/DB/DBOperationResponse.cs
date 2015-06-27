using System;
using System.Runtime.Serialization;

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
