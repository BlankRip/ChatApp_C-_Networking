using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App
{
    public class Util
    {
        public static int GetColorChoice() {
            string[] colors = {
            "Black", "Blue",
            "Cyan", "DarkBlue",
            "DarkCyan", "DarkGray",
            "DarkGreen", "DarkMagenta",
            "DarkRed", "DarkYellow",
            "Gray", "Green",
            "Magenta", "Red",
            "White", "Yellow"
            };

            int recieved = 0;
            bool casted;

            Console.WriteLine("Please Pick One of the Available Colors by choosing it's curresponding number:");
            while(true) {
                for (int i = 0; i < colors.Length; i++) {
                    Console.ForegroundColor = GetColor(i + 1);
                    Console.WriteLine($"{i + 1}: {colors[i]}");
                }

                casted = int.TryParse(Console.ReadLine(), out recieved);
                if(casted) {
                    if (recieved <= colors.Length)
                        return recieved;
                    else
                        Console.WriteLine("The enterted vaule is not a number available in the provided list");
                } else
                    Console.WriteLine("The enterted vaule is not a number. Please enter again.");
            }
        }
        public static ConsoleColor GetColor(int choice) {
            switch (choice) {
                case 1:
                    return ConsoleColor.Black;
                    break;
                case 2:
                    return ConsoleColor.Blue;
                    break;
                case 3:
                    return ConsoleColor.Cyan;
                    break;
                case 4:
                    return ConsoleColor.DarkBlue;
                    break;
                case 5:
                    return ConsoleColor.DarkCyan;
                    break;
                case 6:
                    return ConsoleColor.DarkGray;
                    break;
                case 7:
                    return ConsoleColor.DarkGreen;
                    break;
                case 8:
                    return ConsoleColor.DarkMagenta;
                    break;
                case 9:
                    return ConsoleColor.DarkRed;
                    break;
                case 10:
                    return ConsoleColor.DarkYellow;
                    break;
                case 11:
                    return ConsoleColor.Gray;
                    break;
                case 12:
                    return ConsoleColor.Green;
                    break;
                case 13:
                    return ConsoleColor.Magenta;
                    break;
                case 14:
                    return ConsoleColor.Red;
                    break;
                case 15:
                    return ConsoleColor.White;
                    break;
                case 16:
                    return ConsoleColor.Yellow;
                    break;
                default:
                    return ConsoleColor.White;
                    break;
            }
        }

        public static Byte[] ObjectToByteArray(Object obj) {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(Byte[] bytes) {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                Object obj = bf.Deserialize(ms);
                return obj;
            }
        }
    }
}
