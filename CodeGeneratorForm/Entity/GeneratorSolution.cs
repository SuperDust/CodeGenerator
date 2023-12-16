using SqlSugar;

namespace CodeGeneratorForm.Entity
{
    [SugarTable("generator_solution")]
    public class GeneratorSolution
    {

        /// <summary>
        ///     方案名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     模板
        /// </summary>
        [SugarColumn(ColumnName = "template")]
        public string Template { get; set; }

        /// <summary>
        ///     命名空间
        /// </summary>
        [SugarColumn(ColumnName = "namespace_name")]
        public string NamespaceName { get; set; }

        /// <summary>
        ///     文件前缀
        /// </summary>
        [SugarColumn(ColumnName = "file_first")]
        public string FileFirst { get; set; }

        /// <summary>
        ///     文件后缀
        /// </summary>
        [SugarColumn(ColumnName = "file_last")]
        public string Filelast { get; set; }

        /// <summary>
        ///     生成路径
        /// </summary>
        [SugarColumn(ColumnName = "dir_path")]
        public string DirPath { get; internal set; }
    }
}
