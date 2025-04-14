using System.Data;
using System.Text;
using CodeGeneratorForm.Entity;
using RazorEngineCore;
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
            string extendName,
            bool v,
            bool ignorePrefix
        )
        {
            try
            {
                string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
                string[] templates = Directory.GetFiles(
                    TemplateDirPath,
                    "*.cshtml",
                    SearchOption.TopDirectoryOnly
                );

                var tables = InitTables(dbTableInfos);
                // 设置模板
                string templatePath = templates
                    .Where(t => Path.GetFileName(t) == templateName)
                    .First();
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(
                    File.ReadAllText(templatePath)
                );
                StringBuilder fileContent = new StringBuilder();
                var saveOnePath = Path.Combine(
                    generatorPath,
                    $"{prefix}{Path.GetFileNameWithoutExtension(templateName) + DateTime.Now.ToString("yyyyMMddHHmmss")}{suffix}{extendName}"
                );
                foreach (var table in tables)
                {
                    if (ignorePrefix)
                    {
                        table.ClassName = table.ClassName.Substring(table.TableName.IndexOf("_"));
                    }

                    var savePath = Path.Combine(
                        generatorPath,
                        $"{prefix}{table.ClassName}{suffix}{extendName}"
                    );
                    var saveDirectoryPath = Path.GetDirectoryName(savePath);
                    if (
                        !string.IsNullOrEmpty(saveDirectoryPath)
                        && !Directory.Exists(saveDirectoryPath)
                    )
                    {
                        Directory.CreateDirectory(saveDirectoryPath);
                    }

                    fileContent.Append(template.Run(table));
                    if (v)
                    {
                        fileContent.AppendLine(
                            "----------------------------------分割线----------------------------------"
                        );
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

        private static List<Table> InitTables(List<DbTableInfo> dbTableInfos)
        {
            INIFile ini = new(Path.Combine(Environment.CurrentDirectory, "field.ini"));
            var tables = new List<Table>();
            if (dbTableInfos is { Count: > 0 })
            {
                foreach (var dbTableInfo in dbTableInfos)
                {
                    List<DbColumnInfo> columnInfos =
                        DbContext.db!.DbMaintenance.GetColumnInfosByTableName(
                            dbTableInfo.Name,
                            false
                        );
                    var cols = new List<ColumnInfo>();
                    foreach (var columnInfo in columnInfos)
                    {
                        string type = ini.IniReadValue("FileTypeConvort", columnInfo.DataType);
                        cols.Add(
                            new ColumnInfo
                            {
                                TableName = columnInfo.TableName,
                                ColumnName = columnInfo.DbColumnName,
                                ColumnComment = columnInfo.ColumnDescription,
                                ColumnDefault = columnInfo.DefaultValue,
                                DataType = columnInfo.DataType,
                                IsIdentity = columnInfo.IsIdentity,
                                IsNullable = columnInfo.IsNullable,
                                IsPrimaryKey = columnInfo.IsPrimarykey,
                                PropertyName = ToPascla(columnInfo.DbColumnName, true),
                                PropertyName2 = ToPascla(columnInfo.DbColumnName, false),
                                PropertyType = String.IsNullOrEmpty(type)
                                    ? columnInfo.DataType
                                    : type,
                            }
                        );
                    }
                    var referenceList = ini.IniReadValue("FileTypeConvort", "ReferenceList")
                        .Split(";")
                        .Select(t => t + ";")
                        .ToList();
                    tables.Add(
                        new Table
                        {
                            TableName = dbTableInfo.Name,
                            TableComment = dbTableInfo.Description,
                            ClassName = ToPascla(dbTableInfo.Name, true),
                            ClassName2 = ToPascla(dbTableInfo.Name, false),
                            ColumnInfos = cols,
                            ReferenceList = referenceList,
                        }
                    );
                }
            }

            return tables;
        }

        private static string ToPascla(string str, bool isUpper = true)
        {
            str = System.Text.RegularExpressions.Regex.Replace(
                str,
                "[a-z][A-Z]",
                m => $"{m.Value[0]}_{char.ToLower(m.Value[1])}"
            );
            string[] split = str.Split(new char[] { '/', ' ', '_', '.' });
            string newStr = "";
            int count = 0;
            foreach (var item in split)
            {
                char[] chars = item.ToCharArray();
                if (count == 0)
                {
                    if (isUpper)
                    {
                        chars[0] = char.ToUpper(chars[0]);
                    }
                    else
                    {
                        chars[0] = char.ToLower(chars[0]);
                    }
                }
                else
                {
                    chars[0] = char.ToUpper(chars[0]);
                }

                for (int i = 1; i < chars.Length; i++)
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                newStr += new string(chars);
                count++;
            }
            return newStr;
        }
    }

    public class Table
    {
        public string TableName { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;
        public string ClassName2 { get; set; } = string.Empty;

        public string TableComment { get; set; } = string.Empty;

        public List<string> ReferenceList { get; set; } = new List<string>();

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

        public string PropertyName2 { get; set; } = string.Empty;

        public string PropertyType { get; set; } = string.Empty;

        public bool IsPrimaryKey { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }
    }
}
