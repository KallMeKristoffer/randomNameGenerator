using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace RandomNameGen
{
    /*
    Summary:
        RandomName class, used to generator random name.
    */
    public class RandomName
    {
        /* 
        Summary:
            Class for holding the names from nameList.json
        */
        class NameList
        {
            public string[] boys { get; set; }
            public string[] girls { get; set; }
            public string[] last { get; set; }

            public NameList()
            {
                boys = new string[] { };
                girls = new string[] { };
                last = new string[] { };
            }
        }

        Random rand;
        List<string> Male;
        List<string> Female;
        List<string> Last;

        /* 
        Summary:
            Initialize new instance of RandomName class.
         Parameters:
            "rand" - A random used to pick names from the given file.
        */
        public RandomName(Random rand)
        {
            this.rand = rand;
            NameList l = new NameList();

            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader reader = new StreamReader("nameList.json"))
            using (JsonReader jreader = new JsonTextReader(reader))
            {
                l = serializer.Deserialize<NameList>(jreader);
            }

            Male = new List<string>(l.boys);
            Female = new List<string>(l.girls);
            Last = new List<string>(l.last);
        }

        /*
        Summary:
            Returns a new random name
         Parameters:
            "sex" - The sex of the person to be named. 'true' for male, 'false' for female.
            "middle" - Number of middle names that should be generated.
            "isInital" - Should the middle names be initials or not?
        */
        public string Generate(Sex sex, int middle = 0, bool isInital = false)
        {
            string first = sex == Sex.Male ? Male[rand.Next(Male.Count)] : Female[rand.Next(Female.Count)];
            string last = Last[rand.Next(Last.Count)];

            List<string> middles = new List<string>();

            for (int i = 0; i < middle; i++)
            {
                if (isInital)
                {
                    middles.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[rand.Next(0, 25)].ToString() + ".");
                }
                else
                {
                    middles.Add(sex == Sex.Male ? Male[rand.Next(Male.Count)] : Female[rand.Next(Female.Count)]);
                }
            }

            StringBuilder b = new StringBuilder();
            b.Append(first + " ");
            foreach (string m in middles)
            {
                b.Append(m + " ");
            }
            b.Append(last);

            return b.ToString();
        }

        /*
        Summary:
            Generates a list of random names.
        Parameters:
            "number" - The number of names to be generated.
            "maxMiddleNames" - The maximum number of middle names.
            "sex" - The sex of the names. If 'null' then the sex shall be randomized.
            "initials" - Should the middle names have initials? If 'null' this shall be randomized.
        Returns:
            List of strings of names.
        */
        public List<string> RandomNames(int number, int maxMiddleNames, Sex? sex = null, bool? initials = null)
        {
            List<string> names = new List<string>();

            for (int i = 0; i < number; i++)
            {
                if (sex != null && initials != null)
                {
                    names.Add(Generate((Sex)sex, rand.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else if (sex != null)
                {
                    bool init = rand.Next(0, 2) != 0;
                    names.Add(Generate((Sex)sex, rand.Next(0, maxMiddleNames + 1), init));
                }
                else if (initials != null)
                {
                    Sex s = (Sex)rand.Next(0, 2);
                    names.Add(Generate(s, rand.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else
                {
                    Sex s = (Sex)rand.Next(0, 2);
                    bool init = rand.Next(0, 2) != 0;
                    names.Add(Generate(s, rand.Next(0, maxMiddleNames + 1), init));
                }
            }

            return names;
        }
    }

    public enum Sex
    {
        Male,
        Female
    	}
	}
}