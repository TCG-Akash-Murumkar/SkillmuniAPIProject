using m2ostnextservice;
using System;
public static class ErrorLogger
{
    public static void LogError(Exception ex)
    {
        using (var db = new db_m2ostEntities()) // Your EF database context
        {
            var errorEntry = new error_log
            {
                Error_Message = ex.Message,
                Error_Inner = ex.InnerException?.ToString(),  // Get inner exception if exists
                STATUS = "New",  // Default status
                UPDATEDDATETIME = DateTime.Now
            };

            db.error_log.Add(errorEntry);
            db.SaveChanges(); // Save error to database
        }
    }


}
