using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
   public  class StringCleaner
    {
        //Per "Dirty" character wordt er een find en replace gedaan met de "Cleane" character
        //Deze functie werkt enkel als DirtyCharacter[x] == CleanCharacter[x]
        public string CleanString(string input)
        {
            string DirtyCharacters = "ŠŽšžŸÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïðñòóôõöùúûüýÿ";
            string CleanCharacters = "SZszYAAAAAACEEEEIIIIDNOOOOOUUUUYaaaaaaceeeeiiiidnooooouuuuyy";
            for (int i = 0; i < DirtyCharacters.Length; i++)
            {
                char DirtyChar = DirtyCharacters[i];
                char CleanChar = CleanCharacters[i];
                input = input.Replace(DirtyChar, CleanChar);
            }
            return input;
        }

        //Run (bijna) elke string van InputHolder door CleanString
        public InputHolder CleanInputHolder(InputHolder inputHolder)
        {
            inputHolder.Name = CleanString(inputHolder.Name);
            inputHolder.FirstName = CleanString(inputHolder.FirstName);
            inputHolder.Company = CleanString(inputHolder.Company);
            inputHolder.NumberPlate = CleanString(inputHolder.NumberPlate);
            inputHolder.VoertuigNaam = CleanString(inputHolder.VoertuigNaam);
            inputHolder.Straat = CleanString(inputHolder.Straat);
            inputHolder.Post = CleanString(inputHolder.Post);
            inputHolder.Stad = CleanString(inputHolder.Stad);
            return inputHolder;
        }
    }
}
