namespace PetimOl.Models
{
    public class ChatModel
    {
        //MessageID,UserID,Message,MessageDate,UserSecondID,FullName,FUllanme,
        
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public int UserSecondID { get; set; }
        public string? Message { get; set; }
        public DateTime MessageDate { get; set; }
        public string? FullName { get; set; }
        public string? SecondFullName { get; set; }
    }
}
