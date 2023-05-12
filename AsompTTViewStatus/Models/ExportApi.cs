namespace AsompTTViewStatus.Models
{
    public class ExportApi
    {
        //public decimal ID { get; set; }
        public decimal File_Id { get; set; }
        public string Barcode { get; set; }
        public string DataStr { get; set; }
        public string Dateins { get; set; }
        public Int16 State { get; set; }
        public string? Message { get; set; }
    }
}
