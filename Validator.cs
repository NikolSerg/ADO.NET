using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShopKindaThing
{
    public static class Validator
    {
        public static bool ValidateNames(string name)
        {
            char[] chars = name.ToLower().ToCharArray();
            char[] charSet = new char[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p',
            'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm',
            'й', 'ц', 'у', 'к', 'к', 'е', 'н', 'г', 'ш', 'щ', 'з', 'х', 'ъ', 'ф', 'ы', 'в',
            'а', 'п', 'р', 'о', 'л', 'д', 'ж', 'э', 'я', 'ч', 'с', 'м', 'и', 'т', 'ь', 'ь', 'б', 'ю', '-' };

            if (chars.Any(p => !charSet.Any(c => c == p)))
            {
                MessageBox.Show("This field cant contain some special signs and spaces",
                    "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (chars.Count() == 0) return false;

            else return true;
        }

        public static bool ValidatePhoneNumber(string number)
        {
            char[] chars = number.ToCharArray();
            char[] charSet = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+' };

            if (chars.Any(p => !charSet.Any(c => c == p)))
            {
                MessageBox.Show("This field can contain only numbers and '+' sign",
                    "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (chars.Count() > 0)
            {
                if (chars.Count() > 15 || chars.Count() < 10)
                {
                    MessageBox.Show("Incorrect phone number", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else return true;
            }
            else return true;
        }

        public static bool ValidateEMail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                MessageBox.Show("E-mail cant end with '.'",
                    "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                MessageBox.Show("Incorrect E-Mail", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
