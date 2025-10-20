using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MIV
{
    public class MIV
    {
        private static string file;

        public static void printMIVStartScreen()
        {
            Console.Clear();
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~                             MIV^3 - MInimalistic Vi");
            Console.WriteLine("~");
            Console.WriteLine("~                                 version 3.0");
            Console.WriteLine("~");
            Console.WriteLine("~                            OG by Denis Bartashevich");
            Console.WriteLine("~                            Updated by WinBamStudios");
            Console.WriteLine("~                               Modified by ric211");
            Console.WriteLine("~");
            Console.WriteLine("~                         https://github.com/ric211/miv3");
            Console.WriteLine("~");
            Console.WriteLine("~                  MIV is open source and freely distributable");
            Console.WriteLine("~");
            Console.WriteLine("~              press i                    to write (enter edit-mode)");
            Console.WriteLine("~              press <Esc>                to exit edit-mode");
            Console.WriteLine("~              type :help<Enter>          for information");
            Console.WriteLine("~              type :q<Enter>             to quit without saving");
            Console.WriteLine("~              type :x<Enter>             to save to file and exit");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.WriteLine("~");
            Console.Write("~");
        }

        public static String stringCopy(String value)
        {
            // returns whole string without last char (e.g. newline)
            if (string.IsNullOrEmpty(value) || value.Length <= 1) return String.Empty;
            return value.Substring(0, value.Length - 1);
        }

        public static void printMIVScreen(char[] chars, int pos, String infoBar, Boolean editMode)
        {
            int countNewLine = 0;
            int countChars = 0;
            delay(10000000);
            Console.Clear();

            for (int i = 0; i < pos; i++)
            {
                if (chars[i] == '\n')
                {
                    Console.WriteLine("");
                    countNewLine++;
                    countChars = 0;
                }
                else
                {
                    Console.Write(chars[i]);
                    countChars++;
                    if (countChars % 80 == 79)
                    {
                        countNewLine++;
                    }
                }
            }

            Console.Write("/");

            for (int i = 0; i < 23 - countNewLine; i++)
            {
                Console.WriteLine("");
                Console.Write("~");
            }

            //PRINT INSTRUCTION
            Console.WriteLine();
            for (int i = 0; i < 72; i++)
            {
                if (i < infoBar.Length)
                {
                    Console.Write(infoBar[i]);
                }
                else
                {
                    Console.Write(" ");
                }
            }

            if (editMode)
            {
                Console.Write(countNewLine + 1 + "," + countChars);
            }

        }

        public static String miv(String start)
        {
            Boolean editMode = false;
            int pos = 0;
            // min 2000 chars, expandable
            char[] chars = new char[2000];
            String infoBar = String.Empty;

            if (start == null)
            {
                printMIVStartScreen();
            }
            else
            {
                // lengthen buffer when needed
                if (start.Length >= chars.Length)
                {
                    Array.Resize(ref chars, start.Length * 2);
                }

                pos = start.Length;
                for (int i = 0; i < start.Length; i++)
                {
                    chars[i] = start[i];
                }
                printMIVScreen(chars, pos, infoBar, editMode);
            }

            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);

                if (isForbiddenKey(keyInfo.Key)) continue;

                // start command mode
                if (!editMode && keyInfo.KeyChar == ':')
                {
                    infoBar = ":";
                    printMIVScreen(chars, pos, infoBar, editMode);

                    // collect command
                    while (true)
                    {
                        keyInfo = Console.ReadKey(true);

                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            if (infoBar == ":x")
                            {
                                // cancel & save
                                return new string(chars, 0, pos);
                            }
                            else if (infoBar == ":q")
                            {
                                // cancel without save
                                return null;
                            }
                            else if (infoBar == ":help")
                            {
                                printMIVStartScreen();
                                break;
                            }
                            else
                            {
                                infoBar = "ERROR: No such command";
                                printMIVScreen(chars, pos, infoBar, editMode);
                                break;
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            // cancel command mode
                            infoBar = String.Empty;
                            printMIVScreen(chars, pos, infoBar, editMode);
                            break;
                        }
                        else if (keyInfo.Key == ConsoleKey.Backspace)
                        {
                            infoBar = stringCopy(infoBar);
                            printMIVScreen(chars, pos, infoBar, editMode);
                        }
                        else
                        {
                            // append generically printable chars
                            char c = keyInfo.KeyChar;
                            if (!char.IsControl(c))
                            {
                                infoBar += c;
                                printMIVScreen(chars, pos, infoBar, editMode);
                            }
                        }
                    }

                    continue;
                }

                // Esc ends insert mode
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    editMode = false;
                    infoBar = String.Empty;
                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }

                // start insert mode (kes: i)
                if (keyInfo.Key == ConsoleKey.I && !editMode)
                {
                    editMode = true;
                    infoBar = "-- INSERT --";
                    printMIVScreen(chars, pos, infoBar, editMode);
                    continue;
                }

                // behaviour in insert mode
                if (editMode)
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (pos >= chars.Length) Array.Resize(ref chars, chars.Length * 2);
                        chars[pos++] = '\n';
                        printMIVScreen(chars, pos, infoBar, editMode);
                        continue;
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (pos > 0) pos--;
                        if (pos < chars.Length) chars[pos] = '\0';
                        printMIVScreen(chars, pos, infoBar, editMode);
                        continue;
                    }
                    else
                    {
                        char c = keyInfo.KeyChar;
                        if (!char.IsControl(c))
                        {
                            if (pos >= chars.Length) Array.Resize(ref chars, chars.Length * 2);
                            chars[pos++] = c;
                            printMIVScreen(chars, pos, infoBar, editMode);
                        }
                        // else: ignore
                        continue;
                    }
                }

                // outside of insert-mode: ignore
            }
        }

        public static bool isForbiddenKey(ConsoleKey key)
        {
            ConsoleKey[] forbiddenKeys = { ConsoleKey.Print, ConsoleKey.PrintScreen, ConsoleKey.Pause, ConsoleKey.Home, ConsoleKey.PageUp, ConsoleKey.PageDown, ConsoleKey.End, ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3, ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7, ConsoleKey.NumPad8, ConsoleKey.NumPad9, ConsoleKey.Insert, ConsoleKey.F1, ConsoleKey.F2, ConsoleKey.F3, ConsoleKey.F4, ConsoleKey.F5, ConsoleKey.F6, ConsoleKey.F7, ConsoleKey.F8, ConsoleKey.F9, ConsoleKey.F10, ConsoleKey.F11, ConsoleKey.F12, ConsoleKey.Add, ConsoleKey.Divide, ConsoleKey.Multiply, ConsoleKey.Subtract, ConsoleKey.LeftWindows, ConsoleKey.RightWindows };
            for (int i = 0; i < forbiddenKeys.Length; i++)
            {
                if (key == forbiddenKeys[i]) return true;
            }
            return false;
        }

        public static void delay(int time)
        {
            for (int i = 0; i < time; i++) ;
        }

        private static string ReadFileNameOrCancel()
        {
            string input = string.Empty;

            int startLeft = 0, startTop = 0;
            bool cursorPosSupported = true;
            try
            {
                startLeft = Console.CursorLeft;
                startTop = Console.CursorTop;
            }
            catch
            {
                cursorPosSupported = false;
            }

            int prevLength = 0;

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    return null;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);

                        if (cursorPosSupported)
                        {
                            try
                            {
                                Console.SetCursorPosition(startLeft, startTop);
                                Console.Write(input);
                                if (prevLength > input.Length)
                                {
                                    // delete unused chars
                                    Console.Write(new string(' ', prevLength - input.Length));
                                }
                                // set cursor to end of input
                                Console.SetCursorPosition(startLeft + input.Length, startTop);
                            }
                            catch
                            {
                                // fallback, if SetCursorPosition isn't available
                                Console.Write("\r");
                                Console.Write(input);
                                if (prevLength > input.Length)
                                {
                                    Console.Write(new string(' ', prevLength - input.Length));
                                    Console.Write("\r");
                                    Console.Write(input);
                                }
                            }
                        }
                        else
                        {
                            Console.Write("\r");
                            Console.Write(input);
                            if (prevLength > input.Length)
                            {
                                Console.Write(new string(' ', prevLength - input.Length));
                                Console.Write("\r");
                                Console.Write(input);
                            }
                        }

                        prevLength = input.Length;
                    }
                    continue;
                }

                // append printable chars
                char c = key.KeyChar;
                if (!char.IsControl(c))
                {
                    input += c;
                    if (cursorPosSupported)
                    {
                        try
                        {
                            Console.SetCursorPosition(startLeft, startTop);
                            Console.Write(input);
                            Console.SetCursorPosition(startLeft + input.Length, startTop);
                        }
                        catch
                        {
                            Console.Write(c);
                        }
                    }
                    else
                    {
                        Console.Write(c);
                    }
                    prevLength = input.Length;
                }
            }
        }

        public static void StartMIV()
        {
            Console.WriteLine("\nEnter filename to open:\n" +
                "If the specified file doesn't exist, it will be created.\n" +
                "Press <Esc> to exit.");

            Kernel.file = ReadFileNameOrCancel();
            if (Kernel.file == null)
            {
                Console.WriteLine("\n\nExited.\n");
                return;
            }

            var path = @"0:\" + Kernel.file;
            try
            {
                if (File.Exists(path))
                {
                    Console.WriteLine("Found file!");
                }
                else
                {
                    Console.WriteLine("Creating file!");
                    using (File.Create(path)) { } // close stream immediately
                }
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            String text = String.Empty;
            Console.WriteLine("Do you want to open " + Kernel.file + "? (y/n)");
            var answer = Console.ReadLine()?.Trim().ToLowerInvariant();

            // accept any string starting with y/n (easter egg)
            if (!string.IsNullOrEmpty(answer) && answer.StartsWith("y"))
            {
                text = miv(File.ReadAllText(path));
            }
            else if (!string.IsNullOrEmpty(answer) && answer.StartsWith("n"))
            {
                ExitMiv();
                return;
            }
            else
            {
                // every input => new buffer
                text = miv(null);
            }

            Console.Clear();

            if (text != null)
            {
                File.WriteAllText(path, text);
                Console.WriteLine("Content has been saved to " + Kernel.file);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        public static void ExitMiv()
        {
            Console.Clear();
            Console.WriteLine("Exited Editor Context.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
