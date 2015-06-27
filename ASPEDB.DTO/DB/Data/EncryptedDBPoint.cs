using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class EncryptedDBPoint
    {
        [DataMember]
        public EncryptedDBValue Type { get; set; }
        [DataMember]
        public EncryptedDBValue Name { get; set; }
        [DataMember]
        public EncryptedDBValue Value { get; set; }

        public EncryptedDBPoint(EncryptedDBValue type, EncryptedDBValue name, EncryptedDBValue value)
        {
            this.Type = new EncryptedDBValue(type);
            this.Name = new EncryptedDBValue(name);
            this.Value = new EncryptedDBValue(value);
        }
        public EncryptedDBPoint(EncryptedDBPoint edbp):this(edbp.Type,edbp.Name,edbp.Value)
        {
        }
        
    }
}