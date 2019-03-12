using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdgparking.BL.Domain
{
    public class StringValidator
    {
        public StringValidator() { }

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
    }
}
