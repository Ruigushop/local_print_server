using Ruigu.Crm.BL.Enums;
using Ruigu.Crm.BL.Supplier;

namespace RuiguCrmReports.Orders
{
    using Ruigu.Crm.BL.WorkFlows;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ExPurchaseReturnOrder.
    /// </summary>
    public partial class ExPurchaseReturnOrder : Telerik.Reporting.Report
    {
        public ExPurchaseReturnOrder()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        public void SetDateSource(SupplierReturnOrder purchasingOrder)
        {
            if (purchasingOrder.OrderType == (int) SupplierReturnOrderType.Return)
                txtTitle.Value = "�˻�������ͬ";
            if (purchasingOrder.OrderType == (int) SupplierReturnOrderType.Replace)
                txtTitle.Value = "����������ͬ";

            this.DataSource = purchasingOrder;
            this.tableOrderInfo.DataSource = purchasingOrder;
            this.tablePayInfo.DataSource = purchasingOrder;
            ExPurchaseReturnOrderItem itemReport = new ExPurchaseReturnOrderItem(purchasingOrder)
            {
                Width = this.subReport1.Width
            };
            //this.txtProductRequir.Value = string.Format("�������뱣֤��Ʒ��񡢿�ʽ���ϼ׷�Ҫ�󣬷��Ϲ��ҷ��ɷ��漰��ر�׼��֤�������κ��������⣬����Ҫ�е���Ӧ��ΥԼ���ü���ͬ����{0}%�����⳥�跽����ܵ���������ʧ��", Math.Round(purchasingOrder.Quality_Damages, 2) * 100);
            //this.txtPackageRequir.Value = string.Format("�����谴���跽Ҫ��İ�װ��ʽ����װ����������Ʒ���粻���跽ͬ������Ը��ģ��跽��Ȩ������������ܶ��{0}%��Ϊ�⳥��", Math.Round(purchasingOrder.Package_Damages, 2) * 100);
            //this.txtAssemblyRate.Value = string.Format("��������ṩ���跽����Ʒ�ṩ�ۺ����֧�֣����������跽���۶�{0}%������������ṩ�ۺ���֧�ֵȣ������ۺ����֧�������������ͬ����ʽ���в��䡣", purchasingOrder.Assembly_Preferential_Rate);
            //if (purchasingOrder.Payment_Type == (int)RuiguMemberPaymentType.PrePayment)
            //{
            //    decimal payRate = purchasingOrder.Total_Amount == 0
            //        ? 0
            //        : Math.Round(purchasingOrder.Advance_Payment_Amount / purchasingOrder.Total_Amount, 2) * 100;
            //    txtPayMethod.Value = string.Format("����˫��ǩ����ͬ���跽�򹩷������ܻ����{0}%��{1}Ԫ��ΪԤ��������������跽���պϸ���ٽ���β�", payRate, Math.Round(purchasingOrder.Advance_Payment_Amount, 2));
            //}
            //else
            //{
            //    txtPayMethod.Value = string.Format("�ڹ�Ӧ�����ﵽ������ͨ���跽�ֿ����ջ��跽ָ�������պ��跽�򹩷�����ȫ����{0}Ԫ", purchasingOrder.Total_Amount);
            //}
            this.subReport1.ReportSource = itemReport;
        }
    }
}