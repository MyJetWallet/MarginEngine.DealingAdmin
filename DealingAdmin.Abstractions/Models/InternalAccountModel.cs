using SimpleTrading.Abstraction.Accounts;

namespace DealingAdmin.Abstractions.Models
{
    public class InternalAccountCrmModel
    {
        public string AccountId { get; set; }

        public bool IsInternal { get; set; }
    }

    public class InternalAccountModel : IInternalAccount
    {
        public string Id { get; set; }

        public bool IsInternal { get; set; }
    }

    public static class Helper
    {
        public static InternalAccountModel ToInternalAccountModel(this InternalAccountCrmModel src)
        {
            return new InternalAccountModel()
            {
                Id = src.AccountId,
                IsInternal = src.IsInternal
            };
        }
    }


}
