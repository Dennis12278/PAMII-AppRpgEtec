using Microsoft.Maui.Graphics.Converters;
using System.Globalization;
using Color = Microsoft.Maui.Graphics.Color;

namespace AppRpgEtec.Converters
{
    public class PontosVidaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ColorTypeConverter converter = new ColorTypeConverter();

            int pontosVida = (int)value;
            if (pontosVida == 100)
                return (Color)converter.ConvertFromInvariantString("SeaGreen");
            else if (pontosVida >= 75)
                return (Color)converter.ConvertFromInvariantString("YellowGreen");
            else if (pontosVida >= 25)
                return (Color)converter.ConvertFromInvariantString("Yellow");
            else if (pontosVida >= 1)
                return (Color)converter.ConvertFromInvariantString("OrangeRed");
            else
                return (Color)converter.ConvertFromInvariantString("Red");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
