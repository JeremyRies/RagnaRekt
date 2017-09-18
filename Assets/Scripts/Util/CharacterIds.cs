using System.Collections.Generic;

namespace Util
{
    public class CharacterIds  {

        public class CharacterDict : Dictionary<string, int>
        {
            public CharacterDict()
            {
                Add("Thor", 1);
                Add("Loki", 2);
            }
        }

        public static CharacterDict CharacterIdentifier= new CharacterDict(); 
    }
}
