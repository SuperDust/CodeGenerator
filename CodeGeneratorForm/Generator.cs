using System.Text.RegularExpressions;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using SqlSugar;
using SqlSugar.Extensions;

namespace CodeGeneratorForm
{
    public class Generator
    {
        private static string namespaceName = String.Empty;

        public static void Init(
            List<DbTableInfo> dbTableInfos,
            string templateName,
            string prefix,
            string suffix,
            string generatorPath,
            string namespaceText,
            string extendName,
            bool v)
        {
            try
            {
                namespaceName = namespaceText;
                string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
                string[] templates = Directory.GetFiles(TemplateDirPath, "*.tcode", SearchOption.TopDirectoryOnly);

                var tables = InitTables(dbTableInfos);
                // 设置模板
                string templatePath = templates.Where(t => Path.GetFileName(t) == templateName).First();
                List<string> fileContent = new List<string>();
                var saveOnePath = Path.Combine(generatorPath, $"{prefix}{Path.GetFileNameWithoutExtension(templateName) + DateTime.Now.ToString("yyyyMMddHHmmss")}{suffix}{extendName}");
                foreach (var table in tables)
                {
                    var savePath = Path.Combine(generatorPath, $"{prefix}{table.ClassName}{suffix}{extendName}");
                    var saveDirectoryPath = Path.GetDirectoryName(savePath);
                    if (!string.IsNullOrEmpty(saveDirectoryPath) && !Directory.Exists(saveDirectoryPath))
                    {
                        Directory.CreateDirectory(saveDirectoryPath);
                    }

                    List<string> templateList = new List<string>();
                    List<string> contents = File.ReadAllLines(templatePath).ToList();
                    int skipIndex = contents.FindIndex(x => x.Contains("$(BEGIN)"));
                    int takeIndex = 0;
                    if (skipIndex != -1)
                    {
                        takeIndex = contents.FindLastIndex(x => x.Contains("$(END)"));
                        takeIndex = takeIndex - skipIndex + 1;
                        templateList.AddRange(contents.Skip(skipIndex).Take(takeIndex));
                    }
                    Regex r = new Regex(@"\$\([.\s\S]*?\)");
                    Regex rCalculate = new Regex(@"\$\[[.\s\S]*?\]");
                    for (int j = 0; j < contents.Count; j++)
                    {
                        if (j == skipIndex)
                        {
                            for (int l = 0; l < table.ColumnInfos.Count; l++)
                            {
                                for (int k = 0; k < templateList.Count; k++)
                                {
                                    var templateContent = templateList[k];
                                    var templateMatches = r.Matches(templateContent);
                                    if (templateMatches.Count > 0)
                                    {

                                        for (int i = 0; i < templateMatches.Count; i++)
                                        {
                                            string value = templateMatches[i].Groups[0].Value;
                                            // 支持三目运算$($[C_IsNullable] ? 'is null' : 'not null')
                                            if (value.Contains("$["))
                                            {
                                                IJsEngine engine = new JintJsEngine(
                                                    new JintSettings
                                                    {
                                                        StrictMode = true
                                                    }
                                                );
                                                var calculateMatches = rCalculate.Matches(value);
                                                if (calculateMatches.Count > 0)
                                                {
                                                    for (int calculateIndex = 0; calculateIndex < calculateMatches.Count; calculateIndex++)
                                                    {
                                                        string calculateValue = calculateMatches[calculateIndex].Groups[0].Value;
                                                        calculateValue = calculateValue.Replace("$[", "$(").Replace("]", ")");
                                                        engine.SetVariableValue("x" + calculateIndex, getValue(calculateValue, table, l));
                                                        value = rCalculate.Replace(value, "x" + calculateIndex);
                                                    }
                                                }
                                                templateContent = r.Replace(templateContent, engine.Evaluate(value.Replace("$", "")).ObjToString());
                                            }
                                            else
                                            {
                                                templateContent = r.Replace(templateContent, getValue(value, table, l).ObjToString());
                                            }
                                        }
                                    }
                                    fileContent.Add(templateContent);
                                }
                            }
                        }
                        if (j >= skipIndex && j <= takeIndex)
                        {
                            continue;
                        }

                        var content = contents[j];
                        var matches = r.Matches(content);
                        if (matches.Count > 0)
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                string value = matches[i].Groups[0].Value;
                                if (value.Contains("$["))
                                {
                                    IJsEngine engine = new JintJsEngine(
                                        new JintSettings
                                        {
                                            StrictMode = true
                                        }
                                    );
                                    var calculateMatches = rCalculate.Matches(value);
                                    if (calculateMatches.Count > 0)
                                    {
                                        for (int calculateIndex = 0; calculateIndex < calculateMatches.Count; calculateIndex++)
                                        {
                                            string calculateValue = calculateMatches[calculateIndex].Groups[0].Value;
                                            calculateValue = calculateValue.Replace("$[", "$(").Replace("]", ")");
                                            engine.SetVariableValue("x" + calculateIndex, getValue(calculateValue, table));
                                            value = rCalculate.Replace(value, "x" + calculateIndex);
                                        }
                                    }
                                    content = r.Replace(content, engine.Evaluate(value.Replace("$", "")).ObjToString());
                                }
                                else
                                {
                                    content = r.Replace(content, getValue(value, table).ObjToString());
                                }
                            }
                        }
                        fileContent.Add(content);
                    }
                    if (v)
                    {
                        fileContent.Add("----------------------------------分割线----------------------------------");
                    }
                    else
                    {
                        fileContent = new List<string>();
                        File.WriteAllLines(savePath, fileContent);
                    }
                }
                File.WriteAllLines(saveOnePath, fileContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static object getValue(string value, Table table, int index = 0)
        {
            try
            {
                switch (value)
                {
                    case "$(Namespace)":
                        return namespaceName;
                    case "$(T_TableName)":
                        return table.TableName;
                    case "$(T_ClassName)":
                        return table.ClassName;
                    case "$(T_TableComment)":
                        return table.TableComment;
                    case "$(C_ColumnName)":
                        return table.ColumnInfos[index].ColumnName;
                    case " $(C_ColumnComment)":
                        return table.ColumnInfos[index].ColumnComment;
                    case "$(C_ColumnDefault)":
                        return table.ColumnInfos[index].ColumnDefault;
                    case "$(C_DataType)":
                        return table.ColumnInfos[index].DataType;
                    case "$(C_IsIdentity)":
                        return table.ColumnInfos[index].IsIdentity;
                    case "$(C_IsPrimaryKey)":
                        return table.ColumnInfos[index].IsPrimaryKey;
                    case "$(C_PropertyName)":
                        return table.ColumnInfos[index].PropertyName;
                    case "$(C_PropertyType)":
                        return table.ColumnInfos[index].PropertyType;
                }
            }
            catch (Exception)
            {
                //  MessageBox.Show(ex.Message);
            }
            return "";
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
