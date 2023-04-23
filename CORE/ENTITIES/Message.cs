namespace CORE.ENTITIES;
public class Message
{
    public int ID{get;set;}
    public string Content{get;set;}
    public DateTime DateCreated{get;set;}
    public string Reciever{get;set;}
    public string Sender{get;set;}
    public int SenderId{get;set;}
    public int ConversationId{get;set;}
    public List<User>participants = new();
    public Message(){}
    public Message(string aContent, int aSenderId, int aConversationId)
    {
        Content = aContent;
        SenderId = aSenderId;
        ConversationId = aConversationId;
    }
}