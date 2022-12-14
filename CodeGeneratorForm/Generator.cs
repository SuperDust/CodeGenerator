using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SqlSugar;
using System.Text.RegularExpressions;

namespace CodeGeneratorForm
{
    public class Generator
    {

        public static void Init(
            List<DbTableInfo> dbTableInfos,
            string templateName,
            string prefix,
            string suffix,
            string generatorPath)
        {
            try
            {
                string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
                string[] templates = Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly);

                var tables = InitTables(dbTableInfos);
                var config = new TemplateServiceConfiguration();
                config.Debug = true;
                config.EncodedStringFactory = new RawStringFactory();
                var service = RazorEngineService.Create(config);
                // 设置模板
                string templatePath = templates.Where(t => Path.GetFileName(t) == templateName).First();

                foreach (var table in tables)
                {
                    var result = service.RunCompile(
                           File.ReadAllText(templatePath),
                           Path.GetFileNameWithoutExtension(templatePath),
                           null,
                           table);
                    var savePath = Path.Combine(generatorPath, $"{prefix}{table.ClassName}{suffix}.cs");
                    var saveDirectoryPath = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(saveDirectoryPath))
                    {
                        Directory.CreateDirectory(saveDirectoryPath ?? "");
                    }
                    File.WriteAllText(savePath, result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private static List<Table> InitTables(List<DbTableInfo> dbTableInfos)
        {
            var tables = new List<Table>();
            if (dbTableInfos is { Count: > 0 })
            {
                foreach (var dbTableInfo in dbTableInfos)
                {
                    List<DbColumnInfo> columnInfos =
                        DbContext.db!.DbMaintenance.GetColumnInfosByTableName(dbTableInfo.Name, false);

                    var cols = new List<ColumnInfo>();
                    foreach (var columnInfo in columnInfos)
                    {
                        cols.Add(new ColumnInfo
                        {
                            TableName = columnInfo.TableName,
                            ColumnName = columnInfo.DbColumnName,
                            ColumnComment = columnInfo.ColumnDescription,
                            ColumnDefault = columnInfo.DefaultValue,
                            DataType = columnInfo.DataType,
                            IsIdentity = columnInfo.IsIdentity,
                            IsNullable = columnInfo.IsNullable,
                            IsPrimaryKey = columnInfo.IsPrimarykey,
                            PropertyName = ToUpperCamel(CleanUp(columnInfo.DbColumnName)),
                            PropertyType = DbColTypeConvort(columnInfo.DataType)
                        });
                    }
                    tables.Add(new Table
                    {
                        TableName = dbTableInfo.Name,
                        TableComment = dbTableInfo.Description,
                        ClassName = ToUpperCamel(CleanUp(dbTableInfo.Name)),
                        ColumnInfos = cols
                    });
                }
            }

            return tables;
        }

        private static readonly Regex rxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);
        private static readonly string[] cs_keywords = { "abstract", "event", "new", "struct", "as", "explicit", "null",
         "switch", "base", "extern", "object", "this", "bool", "false", "operator", "throw",
         "break", "finally", "out", "true", "byte", "fixed", "override", "try", "case", "float",
         "params", "typeof", "catch", "for", "private", "uint", "char", "foreach", "protected",
         "ulong", "checked", "goto", "public", "unchecked", "class", "if", "readonly", "unsafe",
         "const", "implicit", "ref", "ushort", "continue", "in", "return", "using", "decimal",
         "int", "sbyte", "virtual", "default", "interface", "sealed", "volatile", "delegate",
         "internal", "short", "void", "do", "is", "sizeof", "while", "double", "lock",
         "stackalloc", "else", "long", "static", "enum", "namespace", "string" };
        private static readonly Func<string, string> CleanUp = (str) =>
        {
            str = rxCleanUp.Replace(str, "");

            if (char.IsDigit(str[0]) || cs_keywords.Contains(str))
                str = "@" + str;

            return str;
        };
        private static readonly Func<string, string> ToUpperCamel = (str) =>
        {
            var temp = str.Split("_");

            for (var i = 0; i < temp.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(temp[i]))
                {
                    temp[i] = temp[i].Substring(0, 1).ToUpper() + temp[i].Substring(1).ToLower();
                }
            }
            return string.Join("", temp);
        };

        private static string DbColTypeConvort(string sqlType)
        {
            switch (DbContext.DbType)
            {
                case DbType.SqlServer: return GetSqlServerPropertyType(sqlType);
                case DbType.MySql: return GetMySqlPropertyType(sqlType);
                case DbType.PostgreSQL: return GetPGPropertyType(sqlType);
                case DbType.Oracle: return GetOraclePropertyType(sqlType);
            }
            return "未映射";
        }

        private static string GetSqlServerPropertyType(string sqlType)
        {
            string sysType = "string";
            switch (sqlType)
            {
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                case "time":
                    sysType = "DateTime";
                    break;
                case "datetimeoffset":
                    sysType = "DateTimeOffset";
                    break;
                case "float":
                    sysType = "double";
                    break;
                case "real":
                    sysType = "float";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = "byte[]";
                    break;
                case "geography":
                    sysType = "Microsoft.SqlServer.Types.SqlGeography";
                    break;
                case "geometry":
                    sysType = "Microsoft.SqlServer.Types.SqlGeometry";
                    break;
            }
            return sysType;
        }

        private static string GetPGPropertyType(string sqlType)
        {
            switch (sqlType)
            {
                case "int8":
                case "serial8":
                    return "long";

                case "bool":
                    return "bool";

                case "bytea	":
                    return "byte[]";

                case "float8":
                    return "double";

                case "int4":
                case "serial4":
                    return "int";

                case "money	":
                    return "decimal";

                case "numeric":
                    return "decimal";

                case "float4":
                    return "float";

                case "int2":
                    return "short";

                case "time":
                case "timetz":
                case "timestamp":
                case "timestamptz":
                case "date":
                    return "DateTime";

                case "uuid":
                    return "Guid";

                default:
                    return "string";
            }
        }

        private static string GetMySqlPropertyType(string sqlType)
        {
            string propType = "string";
            switch (sqlType)
            {
                case "bigint":
                    propType = "long";
                    break;
                case "int":
                    propType = "int";
                    break;
                case "smallint":
                    propType = "short";
                    break;
                case "guid":
                    propType = "Guid";
                    break;
                case "smalldatetime":
                case "date":
                case "datetime":
                case "timestamp":
                    propType = "DateTime";
                    break;
                case "float":
                    propType = "float";
                    break;
                case "double":
                    propType = "double";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    propType = "decimal";
                    break;
                case "bit":
                case "bool":
                case "boolean":
                    propType = "bool";
                    break;
                case "tinyint":

                    propType = "sbyte";
                    break;
                case "image":
                case "binary":
                case "blob":
                case "mediumblob":
                case "longblob":
                case "varbinary":
                    propType = "byte[]";
                    break;
            }
            return propType;
        }

        private static string GetOraclePropertyType(string sqlType)
        {
            string sysType = "string";
            sqlType = sqlType.ToLower();
            switch (sqlType)
            {
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "date":
                    sysType = "DateTime";
                    break;
                case "float":
                    sysType = "double";
                    break;
                case "real":
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                case "number":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = "byte[]";
                    break;
            }
            return sysType;
        }
    }
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
