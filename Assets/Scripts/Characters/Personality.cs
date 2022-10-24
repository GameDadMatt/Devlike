using System.Collections.Generic;
using DataTypes;

namespace Characters
{
    public class Personality
    {
        //Private Values
        private List<Relationship> relationships;

        //Public Values
        public int Seed { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Nickname { get; private set; }
        public PersonalityType PersonalityType { get; private set; }
        public string Hobby { get; private set; }
        public string Role { get; private set; }
    }
}