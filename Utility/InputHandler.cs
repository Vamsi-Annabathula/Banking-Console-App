using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransfer.Utility
{
    public class InputHandler
    {
        public static T GetInput<T>()
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
        public static T GetInput<T>(List<T> validators)
        {
            bool flag = false;
            T value = (T)Convert.ChangeType(1, typeof(T));
            do
            {
                try
                {
                    value = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                    if (string.IsNullOrEmpty(value.ToString()) || !validators.Contains(value))
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
    