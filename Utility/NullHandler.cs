using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Utility
{
    public static class NullHandler
    {
        public static T HandleInput<T>()
        {
            bool flag = false;
            T value = (T)Convert.ChangeType(1, typeof(T));
            do
            {
                try
                {
                    value = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        throw new Exception("Enter valid Input");
                    }
                    flag = true;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("{0}.. Enter input again", e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}.. Enter input again", e.Message);
                }
            } while (!flag);
            return value;
        }
    }
}
    