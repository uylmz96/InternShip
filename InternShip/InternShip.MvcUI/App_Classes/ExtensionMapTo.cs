using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternShip.MvcUI
{
    public static class ExtensionMapTo
    {
        public static void MapTo<T>(this object source,T target)
        {
            /* [15.02.2019] Umut Yılmaz.
             * Bu fonksiyon bir entity deki değerleri aynı tipteki entitye taşımak için yazıldı.
             * Fonksiyon object sınıfının fonksiyonları arasına  Runtime da eklenecektir.
             * Null olan değerler atlanacaktır.
             * Kullanımı -> source.MapTo<Entity>(target);
             */
            PropertyInfo[] targetProps = target.GetType().GetProperties();
            PropertyInfo[] sourceProps = source.GetType().GetProperties();

            foreach (PropertyInfo item in sourceProps)
            {
                object val = item.GetValue(source);

                PropertyInfo targetProperty = targetProps.FirstOrDefault(x => x.Name == item.Name);
                if (targetProperty != null && val != null &item.Name!="CrtDate")
                    targetProperty.SetValue(target, val);
            }
        }
    }
}