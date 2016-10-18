using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Common.DataAccess
{
    /// <summary>
    /// ENum to identify type of Command Object.
    /// </summary>
    public enum QueryCommandType
    {
        Text,
        StoredProcedure
    }

    public enum ParameterType
    {
        BFile = 101,
        Blob = 102,
        Byte = 103,
        Char = 104,
        Clob = 105,
        Date = 106,
        Decimal = 107,
        Double = 108,
        Long = 109,
        LongRaw = 110,
        Int16 = 111,
        Int32 = 112,
        Int64 = 113,
        IntervalDS = 114,
        IntervalYM = 115,
        NClob = 116,
        NChar = 117,
        NVarchar2 = 119,
        Raw = 120,
        RefCursor = 121,
        Single = 122,
        TimeStamp = 123,
        TimeStampLTZ = 124,
        TimeStampTZ = 125,
        Varchar2 = 126,
        XmlType = 127,
        Array = 128,
        Object = 129,
        Ref = 130,
        BinaryDouble = 132,
        BinaryFloat = 133
    }
    /// <summary>
    /// A list of data providers
    /// </summary>
    public enum Providers
    {
        SqlServer,
        OleDB,
        ODBC,
        Oracle
    }
}