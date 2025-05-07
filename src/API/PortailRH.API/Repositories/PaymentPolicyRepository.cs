namespace PortailRH.API.Repositories
{
    public class PaymentPolicyRepository : RepositoryBase<PaymentPolicy>, IPaymentPolicyRepository
    {
        private readonly PortailRHContext _dbContext;

        public PaymentPolicyRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Exemple d’une méthode spécifique
       
    }
}