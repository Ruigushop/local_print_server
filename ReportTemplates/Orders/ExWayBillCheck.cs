using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ruigu.Crm.BL;
using Ruigu.Crm.BL.IntentOrder;
using Ruigu.Crm.BL.Transport;
using Ruigu.Crm.BL.WorkFlows;

namespace RuiguCrmReports.Orders
{

    /// <summary>
    /// Summary description for ExConsignOrder.
    /// </summary>
    public partial class ExWayBillCheck : Telerik.Reporting.Report
    {
        public ExWayBillCheck()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void SetDateSource(ConsignWaybill wayBillOrder)
        {

            DeliveryOrderCollection deliverOrders = DeliveryOrderCollection.GetByWayBillOrder(wayBillOrder.Waybill_No);
            List<ITransportableOrder> currentOrderItems = new  List<ITransportableOrder>();
            
            currentOrderItems.AddRange(TransportableOrderProvider.GetOrdersInWaybill(wayBillOrder.Waybill_No));
            //RuiguCustomerIntentOrderCollection currentOrderItems = RuiguCustomerIntentOrder.GetByWayBillOrder(wayBillOrder.Waybill_No);

            tCarNumber.Value = wayBillOrder.CarInfo.Plate_Number;
            tInstockTime.Value = wayBillOrder.Back_Time.ToString();
            tDriver.Value = wayBillOrder.Driver.True_Name;
            tConsignOrderNumber.Value = wayBillOrder.Returns_No;
            tOrderNumber.Value = wayBillOrder.Waybill_No;
            tSendDc.Value = wayBillOrder.DcInfo.Mname_True;
            tConsignTime.Value = wayBillOrder.Consigned_Time.ToString();
            tActuralRecivedMoney.Value = wayBillOrder.True_Total_Money_Final.ToString();
            var intentOrderTotalMoney = (from orders in currentOrderItems
                where orders.OrderType == "DELIVER_SYSTEM_ORDER"
                select orders).Sum(oo => oo.Total_Money_Final);
            tNeedPay.Value = string.Format("����{0}���ͻ�{1}", intentOrderTotalMoney, deliverOrders.TotalPayMoneyReal);
            tWarehouseManager.Value = wayBillOrder.Manager.True_Name;
            tCreateTime.Value = wayBillOrder.Created.ToString();
            //tremark.Value = wayBillOrder.Remark;
            
            #region �ͻ����˻ؼ������
            List<ReturnDataSource> dataSource = new List<ReturnDataSource>();
            int index = 1;
            foreach (DeliveryOrder dOrder in deliverOrders)
            {
                if (dOrder.Type != "PICK_AFTER_SALE" && dOrder.Status != 3)
                {
                    //������������Ϊ�ʹ����ʾ�˻�
                    foreach (DeliveryOrderItem deliverOrderItem in dOrder.Items)
                    {
                        ReturnDataSource data =
                                    new ReturnDataSource(dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Bianma,
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Product_Name,
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).ProductModel +
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Specification,
                                        deliverOrderItem.Shop_Amount_Min,
                                        deliverOrderItem.Unit_Name,
                                        0);

                        data.OwnerId = dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Owner_Id;
                        data.OwnerString = dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).OwnerString;
                        data.NeedInStock = true;
                        data.OrderNumber = dOrder.Related_No;
                        data.Name = dOrder.Location_Consignee;
                        data.Remark = dOrder.Remark;
                        dataSource.Add(data);
                    }
                }

                if (dOrder.Type == "PICK_AFTER_SALE" && dOrder.Status == 3)
                {
                    //������ɹ���ȡ�����ʾ�˻�
                    foreach (DeliveryOrderItem deliverOrderItem in dOrder.Items)
                    {
                        ReturnDataSource data =
                                    new ReturnDataSource(dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Bianma,
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Product_Name,
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).ProductModel +
                                        dOrder.Items.GetProductInfo(deliverOrderItem.Shop_Id).Specification,
                                        deliverOrderItem.Shop_Amount_Min,
                                        deliverOrderItem.Unit_Name,
                                        0);
                        data.OwnerId = dOrder.Items.GetProductInfoById(deliverOrderItem.Shop_Id).Owner_Id;
                        data.OwnerString = dOrder.Items.GetProductInfoById(deliverOrderItem.Shop_Id).OwnerString;
                        data.NeedInStock = false;
                        data.Remark = dOrder.Remark;
                        data.Name = dOrder.Location_Consignee;
                        data.OrderNumber = dOrder.Related_No;
                        dataSource.Add(data);
                    }
                }
            }
            lbPickBack.Value = "�ۺ�����������ͻ��˻�" + dataSource.Count + "SKU";

           this.tbDeliverReturnDetail.DataSource = dataSource;
            #endregion

            ExWayBillCheckIntentOrderDetail intentDetail = new ExWayBillCheckIntentOrderDetail(currentOrderItems);
            subIntentOrder.ReportSource = intentDetail;
            lbIntentOrderReject.Value = "�����˻���Ʒ" + wayBillOrder.ReturnDetail.Count + "SKU";
            subBackItems.ReportSource = new ExWayBillCheckItem(wayBillOrder.ReturnDetail);
            
        }
    }
}