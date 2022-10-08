namespace QmsPlus.Model;

[SugarTable("BASE_ITEM")]
public class BaseItem
{
    [SugarColumn(ColumnName = "item_id" ,IsPrimaryKey = true ,IsIdentity = true)]
    public decimal  { get; set; }
    
    [SugarColumn(ColumnName = "code"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "name"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "item_type"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "item_version"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "code_version"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "effective_from"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "effective_till"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "description_desc"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "description_cn"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "YY_EL"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "HQHP_FLAGE"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "ITEM_STATUS"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CREATE_BY"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CREATE_NAME"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "CREATE_TIME"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "UPDATE_BY"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "UPDATE_NAME"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "UPDATE_TIME"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "DEL_FLAG"  )]
    public decimal?  { get; set; }
    
}

 