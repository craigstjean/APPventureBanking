using APPventureBanking.Models;

namespace APPventureBanking.Services;

public class AccountService
{
    private const int StartingAccountNumber = 1000000;
    
    private readonly BankContext _context;
    
    public AccountService(BankContext context)
    {
        _context = context;
    }
    
    public int NextAccountNumber()
    {
        var maxAccountNumber = _context.Accounts.Select(a => a!.AccountNumber).DefaultIfEmpty().Max();
        return Math.Max(StartingAccountNumber, maxAccountNumber) + 1;
    }
    
    public decimal GetBalance(int accountId)
    {
        // Temporary hack since SQLite doesn't support Sum on a decimal
        // Should have Sum as part of the query
        var amounts = (from t in _context.Transactions
                      where t.ToAccountId == accountId || t.FromAccountId == accountId
                      select t.ToAccountId == accountId ? t.Amount : -t.Amount).ToList();
        return amounts.Sum();
    }

    public decimal GetBalanceAsOfTransaction(int accountId, int maxTransactionId)
    {
        // Temporary hack similar to GetBalance
        // Not an ideal method to re-calculate per transaction constantly
        var amounts = (from t in _context.Transactions
                      where (t.ToAccountId == accountId || t.FromAccountId == accountId)
                          && t.TransactionId <= maxTransactionId
                      select t.ToAccountId == accountId ? t.Amount : -t.Amount).ToList();
        return amounts.Sum();
    }
}
