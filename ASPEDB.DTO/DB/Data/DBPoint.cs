using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class DBPoint
    {
        [DataMember]
        public decimal Type { get; set; }
        [DataMember]
        public decimal Name { get; set; }
        [DataMember]
        public decimal Value { get; set; }

        public DBPoint(decimal type, decimal name, decimal value)
        {
            this.Type = type;
            this.Name = name;
            this.Value = value;
        }

        public DBPoint(DBPoint dbPoint)
            : this(dbPoint.Type, dbPoint.Name, dbPoint.Value)
        {
        }
    }
}
