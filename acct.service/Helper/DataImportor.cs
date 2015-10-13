using acct.common.POCO;
using acct.common.Repository;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.service.Helper
{
    public class DataImportor
    {
        private CustomerSvc custSvr;
        private GSTSvc gstSvr;
        private SalesmanSvc salesmanSvc;
        private InvoiceSvc invSvr;
        private OrderDetailSvc orderDetailSvr;
        private PaymentSvc paymentSvc;
        private PaymentDetailSvc paymentDetailSvc;
        
        public DataImportor(ICustomerRepo iCustomerRepo, IGSTRepo iGSTRepo,
            IInvoiceRepo iInvoiceRepo, IOrderDetailRepo iOrderDetailRepo, 
            ISalesmanRepo iSalesmanRepo,IPaymentRepo iPaymentRepo,IPaymentDetailRepo iPaymentDetailRepo)
        {
            custSvr = new CustomerSvc(iCustomerRepo);
            gstSvr = new GSTSvc(iGSTRepo);
            invSvr = new InvoiceSvc(iInvoiceRepo);
            orderDetailSvr = new OrderDetailSvc(iOrderDetailRepo);
            salesmanSvc = new SalesmanSvc(iSalesmanRepo);
            paymentSvc = new PaymentSvc(iPaymentRepo);
            paymentDetailSvc = new PaymentDetailSvc(iPaymentDetailRepo);
        }

        public int ImportSalesman(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;

            int count = 0;

            while (CSV.Read())
            {
                string name = CSV.GetField<string>("name");

                if (salesmanSvc.GetByName(name)!=null)
                {
                    //update record?
                }
                else
                {
                    Salesman salesman = new Salesman
                    {
                        Name = name
                    };
                    try
                    {
                        salesmanSvc.Save(salesman);

                    }
                    catch (Exception e)
                    {
                        //throw new Exception("Save Customer Error", e);
                    }
                }
            }
            
            return count;
        }

        public int ImportCustomer(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;

            int count = 0;
            List<Customer> list = new List<Customer>();
            Dictionary<decimal, int> GSTDictionary = gstSvr.GetAll().ToDictionary(t => t.Rate, t => t.Id);
            RegexUtilities util = new RegexUtilities();
            while (CSV.Read())
            {
                string name = CSV.GetField<string>("name");
                string phone = CSV.GetField<string>("phone");
                string fax = CSV.GetField<string>("fax");
                string email = CSV.GetField<string>("email");
                string contactName = CSV.GetField<string>("contactName");
                string address = CSV.GetField<string>("address");
                string address_line_1 = CSV.GetField<string>("address_line_1");
                string address_line_2 = CSV.GetField<string>("address_line_2");
                string address_line_3 = CSV.GetField<string>("address_line_3");
                string address_line_4 = CSV.GetField<string>("address_line_4");
                string GSTRate = CSV.GetField<string>("GSTRate");

                decimal _GSTRate = decimal.Parse(GSTRate);

                string _address;
                if (!string.IsNullOrEmpty(address))
                {
                    _address = address.Replace("|", System.Environment.NewLine);
                }
                else
                {
                    _address = string.Join(System.Environment.NewLine,
                    new string[] { address_line_1, address_line_2, address_line_3, address_line_4 });
                }

                int GSTId;
                //Get GST by Rate
                if (GSTDictionary.ContainsKey(_GSTRate))
                {
                    GSTId = GSTDictionary[_GSTRate];
                }
                else
                {
                    GSTId = GSTDictionary.First().Value;
                }
                if (!util.IsValidEmail(email))
                {
                    email = null;
                }

                if (custSvr.IsCustomerNameExist(name))
                {
                    //update record?
                }
                else
                {
                    Customer customer = new Customer
                    {
                        Name = name,
                        Phone = phone,
                        Fax = fax,
                        Address = _address,
                        idmas_GST = GSTId,
                        Email = email,
                        ContactName = contactName

                    };
                    //check duplicate name in the list
                    if (!list.Exists(c => c.Name.ToUpper() == customer.Name.ToUpper()))
                    {
                        list.Add(customer);
                    }
                }
            }
            try
            {
                custSvr.Save(list);

            }
            catch (Exception e)
            {
                //throw new Exception("Save Customer Error", e);
            }
            return count;
        }
        
        public int ImportInvioceHeader(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;

            List<Invoice> list = new List<Invoice>();
            int count = 0;

            try
            {
                while (CSV.Read())
                {
                    string invoiceNo = CSV.GetField<string>("invoice#").Trim();
                    string customerName = CSV.GetField<string>("customer");
                    string salesmanName = CSV.GetField<string>("salesman");
                    string date = CSV.GetField<string>("date");
                    string remark = CSV.GetField<string>("remark");
                    string GSTRate = CSV.GetField<string>("SalesTaxRate");

                    decimal _GSTRate = 0;
                    decimal.TryParse(GSTRate, out _GSTRate);

                    
                    //Get GST by Code
                    if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(invoiceNo) && !string.IsNullOrEmpty(date))
                    {
                        DateTime orderDate = DateTime.Parse(date);
                        Customer customer = custSvr.GetByName(customerName);
                        if (customer != null)
                        {
                            Salesman salesman = salesmanSvc.GetByName(salesmanName);
                            Invoice invoice = invSvr.GetByInvoiceNumber(invoiceNo);
                            if (invoice != null)
                            {
                                //update salesman only
                                invoice.SalesmanId = salesman.Id;
                            }
                            else
                            {
                               
                                Invoice newInvoice = new Invoice
                                {
                                    OrderNumber = invoiceNo,
                                    OrderDate = orderDate,
                                    CustomerId = customer.Id,
                                    SalesmanId = salesman.Id,
                                    Remark = remark,
                                    GSTRate = _GSTRate
                                };
                                list.Add(newInvoice);
                            }
                        }
                    }

                }

                invSvr.Save(list);
                count = list.Count();
            }
            catch (Exception e)
            {
                throw new Exception("Save Invoice Error", e);
            }
            return count;
        }
        
        public int ImportInvioceDetail(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;
            int count = 0;
            List<OrderDetail> list = new List<OrderDetail>();
            try
            {
                while (CSV.Read())
                {
                    string invoiceNo = CSV.GetField<string>("invoice#").Trim();

                    string description = CSV.GetField<string>("description");
                    string price = CSV.GetField<string>("UnitPrice");
                    string qty = CSV.GetField<string>("qty");
                    string strDiscount = CSV.GetField<string>("Discount");
                    

                    decimal unitPrice = decimal.Parse(price);
                    decimal Qty = decimal.Parse(qty);
                    decimal discount = decimal.Parse(strDiscount);
                    //Get GST by Code
                    Invoice invoice = invSvr.GetByInvoiceNumber(invoiceNo);
                    if (invoice != null)
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            Description = description,
                            UnitPrice = unitPrice,
                            Discount=discount,
                            Qty = Qty,
                            OrderId = invoice.Id
                        };
                        list.Add(orderDetail);
                    }
                }
                orderDetailSvr.Save(list);
            }
            catch (Exception e)
            {
                throw new Exception("Save Invoice details Error", e);
            }
            return count;
        }
        
        public int ImportInvioce(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            int count = 0;
            while (CSV.Read())
            {
                string invoiceNo = CSV.GetField<string>("invoice#").Trim();
                string customerName = CSV.GetField<string>("customer");
                string date = CSV.GetField<string>("date");
                string description = CSV.GetField<string>("description");
                string price = CSV.GetField<string>("price");
                string qty = CSV.GetField<string>("qty");

                DateTime orderDate = DateTime.Parse(date);
                decimal unitPrice = decimal.Parse(price);
                decimal Qty = decimal.Parse(qty);

                Customer customer = custSvr.GetByName(customerName);
                if (customer != null)
                {
                    Invoice invoice = invSvr.GetByInvoiceNumber(invoiceNo);
                    if (invoice != null)
                    {
                        //update record?
                    }
                    else
                    {
                        
                        OrderDetail orderDetail = new OrderDetail
                        {
                            Description = description,
                            UnitPrice = unitPrice,
                            Qty = Qty
                        };
                       
                        Invoice newInvoice = new Invoice
                        {
                            OrderDate = orderDate,
                            OrderNumber = invoiceNo,
                            CustomerId = customer.Id
                        };
                        newInvoice.OrderDetail.Add(orderDetail);
                        try
                        {
                            invSvr.Save(newInvoice);
                            count++;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Save Invoice Error", e);
                        }
                    }

                }
            }
            return count;
        }

        public int ImportPaymentHeader(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;

            List<Payment> list = new List<Payment>();
            int count = 0;
            try
            {
                while (CSV.Read())
                {
                    string paymentNo = CSV.GetField<string>("payment#").Trim();
                    string customerName = CSV.GetField<string>("customer");
                    string date = CSV.GetField<string>("date");
                    string PaymentMethod = CSV.GetField<string>("PaymentMethod");
                    string ChequeNo = CSV.GetField<string>("ChequeNo");
                    string Amount = CSV.GetField<string>("Amount");

                    if (!string.IsNullOrEmpty(customerName) && !string.IsNullOrEmpty(paymentNo) && !string.IsNullOrEmpty(date))
                    {
                        DateTime orderDate = DateTime.Parse(date);
                        decimal _Amount = Decimal.Parse(Amount);
                        Customer customer = custSvr.GetByName(customerName);
                        if (customer != null)
                        {
                            //Payment entity = paymentSvc.GetById(paymentNo);
                            //if (entity != null)
                            //{
                            //}
                            //else
                            //{

                                Payment newInvoice = new Payment
                                { 
                                    PaymentMethod = PaymentMethod,
                                    ReferenceNumber = ChequeNo,
                                    Date = orderDate,
                                    CustomerId = customer.Id,
                                    Remarks = paymentNo,
                                    Amount = _Amount
                                };
                                list.Add(newInvoice);
                            //}
                        }
                    }

                }

                paymentSvc.Save(list);
                count = list.Count();
            }
            catch (Exception e)
            {
                throw new Exception("Save Invoice Error", e);
            }
            return count;
        }

        public int ImportPaymentDetail(Stream csvFile)
        {
            StreamReader textReader = new StreamReader(csvFile);
            CsvReader CSV = new CsvReader(textReader);
            CSV.Configuration.IsHeaderCaseSensitive = false;
            CSV.Configuration.TrimFields = true;
            CSV.Configuration.WillThrowOnMissingField = false;

            List<PaymentDetail> list = new List<PaymentDetail>();
            int count = 0;
            try
            {
                while (CSV.Read())
                {
                    string paymentNo = CSV.GetField<string>("payment#").Trim();
                    string invoiceNo = CSV.GetField<string>("invoice#");
                    string date = CSV.GetField<string>("date");
                    string Amount = CSV.GetField<string>("amount");


                    if (!string.IsNullOrEmpty(invoiceNo) && !string.IsNullOrEmpty(paymentNo) )
                    {
                        DateTime orderDate = DateTime.Parse(date);
                        decimal _Amount = Decimal.Parse(Amount);
                        Invoice invoice = invSvr.GetByInvoiceNumber(invoiceNo.PadLeft(6, '0'));

                        if (invoice != null)
                        {
                            Payment entity = paymentSvc.GetByRemark(paymentNo);
                            if (entity != null)
                            {
                                PaymentDetail newInvoice = new PaymentDetail
                                { 
                                    InvoiceId = invoice.Id,
                                    PaymentId  = entity.Id,
                                    Amount = _Amount
                                };
                                list.Add(newInvoice);
                            }
                        }
                    }

                }

                paymentDetailSvc.Save(list);
                count = list.Count();
            }
            catch (Exception e)
            {
                throw new Exception("Save Payment Detail Error", e);
            }
            return count;
        }
        
    }

}
