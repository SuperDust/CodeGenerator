using SqlSugar;

namespace CodeGeneratorForm.Entity
{
    [SugarTable("db_config")]
    public class DbConfig
    {

        /// <summary>
        ///     连接字符串
        /// </summary>
        [SugarColumn(ColumnName = "connection_config")]
        public string ConnectionConfig { get; set; }

        /// <summary>
        ///     连接类型
        /// </summary>
        [SugarColumn(ColumnName = "connection_type")]
        public string ConnectionType { get; set; }
    }
}
