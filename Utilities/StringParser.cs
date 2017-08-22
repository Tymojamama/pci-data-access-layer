using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensionConsultants.Data.Utilities
{
    public static class StringParser
    {
        public static void Parse(string _stringValue, Type targetType, out object _object)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _object = null;
                return;
            }
            else if (targetType == typeof(DateTime))
            {
                DateTime dateTimeValue;
                Parse(_stringValue, out dateTimeValue);
                _object = dateTimeValue;
                return;
            }
            else if (targetType == typeof(DateTime?))
            {
                DateTime? dateTimeValue;
                Parse(_stringValue, out dateTimeValue);
                _object = dateTimeValue;
                return;
            }
            else if (targetType == typeof(Decimal))
            {
                Decimal decimalValue;
                Parse(_stringValue, out decimalValue);
                _object = decimalValue;
                return;
            }
            else if (targetType == typeof(Decimal?))
            {
                Decimal? decimalValue;
                Parse(_stringValue, out decimalValue);
                _object = decimalValue;
                return;
            }
            else if (targetType == typeof(Guid))
            {
                Guid guidValue;
                Parse(_stringValue, out guidValue);
                _object = guidValue;
                return;
            }
            else if (targetType == typeof(Guid?))
            {
                Guid? guidValue;
                Parse(_stringValue, out guidValue);
                _object = guidValue;
                return;
            }
            else if (targetType == typeof(int))
            {
                int intValue;
                Parse(_stringValue, out intValue);
                _object = intValue;
                return;
            }
            else if (targetType == typeof(int?))
            {
                int intValue;
                Parse(_stringValue, out intValue);
                _object = intValue;
                return;
            }
            else if (targetType == typeof(SqlBoolean))
            {
                SqlBoolean sqlBooleanValue;
                Parse(_stringValue, out sqlBooleanValue);
                _object = sqlBooleanValue;
                return;
            }
            else if (targetType == typeof(SqlBoolean?))
            {
                SqlBoolean? sqlBooleanValue;
                Parse(_stringValue, out sqlBooleanValue);
                _object = sqlBooleanValue;
                return;
            }
            else if (targetType == typeof(string))
            {
                string stringValue;
                Parse(_stringValue, out stringValue);
                _object = stringValue;
                return;
            }
            else
            {
                throw new NotSupportedException("Target type is not currently supported.");
            }
        }

        public static void Parse(string _stringValue, out DateTime _dateTime)
        {
            _dateTime = DateTime.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out DateTime? _dateTime)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _dateTime = null;
                return;
            }

            _dateTime = DateTime.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out Decimal _decimal)
        {
            _decimal = Decimal.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out Decimal? _decimal)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _decimal = null;
                return;
            }

            _decimal = Decimal.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out Guid _guid)
        {
            _guid = new Guid(_stringValue);
        }

        public static void Parse(string _stringValue, out Guid? _guid)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _guid = null;
                return;
            }

            _guid = new Guid(_stringValue);
        }

        public static void Parse(string _stringValue, out int _int)
        {
            _int = Int32.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out int? _int)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _int = null;
                return;
            }

            _int = Int32.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out SqlBoolean _sqlBoolean)
        {
            _sqlBoolean = SqlBoolean.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out SqlBoolean? _sqlBoolean)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _sqlBoolean = null;
                return;
            }

            _sqlBoolean = SqlBoolean.Parse(_stringValue);
        }

        public static void Parse(string _stringValue, out string _value)
        {
            if (String.IsNullOrWhiteSpace(_stringValue))
            {
                _value = null;
                return;
            }

            _value = _stringValue;
        }
    }
}
