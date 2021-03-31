namespace Mapper.Attributes
{
    /// <summary>
    /// Defines source object property name. 
    /// Enables source and destination property to have a different name.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ColumnNameAttribute : System.Attribute
    {
        public string FieldName { get; }
        public bool FieldIgnore { get;}
        public bool IsDate { get; }
        public bool IsDateTime { get; }

        public ColumnNameAttribute(string fieldName, bool fieldIgnore = false, bool isDate = false, bool isDateTime = false)
        {
            FieldName = fieldName;
            FieldIgnore = fieldIgnore;
            IsDate = isDate;
            IsDateTime = isDateTime;
        }
    }
}
