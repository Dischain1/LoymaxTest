using Data.Enums;
using LoymaxTest.Models;
using Services.Accounts.Models;
using Services.Transactions.Models;

namespace LoymaxTest.Automapper
{
    using AutoMapper;
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AddAccountModel, AddAccountDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(model => model.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(model => model.LastName))
                .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(model => model.Patronymic))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(model => model.DateOfBirth));

            CreateMap<WithdrawalTransactionModel, AddTransactionDto>()
                .ConstructUsing(opt => new AddTransactionDto(opt.AccountId, opt.Withdrawal, TransactionType.Withdrawal));

            CreateMap<DepositTransactionModel, AddTransactionDto>()
                .ConstructUsing(opt => new AddTransactionDto(opt.AccountId, opt.Deposit, TransactionType.Deposit));

        }
    }
}
