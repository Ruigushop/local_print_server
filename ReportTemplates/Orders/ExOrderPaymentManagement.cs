using Csla.C5;
using Ruigu.Crm.Utility;

namespace RuiguCrmReports.Orders
{
    using Ruigu.Crm.BL;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for OrderPaymentManagement.
    /// </summary>
    public partial class ExOrderPaymentManagement : Telerik.Reporting.Report
    {
        public ExOrderPaymentManagement()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void SetDateSource(PaymentRequest payOrder)
        {
            SupplierInvoiceCollection invoice = SupplierInvoice.GetBySupplierOrderNumber(payOrder.Order_Number);
            RuiguSupplier supplier = RuiguSupplier.GetByID(payOrder.Supplier_Id);

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(payOrder.Request_Time + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            //txtNumber.Value = payOrder.Order_Number;
            txtCreated.Value = dtStart.Add(toNow).ToString("yyyy-MM-dd HH:mm:ss");
            txtOrderNumber.Value = payOrder.Order_Number;
            txtBig.Value =NumberUtility.ToChinese(payOrder.Total_Amount);
            txtSmall.Value = payOrder.Total_Amount.ToString();
            txtUnitName.Value = supplier.CompanyName;
            txtBank.Value = supplier.Account_Bank;
            txtAccount.Value = supplier.Bank_Account;
            txtApplyBy.Value = payOrder.Requester;
            txtApplyDepart.Value = "�ɹ���";
            txtPayItem.Value = payOrder.Use_Of_Proceeds;
            if (invoice.Count > 0)
            {
                KeyValuePair<decimal, string> result = GetInvoiceAmount(invoice);
                txtInvoice.Value ="�ܽ�"+ result.Key +"(Ԫ)"+ ".   "+"��Ʊ��ţ�" + result.Value;
            }
            else
            {
                txtInvoice.Value = "���޷�Ʊ��Ϣ";
            }
           
        }

        private KeyValuePair<decimal,string> GetInvoiceAmount(SupplierInvoiceCollection invoice)
        {

            decimal all = 0;
            string number=string.Empty;
            for (int i = 0; i < invoice.Count; i++)
            {
                all += invoice[i].Total_Money;
                number += "[" + invoice[i].Invoice_Number + "]";
            }
            return new KeyValuePair<decimal, string>(all,number);
            
        }

        public string CmycurD(decimal num)
        {
            string str1 = "��Ҽ��������½��ƾ�";            //0-9����Ӧ�ĺ���
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�"; //����λ����Ӧ�ĺ���
            string str3 = "";    //��ԭnumֵ��ȡ����ֵ
            string str4 = "";    //���ֵ��ַ�����ʽ
            string str5 = "";  //����Ҵ�д�����ʽ
            int i;    //ѭ������
            int j;    //num��ֵ����100���ַ�������
            string ch1 = "";    //���ֵĺ������
            string ch2 = "";    //����λ�ĺ��ֶ���
            int nzero = 0;  //����������������ֵ�Ǽ���
            int temp;            //��ԭnumֵ��ȡ����ֵ

            num = Math.Round(Math.Abs(num), 2);    //��numȡ����ֵ����������ȡ2λС��
            str4 = ((long)(num * 100)).ToString();        //��num��100��ת�����ַ�����ʽ
            j = str4.Length;      //�ҳ����λ
            if (j > 15) { return "���"; }
            str2 = str2.Substring(15 - j);   //ȡ����Ӧλ����str2��ֵ���磺200.55,jΪ5����str2=��ʰԪ�Ƿ�

            //ѭ��ȡ��ÿһλ��Ҫת����ֵ
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //ȡ����ת����ĳһλ��ֵ
                temp = Convert.ToInt32(str3);      //ת��Ϊ����
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //����ȡλ����ΪԪ�����ڡ������ϵ�����ʱ
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "��" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //��λ�����ڣ��ڣ���Ԫλ�ȹؼ�λ
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "��" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //�����λ����λ��Ԫλ�������д��
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //���һλ���֣�Ϊ0ʱ�����ϡ�����
                    str5 = str5 + '��';
                }
            }
            if (num == 0)
            {
                str5 = "��Ԫ��";
            }
            return str5;
        }

        /**/
        /// <summary>
        /// һ�����أ����ַ�����ת���������ڵ���CmycurD(decimal num)
        /// </summary>
        /// <param name="num">�û�����Ľ��ַ�����ʽδת��decimal</param>
        /// <returns></returns>
        public string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "��������ʽ��";
            }
        }

    }
}