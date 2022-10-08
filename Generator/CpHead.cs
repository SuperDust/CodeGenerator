namespace QmsPlus.Model;

[SugarTable("CP_HEAD")]
public class CpHead
{
    [SugarColumn(ColumnName = "CP_ID" ,IsPrimaryKey = true ,IsIdentity = true)]
    public decimal  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDMANDNR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "MODULE"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CODE"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "NAME"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "DESCRIPTION"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "CP_VERSION"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CP_STATUS"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CP_TYPE"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "CP_IS_COMMON"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDARTIKELNR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDAFONR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDLINIENR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "FRQ"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "FRQ_UNIT"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "NLFDMASCHNR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDMASCHTYPNR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "NLFDPAARTNR"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "SAMPLING_ID"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "SAMPLE_TYPE_ID"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "EXE_STANDBY"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "EFFECTIVE_DATE"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "EXPIRY_DATE"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "TRIGGER_FREQUENCY_DATE"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "SUBMIT_ID"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "SUBMIT_NAME"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "SUBMIT_DATE"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "APPROVAL_ID"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "APPROVAL_NAME"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "APPROVAL_DATE"  )]
    public DateTime?  { get; set; }
    
    [SugarColumn(ColumnName = "APPROVAL_STATUS"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "APPROVAL_COMMENT"  )]
    public string  { get; set; } = string.Empty;
    
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
    
    [SugarColumn(ColumnName = "CP_DEFAULT"  )]
    public decimal?  { get; set; }
    
    [SugarColumn(ColumnName = "SINFO"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "SINFO2"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "SINFO3"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "SINFO4"  )]
    public string  { get; set; } = string.Empty;
    
    [SugarColumn(ColumnName = "SINFO5"  )]
    public string  { get; set; } = string.Empty;
    
}

 