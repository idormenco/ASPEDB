using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class UnEncryptedDBQuery
    {
        [DataMember]
        public Query Type { get; set; }
        [DataMember]
        public Query Name { get; set; }
        [DataMember]
        public Query Value { get; set; }
        [DataMember]
        public Operator Operator { get; set; }
        [DataMember]
        public Query OptionalValue { get; set; }

        public UnEncryptedDBQuery(decimal[] type, decimal[] name, Operator @operator, decimal[] value, decimal[] optionalValue)
        {
            Type = new Query(type);
            Name = new Query(name);
            Value = new Query(value);
            Operator = @operator;
            if (optionalValue != null)
                OptionalValue = new Query(optionalValue);
        }
        public UnEncryptedDBQuery(Query type, Query name, Operator @operator, Query value, Query optionalValue)
            : this(type.q, name.q, @operator, value.q, optionalValue != null ? optionalValue.q : null)
        {

        }

        public UnEncryptedDBQuery(UnEncryptedDBQuery uedbq)
            : this(uedbq.Type, uedbq.Name, uedbq.Operator, uedbq.Value, uedbq.OptionalValue)
        {

        }
    }
}
