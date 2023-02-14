namespace Devlike.Characters
{
    public class Relationship
    {
        public Relationship(string firstName, string lastName, RelationType relation)
        {
            FirstName = firstName;
            LastName = lastName;
            Relation = relation;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public RelationType Relation { get; private set; }
    }
}
