using System;
using System.Windows;

namespace MIKOLAJCZAK_Antoine_TP2_ST2TRD
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // the "??" operator checks for nullability and value all at once.
            // because ConvertCheckBox.IsChecked is of type __bool ?__ which
            // is a nullable boolean, so it can either be true, false or null
            var toDecrypt = ConvertCheckBox.IsChecked ?? false;
            var inputText = InputTextBox.Text;
            var inputKey = InputKey.Text;
            var encryptionmethod = EncryptionComboBox.Text;


            if (toDecrypt)
            {
                OutputTextBox.Text = $"{inputText} is gibberish and should be decrypted using {encryptionmethod}";
            }
            else
            {
                OutputTextBox.Text = $"{inputText} was written as an input to be encrypted using {encryptionmethod}";
            }

            if (encryptionmethod == "Caesar")
            {
                OutputTextBox.Text = Caesar.Code(inputText, toDecrypt , inputKey);
            }

            if (encryptionmethod == "Vignere")
            {
                OutputTextBox.Text = Vigenere.Code(inputText, toDecrypt, inputKey);
            }
            if (encryptionmethod == "Boolean")
            {
                OutputTextBox.Text = AsciiBinaryConverter.Code(inputText, toDecrypt);
            }
            
        }
    }

    // This class is not instantiated because it is static. 
    // You might not be able to do this so easily...
    // And each class should have its own file !
    internal static class Caesar
    {
        public static string Code(string inputText, bool toDecrypt, string KeyString)
        {
            // Ternary operator - Google it
            return toDecrypt ? Decrypt(inputText, KeyString) : Encrypt(inputText, KeyString);
        }

        private static bool TestKey(int Key)
        {
            bool IsNumber = true;
            if (Key == 0)
            {
                MessageBox.Show("Don't try this ! :)..Caesar's key should be only number different de 0");
                IsNumber = false;
            }

            return IsNumber;
        }


        private static string Encrypt(string inputText, string KeyString, bool Inversed = false)
        {
            
            int.TryParse(KeyString, out var key);
            
            var alphabetUp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var alphabetLow = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            string EncryptedMessage = "";
            char[] alphabet = null;

            if (TestKey(key) == false)
            {
                return "Sorry it's not possible to have a negative key";
            }


            var rotation = key < 0 ? ((key % 26) + 26) % 26 : key % 26;
            //bool IsUpper = true; 

            if (Inversed == true)
            {
                rotation = rotation - 2 * rotation;
            }
            

            for (int i = 0; i < inputText.Length; i++)
            {
                if (char.IsLetter(inputText[i]))
                {
                    bool Verif = Char.IsUpper(inputText[i]);

                    if (Verif == true)
                    {
                        alphabet = alphabetUp;
                    }
                    else
                    {
                        alphabet = alphabetLow;
                    }

                    if (inputText[i] == ' ')
                    {
                        EncryptedMessage += ' ';
                    }

                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (inputText[i] == alphabet[j])
                        {
                            EncryptedMessage += alphabet[(j + rotation + 26) % 26];
                        }
                    }
                }
                else
                {
                    EncryptedMessage += inputText[i];
                }
                    
            }
            
            
            return $"{EncryptedMessage} encrypted with Caesar !";
        }

        private static string Decrypt(string inputText, string KeyString)
        {
            var output = Encrypt(inputText, KeyString, true);
            return $"{output} was decrypted with Caesar !";
        }
    }

    internal static class Vigenere
    {
        
        private static bool TestKey(string Key)
        {
            bool IsNumber = false;
            for (int i = 0; i < Key.Length; i++)
            {
                if (!char.IsLetter(Key[i]))
                {
                    MessageBox.Show("Don't try this ! :).. Vignere key should be a string not a number");
                    IsNumber = true;
                }
            }
            

            return IsNumber;
        }
        
        public static string Code(string inputText, bool toDecrypt, string KeyString)
        {
            // Ternary operator - Google it
            return toDecrypt ? Decrypt(inputText, KeyString) : Encrypt(inputText, KeyString);
        }

        private static int Mod(int x, int y)
        {
            return (x % y + y) % y;
        }
        
        
        
        private static string Encrypt(string InputText, string KeyString, bool inversed = true)
        {
            
           
            
            string EncryptedMessage = "";
            var alphabetUp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var alphabetLow = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            int counterNonLetter = 0;

             if (TestKey(KeyString) == true)
             {
                 return "Sorry it's not possible to have number key !";
             }


            for (int i = 0; i < InputText.Length; i++)
            {
                if (char.IsLetter(InputText[i]))
                {
                    bool IsUpper = char.IsUpper(InputText[i]);
                    char OffSet = IsUpper ? 'A' : 'a';
                    int IndexKey = (i - counterNonLetter) % KeyString.Length;
                    int k = (IsUpper ? char.ToUpper(KeyString[IndexKey]) : char.ToLower(KeyString[IndexKey])) - OffSet;

                    k = inversed ? k : -k;
                    char Letter = (char) ((Mod(((InputText[i] + k) - OffSet), 26)) + OffSet);

                    EncryptedMessage += Letter;

                }
                else
                {
                    EncryptedMessage += InputText[i];
                    ++counterNonLetter;
                }
            }

            return EncryptedMessage;
        }

        private static string Decrypt(string InputText, string KeyString, bool inversed = false)
        {
            return Encrypt(InputText, KeyString, false );
        }
        
    }

    internal static class AsciiBinaryConverter
    {
         public static string Code(string inputText, bool toDecrypt)
         {
            // Ternary operator - Google it
            return toDecrypt ? Decrypt(inputText) : Encrypt(inputText);
         }

         private static string BinaryToAscii(string InputText, bool display)
         {
            string ascii = null;
            int charAsciiCode = 0,j=1<<7;
            if((InputText.Length%8) !=0) throw new Exception("La longueur de la chaine n'est pas divisible par 8.");

            for(int i= 0; i< InputText.Length; i++)
            {
                if(InputText[i] == '1')
                {
                    charAsciiCode = j | charAsciiCode;
                }
                else if (InputText[i] !='0')throw new Exception("Seulement les caractères 1 et 0 sont acceptés.");
                if (((i+1) % 8) == 0 && i > 0)
                {
                    //on transforme charAsciiCode en carctère et on l'ajoute à la chaine finale (ascii)
                    ascii += (char)charAsciiCode;
                    //si diplay == true, on affiche le code binaire + le carctère
                    if (display) Console.WriteLine(InputText.Substring(i - 7, 8) + " : " + (char)charAsciiCode);
                    //j est remis sur 128 : 10000000 (en binaire)
                    j = 1 << 7;
                    //charAsciiCode est remis à 0 : 00000000 (en binaire)
                    charAsciiCode = 0;                    
                }
                //sinon on décale le bit de j vers la droite
                //exemple :
                    //j initiale : 01000000
                    //j après >> 1 : 00100000
                else j = j >> 1;
            }
            //retourne la chaine ascii
            return $"{ascii} decrypted with Boolean !" ;

         }
         private static string Decrypt(string InputText)
         {
            return BinaryToAscii(InputText, false);
         }


        private static string AsciiToBinary(string inputText, bool display)
        {
            //chaine contenant le code binaire final
            string binary = null;
           
            //j : indicateur pour les opérations binaires
            int j = 0;
            //on parcourt la chaine ascii
            for (int i = 0; i < inputText.Length; i++)
            {
                //j est mis sur 128 : 10000000 (en binaire)
                j = 1 << 7;
                if(display) Console.Write(inputText[i] + " :\t");
                //durant cette boucle, le bit 1 de j sera déplacé vers la droite
                for (int k = 0; k < 8; k++)
                {
                    //si l'opérateur & (AND) est différent de 0
                    if ((inputText[i] & j) != 0)
                    {
                        //ajouter le caractère 1 à la chaine binary
                        if (display) Console.Write("1");
                        binary += "1";
                    }
                    //sinon
                    else
                    {
                        //ajouter le caractère 0 à la chaine binary
                        if (display) Console.Write("0");
                        binary += "0";
                    }
                    //on décale le bit de j vers la droite
                    //exemple :
                        //j initiale : 01000000
                        //j après >> 1 : 00100000
                    j = j >> 1;
                }
                if (display) Console.WriteLine();
            }
            //on retourne le code binaire de ascii sous forme de string
            return $"{binary} encrypted with Boolean !";
        }
        private static string Encrypt(string inputText)
        {
            return AsciiToBinary(inputText, false);
        }

          
        

    }
    

}
