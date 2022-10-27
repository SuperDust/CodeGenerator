using System.Diagnostics;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SqlSugar;
using System.Text.RegularExpressions;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;

namespace CodeGenerator;
internal class Program
{
    private static readonly string connectionString = Appsettings.ConfigString("ConnectionString");
    private static readonly DbType dbType = (DbType)Enum.Parse(typeof(DbType), Appsettings.ConfigString("DbType"));


    public static void Main(string[] args)
    {
        try
        {
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            string[] templates = Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly);
            int i = 0;
            foreach (string template in templates)
            {
                Console.WriteLine($"{i}:{Path.GetFileNameWithoutExtension(template)}");
                i++;
            }
            Console.WriteLine("请选择模板输入编号:");
            var inputTemplateIndex = Console.ReadLine();
            if (!(int.TryParse(inputTemplateIndex, out int templateIndex) && templateIndex < templates.Length))
            {
                Console.WriteLine("未选择中模板...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("请输入生成文件前缀:");
            var firstStr = Console.ReadLine() ?? "";
            Console.WriteLine("请输入生成文件后缀:");
            var lastStr = Console.ReadLine() ?? "";
            Console.WriteLine("输入表名称(多表;分割)Y(全部生成):");
            var inputStr = Console.ReadLine() ?? "Y";
            List<Table> tables;
            if (string.IsNullOrEmpty(inputStr) || inputStr.ToUpper() == "Y")
            {
                tables = InitTables();
            }
            else
            {
                tables = InitTables(inputStr.Split(";"));
            }
            var config = new TemplateServiceConfiguration();
            config.Debug = true;
            config.EncodedStringFactory = new RawStringFactory();
            var service = RazorEngineService.Create(config);
            // 设置模板
            string templatePath = templates[templateIndex];
            //调试
            //var currentPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
            //发布
            var currentPath = Path.Combine(Appsettings.ConfigString("GeneratorPath"),"Generator");
            if (File.Exists(templatePath))
            {
                foreach (var table in tables)
                {
                    var result = service.RunCompile(
                           File.ReadAllText(templatePath),
                           Path.GetFileNameWithoutExtension(templatePath),
                           null,
                           table);
                    var savePath = Path.Combine(currentPath, $"{firstStr}{table.ClassName}{lastStr}.cs");
                    var saveDirectoryPath = Path.GetDirectoryName(savePath);
                    if (!Directory.Exists(saveDirectoryPath))
                    {
                        Directory.CreateDirectory(saveDirectoryPath ?? "");
                    }
                    File.WriteAllText(savePath, result);
                }
            }
            Process.Start("explorer.exe", currentPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private static List<Table> InitTables(params string[] tableNames)
    {
        var tables = new List<Table>();
        var db = GetInstance();
        if (tableNames.Length > 0)
        {
            foreach (var tableName in tableNames)
            {
                List<DbColumnInfo> columnInfos = db.DbMaintenance.GetColumnInfosByTableName(tableName, false);
                if (columnInfos == null || columnInfos.Count == 0)
                {
                    break;
                }
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
                    TableName = tableName,
                    TableComment = tableName,
                    ClassName = ToUpperCamel(CleanUp(tableName)),
                    ColumnInfos = cols
                });
            }
        }
        else
        {
            foreach (var tableInfo in db.DbMaintenance.GetTableInfoList())
            {
                List<DbColumnInfo> columnInfos = db.DbMaintenance.GetColumnInfosByTableName(tableInfo.Name, false);
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
                    TableName = tableInfo.Name,
                    TableComment = tableInfo.Description,
                    ClassName = ToUpperCamel(CleanUp(tableInfo.Name)),
                    ColumnInfos = cols
                });
            }
        }


        return tables;
    }

    private static SqlSugarClient GetInstance()
    {
        return new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = connectionString,
            DbType = dbType,
            IsAutoCloseConnection = true,
            AopEvents = new AopEvents()
            {
                OnLogExecuting = (sql, pars) =>
                {
                    //Console.WriteLine(sql);
                }
            }
        });
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
        switch (dbType)
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