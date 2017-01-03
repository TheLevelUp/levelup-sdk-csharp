using LevelUp.Api.Client.ClientInterfaces;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// Approved collection of interfaces for version 3.X of the SDK
    /// </summary>
    public interface ILevelUpClientV3 : IComposedInterface,
                                        IAuthenticate,
                                        ICreateDetachedRefund,
                                        ICreateRefund,
                                        IQueryMerchantData,
                                        IQueryOrders,
                                        IRetrievePaymentToken,
                                        ICreateGiftCardValue,
                                        IDestroyGiftCardValue,
                                        IRetrieveMerchantFundedGiftCardCredit,
                                        IManageProposedOrders
    {
    }
}