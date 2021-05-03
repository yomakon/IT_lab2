using System;
using System.Numerics;

namespace RSA
{
  class Program
  {
    enum checkMode { 
      ENCRYPT_KEY,
      DECRYPT_KEY,
      KEYPAIR,
      PRIVKEY_RECOVERY
    }

    static bool KeypairCheck(Keypair keypair,checkMode mode) {
      if (keypair.r < 256) {
        Console.WriteLine("Ошибка: значение r слишком мало.");
        return false;
      }

      if ((mode == checkMode.KEYPAIR || mode == checkMode.ENCRYPT_KEY)  && keypair.e == 0) {
        Console.WriteLine("Ошибка: значение открытой экспоненты e некорректно.");
        return false;
      }

      if ((mode == checkMode.KEYPAIR || mode == checkMode.DECRYPT_KEY) && keypair.d <= 0) {
        Console.WriteLine("Ошибка: секретный ключ неверен.");
        return false;
      }
      return true;
    }

    static Keypair CreateKeypair(checkMode mode)
    {
      BigInteger p = 0, q = 0, e = 0, d = 0, r = 0;

      //pair creation for encryption
      bool isCorrect = false;
      if (mode == checkMode.KEYPAIR || mode == checkMode.PRIVKEY_RECOVERY) { 
        while(!isCorrect){
          try {  
          Console.WriteLine("Введите простое число p:");
          string p_str = Console.ReadLine();
          p = BigInteger.Parse(p_str);
          Console.WriteLine("Введите простое число q:");
          string q_str = Console.ReadLine();
          q = BigInteger.Parse(q_str);
          isCorrect = true;
          } catch {
            Console.WriteLine("Ошибка: некорректные значения.");
          }
        }
      }

      //
      isCorrect = false;
      if (mode == checkMode.DECRYPT_KEY || mode == checkMode.ENCRYPT_KEY) {
        while (!isCorrect){
          Console.WriteLine("Введите значение r:");
          try{
            string r_str = Console.ReadLine();
            r = BigInteger.Parse(r_str);
            isCorrect = true;
          }
          catch{
            Console.WriteLine("Ошибка: некорректное r.");
          }
        }
      }

      isCorrect = false;
      if (mode == checkMode.ENCRYPT_KEY || mode == checkMode.PRIVKEY_RECOVERY) {
        while (!isCorrect){
          Console.WriteLine("Введите значение e:");
          try{
            string e_str = Console.ReadLine();
            e = BigInteger.Parse(e_str);
            isCorrect = true;
          }catch{
            Console.WriteLine("Ошибка: некорректное е.");
          }
        }
      }

      isCorrect = false;
      if (mode == checkMode.DECRYPT_KEY){
        while (!isCorrect){
          Console.WriteLine("Введите значение d:");
          try{
            string d_str = Console.ReadLine();
            d = BigInteger.Parse(d_str);
            isCorrect = true;
          }
          catch{
            Console.WriteLine("Ошибка: некорректное значение d.");
          }
        }
      }
      return new Keypair(p,q,e,r,d);
    }

    static void UI_logic() {
      string srcStr;
      string resStr;
      bool isCorrect = false;
      Console.WriteLine("Демонстрация работы алгоритма RSA. Опции: ");
      while (!isCorrect) {
        Console.WriteLine("1 - Шифровать строку");
        Console.WriteLine("2 - Дешифровать строку");
        Console.WriteLine("3 - Сгенерировать пару ключей");
        Console.WriteLine("4 - Вычислить секретную экспоненту по значениям p, q, e");
        string usr_inpt = Console.ReadLine();
        if (usr_inpt.Length == 1){
          Keypair workingKeypair;
          switch (usr_inpt[0]){
            case '1':
              do { 
                workingKeypair = CreateKeypair(checkMode.ENCRYPT_KEY);
              } while(!KeypairCheck(workingKeypair,checkMode.ENCRYPT_KEY));

              Console.WriteLine("Введите исходную строку:");
              srcStr = Console.ReadLine();
              try { 
                resStr = RSA_crypt.Encrypt(srcStr,workingKeypair);
                Console.WriteLine("Исходная строка:" + srcStr);
                Console.WriteLine("Результат      :" + resStr);
              } catch {
                Console.WriteLine("Ошибка: шифрование данным ключом невозможно.");
              }
            break;

            case '2':
              do { 
                workingKeypair = CreateKeypair(checkMode.DECRYPT_KEY);
              } while(!KeypairCheck(workingKeypair,checkMode.DECRYPT_KEY));

              Console.WriteLine("Введите исходную строку:");
              srcStr = Console.ReadLine();
              try {
                resStr = RSA_crypt.Decrypt(srcStr, workingKeypair);
                Console.WriteLine(srcStr + " -> " + resStr);
                } catch {
                  Console.WriteLine("Ошибка: дешифрование данным ключом невозможно.");
                }
            break;

            case '3':
              do {
                workingKeypair = CreateKeypair(checkMode.KEYPAIR);
              } while (!KeypairCheck(workingKeypair, checkMode.KEYPAIR));
                Console.WriteLine("Ключи сгенерированы.");
                Console.WriteLine("Публичный ключ: ({0},{1})", workingKeypair.e, workingKeypair.r);
                Console.WriteLine("Секретный ключ: ({0},{1})", workingKeypair.d, workingKeypair.r);
            break;

            case '4':
              workingKeypair = CreateKeypair(checkMode.PRIVKEY_RECOVERY);
              Console.WriteLine("Результат вычисления: r = {0}, e = {1}, d = {2}"
                                ,workingKeypair.r, workingKeypair.e, 
                                workingKeypair.d == 0?"не вычислена":workingKeypair.d);
            break;
            case '0':
              Console.WriteLine("Выход...");
              isCorrect = true;
            break;
          }
          Console.WriteLine();
        }
      }
    }  

    static void Main(string[] args){
      UI_logic();
    }
  }
}