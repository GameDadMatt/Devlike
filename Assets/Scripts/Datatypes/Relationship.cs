namespace DataTypes
{
    public class Relationship
    {
        public Relationship(string firstName, string lastName, Relation relation)
        {
            FirstName = firstName;
            LastName = lastName;
            Relation = relation;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Relation Relation { get; private set; }
    }
}
