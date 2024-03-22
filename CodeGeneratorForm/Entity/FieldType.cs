using SqlSugar;

namespace CodeGeneratorForm.Entity
{
    [SugarTable("field_type")]
    public class FieldType
    {

        /// <summary>
        ///    类型名称
        /// </summary>
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        ///    类型名称
        /// </summary>
        [SugarColumn(ColumnName = "ColumnType")]
        public string ColumnType { get; set; }

        /// <summary>
        ///     属性名称
        /// </summary>
        [SugarColumn(ColumnName = "AttrType")]
        public string AttrType { get; set; }

        /// <summary>
        ///     命名空间
        /// </summary>
        [SugarColumn(ColumnName = "PackageName")]
        public string PackageName { get; set; }

        
    }
}
