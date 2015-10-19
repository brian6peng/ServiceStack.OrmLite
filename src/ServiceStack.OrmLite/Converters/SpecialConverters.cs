using System;

namespace ServiceStack.OrmLite.Converters
{
    public class EnumConverter : IntegerConverter
    {
        public override object ToDbValue(Type fieldType, object value)
        {
            return (int)value;
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            return ((int)value).ToString();
        }
    }

    public class RowVersionConverter : OrmLiteConverter
    {
        public override string ColumnDefinition
        {
            get { return "BIGINT"; }
        }

        public virtual ulong FromDbRowVersion(object value)
        {
            return (ulong)this.ConvertNumber(typeof(ulong), value);
        }

        public override object FromDbValue(Type fieldType, object value)
        {
            return value != null
                ? this.ConvertNumber(typeof(ulong), value)
                : null;
        }
    }

    public class ReferenceTypeConverter : StringConverter
    {
        public override string ColumnDefinition
        {
            get { return DialectProvider.GetStringConverter().MaxColumnDefinition; }
        }

        public override string MaxColumnDefinition
        {
            get { return DialectProvider.GetStringConverter().MaxColumnDefinition; }
        }

        public override string GetColumnDefinition(int? stringLength)
        {
            return stringLength != null
                ? base.GetColumnDefinition(stringLength)
                : MaxColumnDefinition;
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            return DialectProvider.GetQuotedValue(DialectProvider.StringSerializer.SerializeToString(value));
        }

        public override object ToDbValue(Type fieldType, object value)
        {
            //Let ADO.NET providers handle byte[]
            if (fieldType == typeof(byte[]))
                return value;

            return DialectProvider.StringSerializer.SerializeToString(value);
        }

        public override object FromDbValue(Type fieldType, object value)
        {
            var convertedValue = DialectProvider.StringSerializer.DeserializeFromString(value.ToString(), fieldType);
            return convertedValue;
        }
    }

    public class ValueTypeConverter : StringConverter
    {
        public override string ColumnDefinition
        {
            get { return DialectProvider.GetStringConverter().MaxColumnDefinition; }
        }

        public override string MaxColumnDefinition
        {
            get { return DialectProvider.GetStringConverter().MaxColumnDefinition; }
        }

        public override string GetColumnDefinition(int? stringLength)
        {
            return stringLength != null
                ? base.GetColumnDefinition(stringLength)
                : MaxColumnDefinition;
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            return DialectProvider.GetQuotedValue(DialectProvider.StringSerializer.SerializeToString(value));
        }

        public override object ToDbValue(Type fieldType, object value)
        {
            return DialectProvider.ToDbValue(value, fieldType);
        }

        public override object FromDbValue(Type fieldType, object value)
        {
            return DialectProvider.FromDbValue(value, fieldType);
        }
    }
}