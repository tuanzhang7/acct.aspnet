using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.report.DTO
{
    public class Helper
    {
        public static void Mapping()
        {

            Mapper.CreateMap<acct.common.POCO.Customer, acct.common.POCO.Customer>()
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            Mapper.CreateMap<acct.common.POCO.GST, acct.common.POCO.GST>();

            Mapper.CreateMap<acct.common.POCO.OrderDetail, acct.common.POCO.OrderDetail>()
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            Mapper.CreateMap<acct.common.POCO.Order, acct.common.POCO.Order>()
                ;

            Mapper.CreateMap<acct.common.POCO.Quotation, acct.common.POCO.Quotation>();
            Mapper.CreateMap<acct.common.POCO.Invoice, acct.common.POCO.Invoice>();
            


        }
    }
}
