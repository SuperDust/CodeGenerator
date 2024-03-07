using System;
using System.Text;
using System.Text.RegularExpressions;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using Jint;
using MySqlX.XDevAPI.Relational;
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
                StringBuilder fileContent = new StringBuilder();
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
                    int skipIndex = contents.FindIndex(x => x.Contains("${BEGIN}"));
                    int takeIndex = 0;
                    if (skipIndex != -1)
                    {
                        takeIndex = contents.FindLastIndex(x => x.Contains("${END}"));
                        takeIndex = takeIndex - skipIndex + 1;
                        templateList.AddRange(contents.Skip(skipIndex).Take(takeIndex));
                    }
                    Regex r = new Regex(@"\$\{[.\s\S]*?\}");
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
                                           templateContent = r.Replace(templateContent, getValue(value, table, l).ObjToString());                                            
                                        }
                                    }
                                    fileContent.AppendLine(templateContent);
                                }
                            }
                        }
                        if (skipIndex!=-1 && j >= skipIndex && j <= takeIndex)
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
                                content = r.Replace(content, getValue(value, table).ObjToString());
                            }
                        }
                        fileContent.AppendLine(content);
                    }


                    Regex funcCalculate = new Regex(@"\$FUNC\{[.\s\S]*?\}\$");
                    if (fileContent.ToString().Contains("$FUNC{"))
                    {
                        
                        var calculateMatches = funcCalculate.Matches(fileContent.ToString());
                        if (calculateMatches.Count > 0)
                        {
                            for (int calculateIndex = 0; calculateIndex < calculateMatches.Count; calculateIndex++)
                            {
                                string calculateValue = calculateMatches[calculateIndex].Groups[0].Value;
                                var engine = new Engine().Execute(calculateValue.Replace("$FUNC{", "function getResult(){").Replace("}$", "}"));
                                var result= engine.Invoke("getResult").ObjToString();
                                fileContent.Replace(calculateValue, result);
                            }
                        }
                    }
                    if (v)
                    {
                        fileContent.AppendLine("----------------------------------分割线----------------------------------");
                    }
                    else
                    {
                        File.WriteAllText(savePath, fileContent.ToString());
                        fileContent = new StringBuilder();
                    }
                }
                if (v)
                {
                    File.WriteAllText(saveOnePath, fileContent.ToString());
                }
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
                    case "${namespace}":
                        return namespaceName;
                    case "${tableName}":
                        return table.TableName;
                    case "${tableComment}":
                        return table.TableComment;
                    case "${ClassName}":
                        return table.ClassName;
                    case "${className}":
                        return ToPascla(table.TableName, false);
                    case "${columnName}":
                        return table.ColumnInfos[index].ColumnName;
                    case "${columnComment}":
                        return table.ColumnInfos[index].ColumnComment;
                    case "${columnDefault}":
                        return table.ColumnInfos[index].ColumnDefault;
                    case "${dataType}":
                        return table.ColumnInfos[index].DataType;
                    case "${isIdentity}":
                        return table.ColumnInfos[index].IsIdentity;
                    case "${isPrimaryKey}":
                        return table.ColumnInfos[index].IsPrimaryKey;
                    case "${PropertyName}":
                        return table.ColumnInfos[index].PropertyName;
                    case "${propertyName}":
                        return ToPascla(table.ColumnInfos[index].PropertyName, false);
                    case "${propertyType}":
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
                            PropertyName = ToPascla(columnInfo.DbColumnName,true),
                            PropertyType = DataTypeConvort(columnInfo.DataType)
                        });
                    }
                    tables.Add(new Table
                    {
                        TableName = dbTableInfo.Name,
                        TableComment = dbTableInfo.Description,
                        ClassName = ToPascla(dbTableInfo.Name,true),
                        ColumnInfos = cols
                    });
                }
            }

            return tables;
        }

        private static string ToPascla(string str,bool isUpper=true)
        {
            if (isUpper)
            {
                str = str.ToUpper();
            }
            else 
            {
                str = str.ToLower();
            }
            string[] split = str.Split(new char[] { '/', ' ', '_', '.' });
            string newStr = "";
            foreach (var item in split)
            {
                char[] chars = item.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                for (int i = 1; i < chars.Length; i++)
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                newStr += new string(chars);
            }
            return newStr;
        }
                        

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
