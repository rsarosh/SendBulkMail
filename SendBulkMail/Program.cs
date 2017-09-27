using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SendBulkMail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string senderEmail = string.Empty;
                List<string> emails = ReadEmails();
                MailMessage message = new MailMessage();
               
                message.Subject = "Your subject";

                Console.WriteLine("Input your email and press enter (alias@microsoft.com):");
                senderEmail = Console.ReadLine();

                message.From = new System.Net.Mail.MailAddress(senderEmail);
                message.Body = System.IO.File.ReadAllText ("Content.txt");

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                Console.WriteLine("Input your password");
                string password = ReadPassword();
                smtp.Credentials = new NetworkCredential(senderEmail, password);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Wait...sending mail ...");
                Console.ForegroundColor = ConsoleColor.Green;

                foreach (string email in emails)
                {
                    message.To.Add(email);
                    smtp.Send(message);
                    Console.Write(".");
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Message Sent");
            Console.Read();

        }

        private static List<string> ReadEmails()
        {
            string line;
            List<String> lines = new List<string>();
            using (System.IO.StreamReader file = new System.IO.StreamReader("CustomerEmail.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    
                }
                file.Close();
                return lines;
            }
            

        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
    }
}

