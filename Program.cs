using System;
using System.IO;
using System.Threading;

/*
    ###### #    ##     ## ## ##### #####  ##  # ##
         # #   # #      #  #     # #   #   #  #   #
         # #     #      #  #  #  # #   #   # #    #
         # #     #      #  #  #  # #   #   #      #
         # ####### ######  #  # #  #   #   # ######
                              #
    מחפש קונטר בכל כונן או תיקייה שתבחרו ואז מוחק אותו
*/

namespace מוחקונטר
{
    class Program
    {
        static uint c, all = 0; //סופרים את מספר התכנים שנמצאו, אחד לסריקה הנוכחית ואחד לכל הסריקות
        static string drive; //שם הכונן

        static void DeletDir(string path) //path פעולה שמוחקת את התיקיה בכתובת
        {
            string[] paths = Directory.GetFiles(path);
            foreach (string file in paths)
            {
                try
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("File " + file + " was deleted.");
                    Console.ResetColor();
                    c++;
                    all++;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Can't delete file " + file + "!");
                    Console.ResetColor();
                }
            }
            paths = Directory.GetDirectories(path);
            foreach (string dir in paths)
                DeletDir(dir); //מחיקת תיקיות המשנה
            try
            {
                Directory.Delete(path);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Directory " + path + " was deleted.");
                Console.ResetColor();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Can't delete directory " + path + "!");
                Console.ResetColor();
            }
        }

        static void Scan(string path) //ועל כל התיקיות תחתיה path פעולה שמבצעת סריקה על התיקיה 
        {
            try
            {
                if (!Directory.Exists(path)) return; //במידה והתיקיה לא קיימת - לא מתבצעת סריקה
                Console.WriteLine(path);
                Console.Title = "מוחקונטר     " + all;
                string[] dirs = Directory.GetDirectories(path);
                if (dirs.Length > 500 && !path.ToLower().Equals(drive)) return; //השורה הזאת מונעת מהתוכנה לסרוק תיקייה עם יותר מ-500 תיקיות
                string[] files = Directory.GetFiles(path);
                if (files.Length > 500 && !path.ToLower().Equals(drive)) return; //השורה הזאת מונעת מהתוכנה לסרוק תיקייה עם יותר מ-500 קבצים
                uint counterPathsFound = 0; //סופר "ראיות" שנמצאות בתוך תיקייה כדי לבדוק האם היא מכילה קונטר
                foreach (string dir in dirs) //סריקת התיקיות
                {
                    if (dir.ToLower().IndexOf("counter-strike") != -1 || dir.ToLower().IndexOf("counter strike") != -1 || dir.IndexOf("קונטר") != -1) //בדיקה - האם לתיקייה יש שם שקשור לקונטר
                    {
                        DeletDir(dir);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Directory " + dir + " was deleted.");
                        Console.ResetColor();
                    }
                    if (dir.ToLower().IndexOf("valve") != -1 || dir.ToLower().IndexOf("cstrike") != -1 || dir.ToLower().IndexOf("config") != -1)
                        counterPathsFound++;
                }
                foreach (string file in files) //סריקת הקבצים
                {
                    if (file.ToLower().IndexOf("hl.exe") != -1 || file.ToLower().IndexOf("steam_api.dll") != -1 || file.ToLower().IndexOf("hlds.exe") != -1 || file.ToLower().IndexOf("hltv.exe") != -1 || file.ToLower().IndexOf("hlds.exe") != -1)
                        counterPathsFound++;
                    if (file.ToLower().IndexOf("counter-strike") != -1 || file.ToLower().IndexOf("counter strike") != -1 || file.IndexOf("קונטר") != -1) //בדיקה - האם לקובץ יש שם שקשור לקונטר
                    {
                        File.Delete(file); c++; all++;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("File " + file + " was deleted.");
                        Console.ResetColor();
                    }
                }
                if (counterPathsFound > 2) //בודק האם קיימות מספיק ראיות כדי למחוק את התיקייה
                {
                    DeletDir(path);
                    return;
                }
                foreach (string dir in dirs) //קריאה לפעולה שתסרוק את כל התיקיות בתוך התיקיה הנוכחית
                    Scan(dir);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error!");
                Console.ResetColor();
            }
        }

        static void Main(string[] args) //פעולה ראשית, הקוד מתחיל כאן
        {
            //args[0]-הכתובת שאנו רוצים לסרוק מוגדרת כ
            Console.Title = "מוחקונטר";
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("    ###### #    ##    ## ## ##### ##### ##  # ##");
            Console.WriteLine("         # #   # #     #  #     # #   #  #  #   #");
            Console.WriteLine("         # #     #     #  #  #  # #   #  # #    #");
            Console.WriteLine("         # #     #     #  #  #  # #   #  #      #");
            Console.WriteLine("1.3.3    # ####### #####  #  # #  #   #  # ######");
            Console.WriteLine("By L54                       #");
            Thread.Sleep(500);
            Console.ResetColor();
            Console.WriteLine();
            /*args = new string[1];
              args[0] = @"Z:\";     (השורות האלה מגדירות אוטומטית את התיקיה למיקום הרצוי, אם לא מגדירים כתובת בקוד מראש, אז יש להכניס משתנים דרך שורת הפקודה (או לגרור את התיקייה לתוכנה*/
            if (args.Length == 0)
            {
                Console.WriteLine("usage: רטנוקחומ [path]");
                Thread.Sleep(5000);
            }
            else
            {
                if (Directory.Exists(args[0]))
                {
                    long size = new DriveInfo(args[0].Substring(0, 3)).TotalFreeSpace;
                    drive = args[0].Substring(0, 3);
                    try
                    {
                        while (true) //לולאה שסורקת את הכתובת מחדש לאחר שינוי בגודל הכונן
                        {
                            c = 0;
                            Console.WriteLine("Scanning...");
                            Scan(args[0]); //סרוק את התיקיה או הכונן שהוגדר
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Scan complete! " + c + " Counter-Strike files were deleted.");
                            Console.ResetColor();
                            while (size.Equals(new DriveInfo(args[0].Substring(0, 3)).TotalFreeSpace)) //מחכה לשינוי בגודל הכונן
                                Thread.Sleep(60000);
                            size = new DriveInfo(args[0].Substring(0, 3)).TotalFreeSpace;
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Critical error!");
                        Console.ResetColor();
                    }
                }
                else
                    Console.WriteLine("Path not found!");
            }
        }
    }
}