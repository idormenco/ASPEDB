using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public enum Operator
    {
        [EnumMember]
        NotEqual,       // !=
        [EnumMember]
        Less,           // <
        [EnumMember]
        LessEqual,      // <=
        [EnumMember]
        Equal,          // ==
        [EnumMember]
        GreaterEqual,   // >=
        [EnumMember]
        Greater,        // >
        [EnumMember]
        ExactBetween,   // > <
        [EnumMember]
        BetweenDown,    // >= <
        [EnumMember]
        Between,        // >= <=
        [EnumMember]
        BetweenUp       // > <=
    }
}
