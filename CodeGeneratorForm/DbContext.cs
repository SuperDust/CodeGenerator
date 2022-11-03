using SqlSugar;
using System.Reflection;

namespace CodeGeneratorForm;

/// <summary>
///     数据库上下文对象
/// </summary>
public static class DbContext
{
    /// <summary>
    ///     SqlSugar 数据库实例
    /// </summary>
    public static readonly SqlSugarScope Instance = new(new ConnectionConfig
    {
        ConnectionString = "Data Source=./CodeGenerator.db",
        DbType = SqlSugar.DbType.Sqlite
    }, db =>
    {
        db.Aop.OnError = exp => //SQL报错
        {
            //调试使用
            // UtilMethods.GetSqlString(DbType.MySql, exp.Sql, (SugarParameter[])exp.Parametres);
        };
        db.Aop.OnLogExecuting = (sql, pars) =>
        {

        };
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {

        };
        db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices
        {
            //注意:  这儿AOP设置不能少
            EntityService = (c, p) =>
            {
                if (c.PropertyType.Name == "String" && !p.IsPrimarykey) p.IsNullable = true;
                // int?  decimal?这种 isnullable=true
                if (new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                    p.IsNullable = true;
            }
        };
    });

    public static SqlSugarScope Connection(string connectionString, string dbtype)
    {
        return new SqlSugarScope(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = (SqlSugar.DbType)Enum.Parse(typeof(SqlSugar.DbType), dbtype)
        });

    }

    public static SqlSugarScope? db = null;

    public static SqlSugar.DbType? DbType = null;
}