/******************************************************************************/
/*                                                                            */
/*                         File: Program.cs                                   */
/*                   Created By: Dmitry Diordichuk                            */
/*                        Email: cort@mail.ru                                 */
/*                                                                            */
/*                 File Created: 22nd April 2020 12:05:07 pm                  */
/*                Last Modified: 22nd April 2020 12:17:10 pm                  */
/*                                                                            */
/******************************************************************************/

using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

namespace RSA
{
	class Program
	{
		private static List<BigInteger>	EncryptMessage(string message, BigInteger e, BigInteger n)
		{
			List<BigInteger>	secretMessage;

			secretMessage = new List<BigInteger>();
			foreach (char letter in message)
			{
				secretMessage.Add(Algo.Encrypt(e, n, letter));
			}
			return (secretMessage);
		}
		private static string	DecryptMessage(List<BigInteger> c, BigInteger d, BigInteger n)
		{
			List<char> message;

			message = new List<char>();
			foreach (var el in c)
			{
				message.Add((char)Algo.Decrypt(d, n, el));
			}
			return (new string(message.ToArray()));
		}
		static void Main(string[] args)
		{
			string				message;
			ConsoleKey			key;
			BigInteger[]		keys;
			List<BigInteger>	c;

			keys = new BigInteger[3];
			Console.WriteLine("Нажмите F1 для того чтобы создать ключи.");
			Console.WriteLine("Нажмите F2 для шифрования.");
			Console.WriteLine("Нажмите F3 для расшифрования.");
			Console.WriteLine("Нажмите q для выхода.");
            while (true)
			{
                key = Console.ReadKey().Key;
				switch (key)
				{
					case ConsoleKey.F1:
						Console.WriteLine("Подготовка ключей может занять какое-то время.");
						keys = Algo.GenerateRSAKeys();
						using (StreamWriter writer = new StreamWriter("./keys.txt"))
						{
							writer.WriteLine($"e: {keys[0]}");
							writer.WriteLine($"d: {keys[1]}");
							writer.WriteLine($"n: {keys[2]}");
						}
						Console.WriteLine("Ключи были записанны в keys.txt");
						break;
					case ConsoleKey.F2:
						Console.WriteLine("Введите сообщение для шифрования:");
						message = Console.ReadLine();
                        using (StreamReader reader = new StreamReader("./keys.txt"))
                        {
						    keys[0] = BigInteger.Parse( reader.ReadLine().Replace( "e: ", "" ) );
                            keys[1] = BigInteger.Parse( reader.ReadLine().Replace( "d: ", "" ) );
                            keys[2] = BigInteger.Parse( reader.ReadLine().Replace( "n: ", "" ) );
					    }
                        Console.WriteLine($"Открытый ключ (e, n) = ({keys[0]}, {keys[2]})");
                        c = EncryptMessage(message, keys[0], keys[2]);
                        using( StreamWriter writer = new StreamWriter( "./secretMessage.txt" ) )
                        {
                            foreach (var n in c)
                            {
                                writer.WriteLine(n.ToString());
                            }
                        }
					    Console.WriteLine( "Зашифрованное сообщение было записано в secretMessage.txt" );
					    break;
					case ConsoleKey.F3:
                        using (StreamReader reader = new StreamReader("./keys.txt"))
                        {
						    keys[0] = BigInteger.Parse( reader.ReadLine().Replace("e: ", "") );
                            keys[1] = BigInteger.Parse( reader.ReadLine().Replace("d: ", "") );
                            keys[2] = BigInteger.Parse( reader.ReadLine().Replace("n: ", "") );
					    }
						Console.WriteLine($"Зыкрытый ключ (d, n) = ({keys[1]}, {keys[2]})");
                        c = new List<BigInteger>();
                        using ( StreamReader reader = new StreamReader( "./secretMessage.txt" ) )
                        {
                            while( reader.Peek() >= 0 )
                            {
							    Console.Write((char)Algo.Decrypt(keys[1], keys [2], BigInteger.Parse( reader.ReadLine() ) ) );
                            }
                        }
					    Console.Write( '\n' );
					    break;
					case ConsoleKey.Q:
						Environment.Exit(0);
						break;
				}
			}
		}
	}
}
