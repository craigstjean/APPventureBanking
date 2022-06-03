namespace APPventureBanking.Services;

public class CardService
{
    public string NewCardNumber()
    {
        return "7" + Random.Shared.NextInt64(999).ToString().PadLeft(3, '0')
                   + "-" + Random.Shared.NextInt64(9999).ToString().PadLeft(4, '0')
                   + "-" + Random.Shared.NextInt64(9999).ToString().PadLeft(4, '0')
                   + "-" + Random.Shared.NextInt64(9999).ToString().PadLeft(4, '0');
    }

    public string NewSecurityCode()
    {
        return Random.Shared.NextInt64(999).ToString()
            .PadLeft(3, '0');
    }
}
