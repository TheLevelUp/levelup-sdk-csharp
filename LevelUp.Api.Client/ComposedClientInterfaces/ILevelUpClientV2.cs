using LevelUp.Api.Client.ClientInterfaces;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// Approved collection of interfaces for version 2.X of the SDK
    /// </summary>
    public interface ILevelUpClientV2 : IComposedInterface,
                                        IAuthenticate,
                                        ICreateCreditCards,
                                        ICreateDetachedRefund,
                                        ICreateOrders,
                                        ICreateRefund,
                                        IDestroyCreditCards,
                                        ILookupUserLoyalty,
                                        IModifyUser,
                                        IQueryCreditCards,
                                        IQueryMerchantData,
                                        IQueryOrders,
                                        IQueryUser,
                                        IRetrieveMerchantFundedCredit,
                                        IRetrievePaymentToken,
                                        ICreateGiftCardValue,
                                        IDestroyGiftCardValue,
                                        IManageRemoteCheckData
    {
    }
}