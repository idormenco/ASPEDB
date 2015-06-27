using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class DBQuery
    {
        [DataMember]
        public decimal Type { get; set; }
        [DataMember]
        public decimal Name { get; set; }
        [DataMember]
        public Operator Operator { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public decimal OptionalValue { get; set; }

        public DBQuery(decimal type, decimal name, Operator @operator, decimal value, decimal optionalValue)
        {
            Type = type;
            Name = name;
            Operator = @operator;
            Value = value;
            OptionalValue = optionalValue;
        }

        public DBQuery(DBQuery dbQuery)
            : this(dbQuery.Type, dbQuery.Name, dbQuery.Operator, dbQuery.Value, dbQuery.OptionalValue)
        {

        }
    }
}
