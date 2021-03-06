﻿using ReportTemplates.Transport;

namespace ReportTemplates.Models
{

    public class DeliveryOrderModel : PrintModel
    {
        public string mark { get; set; }
        public string delivery_no { get; set; }
        public int dealer_order_id { get; set; }
        public int waybill_sort { get; set; }
        public string waybill_no { get; set; }
        public string created { get; set; }
        public string related_no { get; set; }
        public string location_prefix { get; set; }
        public string location_address { get; set; }
        public string type { get; set; }
        public string location_consignee { get; set; }
        public string location_consignee_mobile { get; set; }
        public string detrusion_no { get; set; }
        public int box_count { get; set; }
        public string pay_money { get; set; }

        public string paidMoney { get; set; }
        public string deliveFee { get; set; }
        public string order_tip { get; set; }
        public string product_total { get; set; }
        public string total_this_order { get; set; }
        public string coupon { get; set; }
        public string other_discount { get; set; }
        public string pay_money_string { get; set; }

        public override string GenerateFile()
        {
            DeliveryOrder p = new DeliveryOrder();
            p.SetDateSource(this);
            return this.ExportPdf(p);
        }

        public decimal totalMoney { get; set; }
        public decimal discount { get; set; }
        public dItem[] data { get; set; }
    }

    public class dItem
    {
        public string order_number { get; set; }
        public string code { get; set; }
        public string product_name { get; set; }
        public string unit { get; set; }
        public int buy_qty { get; set; }
        public string is_gift { get; set; }
        public string price { get; set; }
        public float total { get; set; }
        public int actual { get; set; }
    }

}
