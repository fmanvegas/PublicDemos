using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CRUD_App.Converters
{
    public class StrengthToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = 0;

            if (value is string sVal && !string.IsNullOrEmpty(sVal))
                _ = int.TryParse(sVal, out result);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IntelligenceToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = 0;

            if (value is string sVal && !string.IsNullOrEmpty(sVal))
            {

                Dictionary<string, int> mapping = new() {
                    { "low", 20 },
                    { "moderate", 50 },
                    { "average", 70 },
                    { "good", 80 },
                    { "high", 95 },
                };            
                
                result = mapping.TryGetValue(sVal, out result) ? result : 0;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
