using System;

/// <summary>
/// 自訂系統羅輯錯誤。
/// </summary>

   public class LogicRoll : Exception
    {
        public LogicRoll()
            : base()
        {
        }

        public LogicRoll(String message)
            : base(message)
        {
        }

        public LogicRoll(String message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
   public class LogicError : Exception
   {
       public LogicError()
           : base()
       {
       }

       public LogicError(String message)
           : base(message)
       {
       }

       public LogicError(String message, Exception innerException)
           : base(message, innerException)
       {
           
       }
   }