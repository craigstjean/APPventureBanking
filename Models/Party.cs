namespace APPventureBanking.Models;

public enum PartyType
{
    Person,
    Entity
}

public class Party
{
    public int PartyId { get; set; }
    public PartyType Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EntityName { get; set; }
    public string PrimaryEmailAddress { get; set; }
    public int MailingAddressId { get; set; }
    public Address MailingAddress { get; set; }

    public string DisplayName
    {
        get
        {
            switch (Type)
            {
                case PartyType.Entity:
                    return EntityName;
                case PartyType.Person:
                    return FirstName + " " + LastName;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
