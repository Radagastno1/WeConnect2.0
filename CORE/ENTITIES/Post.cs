using System.ComponentModel;
namespace CORE.ENTITIES;
public class Post
{
    public readonly int ID;
    public string Content{get;set;}
    public DateTime DateCreated{get;set;}
    public int UserId{get;set;} 
    public string FirstName{get;set;}
    public string LastName{get;set;}
    List<Comment>comments = new();
    public Post(){}
    public Post(string aContent, DateTime aDateCreated, int aUserId)
    {
        Content = aContent;
        DateCreated = aDateCreated;
        UserId = aUserId;
    }

}