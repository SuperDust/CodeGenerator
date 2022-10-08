namespace CodeGenerator
{
    public class Table
    {
        public string TableName { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;

        public string TableComment { get; set; } = string.Empty;

        public List<ColumnInfo> ColumnInfos { get; set; } = new List<ColumnInfo>();

    }

    public class ColumnInfo
    {
        public string TableName { get; set; } = string.Empty;

        public string ColumnName { get; set; } = string.Empty;

        public string ColumnComment { get; set; } = string.Empty;

        public string ColumnDefault { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        public string PropertyName { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public bool IsPrimaryKey { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }
    }
}
