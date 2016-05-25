namespace ProcCore.DatabaseCore
{
    /// <summary>
    /// 系統欄位更新所要使用的欄位
    /// </summary>
    public enum UpdateFieldsInfoType
    {
        None,
        Insert,
        Update,
        Both, Lock,
        UnLock,
        Lang,
        TranFalse,
        TranTrue
    }
    public enum SQLUpdateType
    {
        Insert,
        Update,
        Delete
    }
    public enum WhereLogicType
    {
        none,
        and,
        or,
        not
    }
    public enum WhereCompareType
    {
        Like,
        /// <summary>
        /// like '~%'
        /// </summary>
        LikeRight,
        LikeLeft,
        Equel,
        Than,
        Less,
        ThanEquel,
        LessEquel,
        UnEquel,
        Between,
        NotBetween,
        In
    }
    public enum SQLValueType
    {
        Int, String, DateTime, Boolean
    }
    public enum OrderByType
    {
        ASC, DESC
    }
    public enum ConnectionType
    {
        OleDb, SqlClient, MySqlClient,SqlCE
    }
}
