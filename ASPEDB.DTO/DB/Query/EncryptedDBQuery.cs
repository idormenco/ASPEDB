using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class EncryptedDBQuery
    {
        [DataMember]
        public EncryptedQuery Type { get; set; }
        [DataMember]
        public EncryptedQuery Name { get; set; }
        [DataMember]
        public EncryptedQuery Value { get; set; }
        [DataMember]
        public Operator Operator { get; set; }
        [DataMember]
        public EncryptedQuery OptionalValue { get; set; }

        public EncryptedDBQuery(EncryptedQuery type, EncryptedQuery name, Operator @operator, EncryptedQuery value, EncryptedQuery optionalValue)
        {
            this.Type = new EncryptedQuery(type);
            this.Name = new EncryptedQuery(name);
            this.Operator = @operator;
            this.Value = new EncryptedQuery(value);
            if (optionalValue != null)
                this.OptionalValue = new EncryptedQuery(optionalValue);
        }

        public EncryptedDBQuery(EncryptedDBQuery edbq)
            : this(edbq.Type, edbq.Name, edbq.Operator, edbq.Value, edbq.OptionalValue)
        {

        }
    }
}
