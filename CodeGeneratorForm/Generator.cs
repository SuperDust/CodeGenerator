using System.Text.RegularExpressions;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SqlSugar;

namespace CodeGeneratorForm
{
    public class Generator
    {

        public static void Init(
            List<DbTableInfo> dbTableInfos,
            string templateName,
            string prefix,
            string suffix,
            string generatorPath,
            string namespaceText)
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
                    File.WriteAllText(savePath, result.Replace("命名空间", namespaceText));
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
                            PropertyType = DataTypeConvort(columnInfo.DataType)
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

        private static string DataTypeConvort(string sqlType)
        {
            INIFile ini = new INIFile(Path.Combine(Environment.CurrentDirectory, "config.ini"));
            string type = ini.IniReadValue("DataTypeConvort", sqlType);
            if (string.IsNullOrWhiteSpace(type))
            {
                return sqlType;
            }
            return type;
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
