namespace Laba2.Task2;

public class Transaction
{
    private static List<Guid> UserIds = new List<Guid> {
        Guid.Parse("d2f7c3a9-8e4b-4c12-9a2f-0b1c2d3e4f50"),
        Guid.Parse("1a2b3c4d-5e6f-7081-92a3-b4c5d6e7f809"),
        Guid.Parse("aa11bb22-cc33-dd44-ee55-001122334455"),
        Guid.Parse("0f1e2d3c-4b5a-6978-8c9d-0a1b2c3d4e5f"),
        Guid.Parse("12345678-90ab-cdef-1234-567890abcdef"),
        Guid.Parse("fedcba98-7654-3210-fedc-ba9876543210"),
        Guid.Parse("abcdef01-2345-6789-abcd-ef0123456789"),
        Guid.Parse("11111111-2222-3333-4444-555555555555"),
        Guid.Parse("99999999-8888-7777-6666-555555555555"),
        Guid.Parse("0a0b0c0d-0e0f-1011-1213-141516171819"),
        Guid.Parse("cafebabe-dead-beef-cafe-babecafedead"),
        Guid.Parse("7f6e5d4c-3b2a-1908-0706-050403020100"),
        Guid.Parse("abcdefab-cdef-abcd-efab-cdefabcdefab"),
        Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"),
        Guid.Parse("89abcdef-0123-4567-89ab-cdef01234567"),
        Guid.Parse("deadbeef-dead-beef-dead-beefdeadbeef"),
        Guid.Parse("00112233-4455-6677-8899-aabbccddeeff"),
        Guid.Parse("10203040-5060-7080-90a0-b0c0d0e0f000"),
        Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
        Guid.Parse("123e4567-e89b-12d3-a456-426614174000")
    };
    private static int _id = 0;
    public int Id { get; private set; }
    public Guid UserId { get; }
    public int Amount { get; set; }
    public Currency Currency { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public bool WithCashback { get; }

    public Transaction()
    {
        var random = new Random();
        Id = Interlocked.Increment(ref _id);
        UserId = UserIds[random.Next(0, UserIds.Count)];
        Amount = random.Next(100, 1000);
        Currency = (Currency)random.Next(0, 3);
        Date = DateTime.UtcNow.AddDays(random.Next(0, 20));
        Type = new Random().Next(0, 3) == 0 ? TransactionType.Deposit : TransactionType.Withdrawal;
        WithCashback = Type == TransactionType.Withdrawal && new Random().Next(0, 10) < 3;
    }
}

public enum Currency { UAH, USD, EUR }
public enum TransactionType { Deposit, Withdrawal }