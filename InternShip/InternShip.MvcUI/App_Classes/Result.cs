using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InternShip.MvcUI.App_Classes
{
    public static class Result
    {
        public static string isAppliedSaveChanges(DbContext context)
        {
            try
            {
                int isApplied=context.SaveChanges();
                if (isApplied >= 0)
                    return "success();";
                else
                    return "error();";
            }
            catch (Exception)
            {
                return "errorMessage('Program Derleme Hatası. Hata:isAppliedSaveChanges');";
            }
        }
        public static bool SaveChanges2(DbContext context)
        {
            try
            {
                int isApplied = context.SaveChanges();
                if (isApplied > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}