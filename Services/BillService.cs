using APPventureBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Services;

public class BillService
{
    private readonly BankContext _context;
    
    public BillService(BankContext context)
    {
        _context = context;
    }
    
    public decimal GetAmountPaid(int billId)
    {
        var bill = _context.Bills
            .Include(b => b.AssociatedTransactions)
            .FirstOrDefault(b => b.BillId == billId);

        if (bill == null)
        {
            return (decimal) 0.0;
        }
        
        return bill.AssociatedTransactions.Sum(t => t.Amount);
    }
}
