using System;
using System.Data;

public class RunEnd
{
    public RunEnd() {
        Message = "";
    }
    public Boolean Result { get; set; }
    public String Message { get; set; }
    public BusinessErrType ErrType { get; set; }
    public int MessageCode { get; set; }
    public String SQL { get; set; }
}

public class RunInsertEnd : RunEnd {
    /// <summary>
    /// 此為自動新增欄位Id
    /// </summary>
    public int InsertId { get; set; }
    public int Rows { get; set; }
    public object CustomId { get; set; }
}

public class RunUpdateEnd : RunEnd
{
    public int Rows { get; set; }
}

public class RunDeleteEnd : RunEnd
{
}

public class RunQueryEnd : RunEnd
{
    public RunQueryEnd() {
        SearchData = new DataTable(); 
    }
    public DataTable SearchData { get; set; }
    public DataSet SearchDataDataSet{ get; set; }
    public int Count { get { return SearchData.Rows.Count; } }
}

public class RunQueryPackage<m_Module> : RunEnd
{
    public m_Module[] SearchData { get; set; }
    public int Count { get { return SearchData.Length; } }
}

public class RunOneDataEnd<m_Module> : RunEnd
{
    public m_Module SearchData { get; set; }
}

public class LoginSate : RunEnd {
    public String Acccount { get; set; }
    public int Id { get; set; }
    public Boolean IsAdmin { get; set; }
    public int Unit { get; set; }
}

public enum BusinessErrType
{
    Logic,
    System
}